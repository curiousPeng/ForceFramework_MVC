using Force.Common.LightMessager.Common;
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
    public sealed class RabbitMQProducer : IRabbitMQProducer
    {
        static ConnectionFactory factory;
        static IConnection connection;
        static volatile int prepersist_count;
        static readonly int default_retry_wait;
        static readonly int default_retry_count;
        static List<long> prepersist;
        static BlockingQueue<BaseMessage> direct_queue;
        static BlockingQueue<BaseMessage> topic_queue;
        static BlockingQueue<BaseMessage> fanout_queue;
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
        public RabbitMQProducer(ConnectionModel connectionModel)
        {
            factory = new ConnectionFactory();
            factory.UserName = connectionModel.UserName; // "admin";
            factory.Password = connectionModel.Password; // "123456";
            factory.VirtualHost = connectionModel.VirtualHost; // "/";
            factory.HostName = connectionModel.HostName; // "127.0.0.1";
            factory.Port = connectionModel.Port; // 5672;
            factory.AutomaticRecoveryEnabled = connectionModel.AutomaticRecoveryEnabled;//true
            factory.NetworkRecoveryInterval = connectionModel.NetworkRecoveryInterval;//15
            connection = factory.CreateConnection();
        }
        static RabbitMQProducer()
        {
            prepersist_count = 0;
            default_retry_wait = 1000; // 1秒
            default_retry_count = 3; // 消息重试最大3次
            prepersist = new List<long>();
            direct_queue = new BlockingQueue<BaseMessage>(1000);
            topic_queue = new BlockingQueue<BaseMessage>(1000);
            fanout_queue = new BlockingQueue<BaseMessage>(1000);
            pools = new ConcurrentDictionary<Type, ObjectPool<IPooledWapper>>();

            //开启轮询检测，扫描重试队列，重发消息
            new Thread(() =>
            {
                while (true)
                {
                    BaseMessage send_item;
                    direct_queue.Dequeue(out send_item);
                    SendDirect(send_item);
                }
            }).Start();
            new Thread(() =>
            {
                while (true)
                {
                    BaseMessage pub_item;
                    topic_queue.Dequeue(out pub_item);
                    SendTopic(pub_item);
                }
            }).Start();
            new Thread(() =>
            {
                while (true)
                {
                    BaseMessage pub_item_fanout;
                    fanout_queue.Dequeue(out pub_item_fanout);
                    SendFanout(pub_item_fanout);
                }
            }).Start();
        }

        /// <summary>
        /// 发送一条消息，默认的direct模式,自定义exchangeName,queueName
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exchangeName">exchangeName</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routeKey">路由键，不定义就默认使用队列名做路由键</param>
        /// <param name="delaySend">延迟多少毫秒发送消息,一般不低于5000</param>
        /// <returns>发送成功返回true，否则返回false</returns>
        public bool DirectSend(BaseMessage message, int delaySend = 0)
        {
            return SendDirect(message, delaySend);
        }

        /// <summary>
        /// topic模式发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exchangeName">exchangeName</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="delaySend">延迟多少毫秒发布消息</param>
        /// <returns>发布成功返回true，否则返回false</returns>
        public bool TopicSend(BaseMessage message, int delaySend = 0)
        {
            return SendTopic(message, delaySend);
        }

        /// <summary>
        /// fanout模式发布消息
        /// 此模式适合两种及以上业务用同一条消息订阅使用，如果只有一种业务或者单机建议用默认模式
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="delaySend"></param>
        /// <returns></returns>
        public bool FanoutSend(BaseMessage message, int delaySend = 0)
        {
            return SendFanout(message, delaySend);
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
            delaySend = delaySend == 0 ? delaySend : Math.Max(delaySend, 5000); // 至少保证5秒的延迟，否则意义不大
            using (var pooled = InnerCreateChannel(messageType))
            {
                IModel channel = pooled.Channel;
                pooled.PreRecord(message.MsgHash);

                var exchange = string.Empty;
                var queue = string.Empty;
                if (!string.IsNullOrEmpty(message.exchangeName) && !string.IsNullOrEmpty(message.queueName) && !string.IsNullOrEmpty(message.routeKey))
                {
                    exchange = message.exchangeName;
                    queue = message.queueName;
                    EnsureQueue.DirectEnsureQueue(channel, ref exchange, ref queue, message.routeKey, delaySend);
                }
                else
                {
                    EnsureQueue.DirectEnsureQueue(channel, messageType, out exchange, out queue, delaySend);
                }

                var route_key = queue;
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
        private static bool SendTopic(BaseMessage message, int delaySend = 0)
        {
            if (string.IsNullOrWhiteSpace(message.Source))
            {
                throw new ArgumentNullException("message.Source");
            }

            if (string.IsNullOrWhiteSpace(message.routeKey))
            {
                throw new ArgumentNullException("routeKey");
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
                var queue = string.Empty;
                if (!string.IsNullOrEmpty(message.exchangeName) && !string.IsNullOrEmpty(message.queueName))
                {
                    exchange = message.exchangeName;
                    queue = message.queueName;
                    EnsureQueue.TopicEnsureQueue(channel, message.routeKey, ref exchange, ref queue, delaySend);
                }
                else
                {
                    EnsureQueue.TopicEnsureQueue(channel, messageType, message.routeKey, out exchange, out queue, delaySend);
                }

                var json = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                var props = channel.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2;
                channel.BasicPublish(exchange, message.routeKey, props, bytes);
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
        private static bool SendFanout(BaseMessage message, int delaySend = 0)
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
                if (!string.IsNullOrEmpty(message.exchangeName))
                {
                    exchange = message.exchangeName;
                    EnsureQueue.FanoutEnsureQueue(channel, ref exchange, delaySend);
                }
                else
                {
                    EnsureQueue.FanoutEnsureQueue(channel, messageType, out exchange, delaySend);
                }
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
                    if (Interlocked.Increment(ref prepersist_count) == 1000)
                    {
                        prepersist.RemoveRange(0, 950);
                    }
                    prepersist.Add(msgHash);

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

        public static long GenerateMessageIdFrom(string str)
        {
            return str.GetHashCode();
        }
    }
}
