using Force.Common.LightMessager.DAL;
using Force.Common.LightMessager.DAL.Model;
using Force.Common.LightMessager.Message;
using Force.Common.LightMessager.Pool;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Force.Common.LightMessager.Helper
{
    public sealed class RabbitMQProducer: IRabbitMQProducer
    {
        static ConnectionFactory factory;
        static IConnection connection;
        static volatile int prepersist_count;
        static readonly int default_retry_wait;
        static readonly int default_retry_count;
        static List<long> prepersist;
        static ConcurrentQueue<BaseMessage> direct_queue;
        static ConcurrentQueue<BaseMessage> topic_queue;
        static ConcurrentQueue<BaseMessage> fanout_queue;
        static ConcurrentDictionary<Type, QueueInfo> dict_info;
        static ConcurrentDictionary<Type, string> dict_info_fanout;
        static ConcurrentDictionary<Type, ObjectPool<IPooledWapper>> pools;
        private static IMessageQueueHelper _message_queue_helper;

        public RabbitMQProducer(IConfiguration configurationRoot, IMessageQueueHelper messageQueueHelper)
        {
            factory = new ConnectionFactory();
            factory.UserName = configurationRoot.GetSection("LightMessager:UserName").Value; // "admin";
            factory.Password = configurationRoot.GetSection("LightMessager:Password").Value; // "123456";
            factory.VirtualHost = configurationRoot.GetSection("LightMessager:VirtualHost").Value; // "/";
            factory.HostName = configurationRoot.GetSection("LightMessager:HostName").Value; // "127.0.0.1";
            factory.Port = int.Parse(configurationRoot.GetSection("LightMessager:Port").Value); // 5672;
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);
            connection = factory.CreateConnection();
            _message_queue_helper = messageQueueHelper;
        }
        static RabbitMQProducer()
        {
            prepersist_count = 0;
            default_retry_wait = 1000; // 1秒
            default_retry_count = 3; // 消息重试最大3次
            prepersist = new List<long>();
            direct_queue = new ConcurrentQueue<BaseMessage>();
            topic_queue = new ConcurrentQueue<BaseMessage>();
            fanout_queue = new ConcurrentQueue<BaseMessage>();
            dict_info = new ConcurrentDictionary<Type, QueueInfo>();
            dict_info_fanout = new ConcurrentDictionary<Type, string>();
            pools = new ConcurrentDictionary<Type, ObjectPool<IPooledWapper>>();

            //开启轮询检测，扫描重试队列，重发消息
            new Thread(() =>
            {
                // 先实现为spin的方式，后面考虑换成blockingqueue的方式
                while (true)
                {
                    BaseMessage send_item;
                    while (direct_queue.TryDequeue(out send_item))
                    {
                        SendDirect(send_item);
                    }

                    BaseMessage pub_item;
                    while (topic_queue.TryDequeue(out pub_item))
                    {
                        SendTopic(pub_item, pub_item.Pattern);
                    }

                    BaseMessage pub_item_fanout;
                    while (fanout_queue.TryDequeue(out pub_item_fanout))
                    {
                        SendFanout(pub_item_fanout);
                    }
                    Thread.Sleep(1000 * 5);
                }
            }).Start();
        }

        /// <summary>
        /// 发送一条消息
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="message">消息</param>
        /// <param name="delaySend">延迟多少毫秒发送消息</param>
        /// <returns>发送成功返回true，否则返回false</returns>
        public bool Send(BaseMessage message, int delaySend = 0)
        {
            return SendDirect(message, delaySend);
        }

        /// <summary>
        /// 发布一条消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message">消息</param>
        /// <param name="pattern">消息满足的模式（也就是routeKey）</param>
        /// <param name="delaySend">延迟多少毫秒发布消息</param>
        /// <returns>发布成功返回true，否则返回false</returns>
        public bool Publish(BaseMessage message, string pattern, int delaySend = 0)
        {
            return SendTopic(message, pattern, delaySend);
        }

        /// <summary>
        /// fanout模式发布消息,此模式适合两种及以上业务用同一条消息订阅使用，如果只有一种业务或者单机建议用默认模式
        /// </summary>
        /// <param name="message"></param>
        /// <param name="delaySend"></param>
        /// <returns></returns>
        public bool FanoutPublish(BaseMessage message)
        {
            return SendFanout(message);
        }

        private static bool SendDirect(BaseMessage message, int delaySend = 0)
        {
            if (string.IsNullOrWhiteSpace(message.Source))
            {
                throw new ArgumentNullException("message.Source");
            }

            if (!PrePersistMessage(message))
            {
                return false;
            }

            var messageType = message.GetType();
            delaySend = delaySend == 0 ? delaySend : Math.Max(delaySend, 1000); // 至少保证1秒的延迟，否则意义不大
            using (var pooled = InnerCreateChannel(messageType))
            {
                IModel channel = pooled.Channel;
                pooled.PreRecord(message.MsgHash);

                var exchange = string.Empty;
                var route_key = string.Empty;
                var queue = string.Empty;
                EnsureQueue(channel, messageType, out exchange, out route_key, out queue, delaySend);

                var json = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                var props = channel.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2;
                channel.BasicPublish(exchange, route_key, props, bytes);
                var time_out = Math.Max(default_retry_wait, message.RetryCount_Publish * 2 /*2倍往上扩大，防止出现均等*/ * 1000);
                var ret = channel.WaitForConfirms(TimeSpan.FromMilliseconds(time_out));
                if (!ret)
                {
                    // 数据库更新该条消息的状态信息
                    if (message.RetryCount_Publish < default_retry_count)
                    {
                        var ok = _message_queue_helper.Update(
                            message.MsgHash,
                            fromStatus1: MsgStatus.Created, // 之前的状态只能是1 Created 或者2 Retry
                            fromStatus2: MsgStatus.Retrying,
                            toStatus: MsgStatus.Retrying);
                        if (ok)
                        {
                            message.RetryCount_Publish += 1;
                            message.LastRetryTime = DateTime.Now;
                            direct_queue.Enqueue(message);
                            return true;
                        }
                        throw new Exception("数据库update出现异常");
                    }
                    throw new Exception($"消息发送超过最大重试次数（{default_retry_count}次）");
                }
            }

            return true;
        }

        private static bool SendTopic(BaseMessage message, string pattern, int delaySend = 0)
        {
            if (string.IsNullOrWhiteSpace(message.Source))
            {
                throw new ArgumentNullException("message.Source");
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentNullException("pattern");
            }

            if (!PrePersistMessage(message))
            {
                return false;
            }

            var messageType = message.GetType();
            delaySend = delaySend == 0 ? delaySend : Math.Max(delaySend, 1000); // 至少保证1秒的延迟，否则意义不大
            using (var pooled = InnerCreateChannel(messageType))
            {
                IModel channel = pooled.Channel;
                pooled.PreRecord(message.MsgHash);

                var exchange = string.Empty;
                var route_key = string.Empty;
                var queue = string.Empty;
                EnsureQueue(channel, messageType, out exchange, out route_key, out queue, pattern, delaySend);

                var json = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                var props = channel.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2;
                channel.BasicPublish(exchange, pattern, props, bytes);
                var time_out = Math.Max(default_retry_wait, message.RetryCount_Publish * 2 /*2倍往上扩大，防止出现均等*/ * 1000);
                var ret = channel.WaitForConfirms(TimeSpan.FromMilliseconds(time_out));
                if (!ret)
                {
                    if (message.RetryCount_Publish < default_retry_count)
                    {
                        var ok = _message_queue_helper.Update(
                             message.MsgHash,
                             fromStatus1: MsgStatus.Created, // 之前的状态只能是1 Created 或者2 Retry
                             fromStatus2: MsgStatus.Retrying,
                             toStatus: MsgStatus.Retrying);
                        if (ok)
                        {
                            message.RetryCount_Publish += 1;
                            message.LastRetryTime = DateTime.Now;
                            message.Pattern = pattern;
                            topic_queue.Enqueue(message);
                            return true;
                        }
                        throw new Exception("数据库update出现异常");
                    }
                    throw new Exception($"消息发送超过最大重试次数（{default_retry_count}次）");
                }
            }

            return true;
        }

        private static bool SendFanout(BaseMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Source))
            {
                throw new ArgumentNullException("message.Source");
            }

            ///fanout模式不同其他模式，又无法获取到订阅者数量，于是直接默认CanbeRemove为true
            ///如果出现异常在设为false，具体在哪儿失败了，怎么恢复就需要调用者自己去查了
            ///数据库消息只提供一个记录，供查证用
            if (!PrePersistMessage(message, true))
            {
                return false;
            }

            var messageType = message.GetType();
            using (var pooled = InnerCreateChannel(messageType))
            {
                IModel channel = pooled.Channel;
                // pooled.PreRecord(message.MsgHash);无需修改状态了

                var exchange = string.Empty;
                PublishEnsureQueue(channel, messageType, out exchange);

                var json = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                channel.BasicPublish(exchange, "", props, bytes);
                var time_out = Math.Max(default_retry_wait, message.RetryCount_Publish * 2 /*2倍往上扩大，防止出现均等*/ * 1000);
                var ret = channel.WaitForConfirms(TimeSpan.FromMilliseconds(time_out));
                if (!ret)
                {
                    if (message.RetryCount_Publish < default_retry_count)
                    {
                        var ok = _message_queue_helper.Update(
                             message.MsgHash,
                             fromStatus1: MsgStatus.Created, // 之前的状态只能是1 Created 或者2 Retry
                             fromStatus2: MsgStatus.Retrying,
                             toStatus: MsgStatus.Retrying);
                        if (ok)
                        {
                            message.RetryCount_Publish += 1;
                            message.LastRetryTime = DateTime.Now;
                            fanout_queue.Enqueue(message);
                            return true;
                        }
                        throw new Exception("数据库update出现异常");
                    }
                    throw new Exception($"消息发送超过最大重试次数（{default_retry_count}次）");
                }
            }

            return true;
        }

        private static PooledChannel InnerCreateChannel(Type messageType)
        {
            var pool = pools.GetOrAdd(
                messageType,
                t => new ObjectPool<IPooledWapper>(p => new PooledChannel(connection.CreateModel(), p, _message_queue_helper), 10));
            return pool.Get() as PooledChannel;
        }

        private static bool PrePersistMessage(BaseMessage message, bool isFanout = false)
        {
            if (message.RetryCount_Publish == 0)
            {
                var msgHash = GenerateMessageIdFrom(message.Source);
                if (prepersist.Contains(msgHash))
                {
                    return false;
                }
                else
                {
                    message.MsgHash = msgHash;
                    if (Interlocked.Increment(ref prepersist_count) != 1000)
                    {
                        prepersist.Add(msgHash);
                    }
                    else
                    {
                        prepersist.RemoveRange(0, 950);
                    }

                    var model = _message_queue_helper.GetModelBy(msgHash);
                    if (model != null)
                    {
                        return false;
                    }
                    else
                    {
                        var new_model = new MessageQueue
                        {
                            MsgHash = msgHash,
                            MsgContent = message.Source,
                            RetryCount = 0,
                            CanBeRemoved = false,
                            Status = MsgStatus.Created,
                            CreatedTime = DateTime.Now
                        };
                        if (isFanout)
                        {
                            new_model.CanBeRemoved = true;
                        }
                        _message_queue_helper.Insert(new_model);
                        return true;
                    }
                }
            }
            else // RetryCount > 0
            {
                // 直接返回true，以便后续可以进行重发
                return true;
            }
        }

        private static void EnsureQueue(IModel channel, Type messageType, out string exchange, out string routeKey, out string queue, int delaySend = 0)
        {
            var type = messageType;
            if (!dict_info.ContainsKey(type))
            {
                var info = GetQueueInfo(type);
                exchange = info.Exchange;
                routeKey = info.DefaultRouteKey;
                queue = info.Queue;

                channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
                channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queue, exchange, routeKey);

                if (delaySend > 0)
                {
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", exchange);
                    args.Add("x-dead-letter-routing-key", queue);
                    channel.QueueDeclare(queue + ".delay", durable: false, exclusive: false, autoDelete: false, arguments: args);
                    exchange = string.Empty;
                    routeKey = info.Queue + ".delay";
                    queue = info.Queue + ".delay";
                }
            }
            else
            {
                var info = GetQueueInfo(type);
                exchange = info.Exchange;
                routeKey = info.DefaultRouteKey;
                queue = info.Queue;
                if (delaySend > 0)
                {
                    exchange = string.Empty;
                    routeKey = info.Queue + ".delay";
                    queue = info.Queue + ".delay";
                }
            }
        }

        private static void EnsureQueue(IModel channel, Type messageType, out string exchange, out string routeKey, out string queue, string pattern, int delaySend = 0)
        {
            var type = messageType;
            if (!dict_info.ContainsKey(type))
            {
                var info = GetQueueInfo(type);
                exchange = "topic." + info.Exchange;
                routeKey = pattern;
                queue = info.Queue;
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);

                if (delaySend > 0)
                {
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", exchange);
                    args.Add("x-dead-letter-routing-key", pattern);
                    channel.QueueDeclare(queue + ".delay", durable: true, exclusive: false, autoDelete: false, arguments: args);
                    exchange = string.Empty;
                    routeKey = info.Queue + ".delay";
                    queue = info.Queue + ".delay";
                }
            }
            else
            {
                var info = GetQueueInfo(type);
                exchange = "topic." + info.Exchange;
                routeKey = pattern;
                queue = info.Queue;
                if (delaySend > 0)
                {
                    exchange = string.Empty;
                    routeKey = info.Queue + ".delay";
                    queue = info.Queue + ".delay";
                }
            }
        }

        private static void PublishEnsureQueue(IModel channel, Type messageType, out string exchange)
        {
            var type = messageType;
            if (!dict_info_fanout.ContainsKey(type))
            {
                var info = GetQueueInfoForFanout(messageType);
                exchange = info;
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);
            }
            else
            {
                var info = GetQueueInfoForFanout(messageType);
                exchange = info;
            }
        }

        private static QueueInfo GetQueueInfo(Type messageType)
        {
            var type_name = messageType.IsGenericType ? messageType.GenericTypeArguments[0].Name : messageType.Name;
            var info = dict_info.GetOrAdd(messageType, t => new QueueInfo
            {
                Exchange = type_name + ".exchange",
                DefaultRouteKey = type_name + ".input",
                Queue = type_name + ".input"
            });

            return info;
        }

        private static string GetQueueInfoForFanout(Type messageType)
        {
            var type_name = messageType.IsGenericType ? messageType.GenericTypeArguments[0].Name : messageType.Name;
            var info = dict_info_fanout.GetOrAdd(messageType, type_name + ".exchange");

            return info;
        }

        public static long GenerateMessageIdFrom(string str)
        {
            return str.GetHashCode();
        }
    }
}
