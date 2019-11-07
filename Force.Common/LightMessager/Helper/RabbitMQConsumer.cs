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
    /* 
     * links: 
     * https://www.rabbitmq.com/dotnet-api-guide.html
     * https://www.rabbitmq.com/queues.html
     * https://www.rabbitmq.com/confirms.html
     * https://stackoverflow.com/questions/4444208/delayed-message-in-rabbitmq
    */
    public sealed class RabbitMQHelper
    {
        static ConnectionFactory factory;
        static IConnection connection;
        static ConcurrentDictionary<Type, QueueInfo> dict_info;
        static ConcurrentDictionary<Type, object> dict_func;
        static ConcurrentDictionary<(Type, string), QueueInfo> dict_info_name; // with name
        static ConcurrentDictionary<(Type, string), object> dict_func_name; // with name
        static ConcurrentDictionary<Type, string> dict_info_fanout;
        static ConcurrentDictionary<Type, object> dict_func_fanout;
        static readonly ushort prefetch_count;
        static Logger _logger = LogManager.GetLogger("RabbitMQHelper");
        private static IMessageQueueHelper _message_queue_helper;

        public RabbitMQHelper(IConfiguration configurationRoot, IMessageQueueHelper messageQueueHelper)
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
        static RabbitMQHelper()
        {
            prefetch_count = 100;
            dict_info = new ConcurrentDictionary<Type, QueueInfo>();
            dict_func = new ConcurrentDictionary<Type, object>();
            dict_info_name = new ConcurrentDictionary<(Type, string), QueueInfo>();
            dict_func_name = new ConcurrentDictionary<(Type, string), object>();
            dict_info_fanout = new ConcurrentDictionary<Type, string>();
            dict_func_fanout = new ConcurrentDictionary<Type, object>();
        }

        /// <summary>
        /// 注册消息处理器
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="THandler">消息处理器类型</typeparam>
        /// <param name="redeliveryCheck">是否开启重发确认；如果消息处理器逻辑已经实现为幂等则不需要开启以便节省计算资源，否则请打开该选项</param>
        public void RegisterHandler<TMessage, THandler>(bool redeliveryCheck = false)
            where THandler : BaseHandleMessages<TMessage>
            where TMessage : BaseMessage
        {
            try
            {
                var type = typeof(TMessage);
                if (!dict_func.ContainsKey(type))
                {
                    var handler = dict_func.GetOrAdd(type, t => Activator.CreateInstance<THandler>()) as THandler;
                    var channel = connection.CreateModel();
                    var consumer = new EventingBasicConsumer(channel);
                    /*
                      @param prefetchSize maximum amount of content (measured in octets) that the server will deliver, 0 if unlimited
                      @param prefetchCount maximum number of messages that the server will deliver, 0 if unlimited
                      @param global true if the settings should be applied to the entire channel rather than each consumer
                    */
                    channel.BasicQos(0, prefetch_count, false);
                    consumer.Received += async (model, ea) =>
                    {
                        var json = Encoding.UTF8.GetString(ea.Body);
                        var msg = JsonConvert.DeserializeObject<TMessage>(json);
                        if (redeliveryCheck)
                        {
                            if (!ea.Redelivered) // 之前一定没有处理过该条消息
                            {
                                await handler.Handle(msg);
                                if (msg.NeedNAck)
                                {
                                    channel.BasicNack(ea.DeliveryTag, false, true);
                                    _message_queue_helper.Update(
                                    msg.MsgHash,
                                    fromStatus: MsgStatus.ArrivedBroker,
                                    toStatus: MsgStatus.Exception);
                                }
                                else
                                {
                                    channel.BasicAck(ea.DeliveryTag, false);
                                    _message_queue_helper.Update(
                                    msg.MsgHash,
                                    fromStatus: MsgStatus.ArrivedBroker,
                                    toStatus: MsgStatus.Consumed);
                                }
                            }
                            else
                            {
                                var m = _message_queue_helper.GetModelBy(msg.MsgHash);
                                if (m.Status == MsgStatus.Exception)
                                {
                                    await handler.Handle(msg);
                                    if (msg.NeedNAck)
                                    {
                                        channel.BasicNack(ea.DeliveryTag, false, true);
                                    }
                                    else
                                    {
                                        channel.BasicAck(ea.DeliveryTag, false);
                                    }
                                }
                                else if (m.Status == MsgStatus.ArrivedBroker)
                                {
                                    // 相对特殊的一种情况，Redelivered为true，但是本地消息实际上只到达第三档状态
                                    // 说明在消息刚从broker出来，rabbitmq重置了链接
                                    await handler.Handle(msg);
                                    if (msg.NeedNAck)
                                    {
                                        channel.BasicNack(ea.DeliveryTag, false, true);
                                    }
                                    else
                                    {
                                        channel.BasicAck(ea.DeliveryTag, false);
                                    }
                                }
                                else
                                {
                                    // 为了保持幂等这里不做任何处理
                                }
                            }
                        }
                        else
                        {
                            await handler.Handle(msg);
                            if (msg.NeedNAck)
                            {
                                channel.BasicNack(ea.DeliveryTag, false, true);
                            }
                            else
                            {
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                        }
                    };

                    var exchange = string.Empty;
                    var route_key = string.Empty;
                    var queue = string.Empty;
                    EnsureQueue(channel, type, out exchange, out route_key, out queue);
                    channel.BasicConsume(queue, false, consumer);
                }
            }
            catch (Exception ex)
            {
                _logger.Debug("RegisterHandler()出错，异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 注册消息处理器，根据模式匹配接收消息
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="THandler">消息处理器类型</typeparam>
        /// <param name="subscriberName">订阅器的名称</param>
        /// <param name="redeliveryCheck">是否开启重发确认；如果消息处理器逻辑已经实现为幂等则不需要开启以便节省计算资源，否则请打开该选项</param>
        /// <param name="subscribePatterns">订阅器支持的消息模式</param>
        public void RegisterHandlerAs<TMessage, THandler>(string subscriberName, bool redeliveryCheck = false, params string[] subscribePatterns)
            where THandler : BaseHandleMessages<TMessage>
            where TMessage : BaseMessage
        {
            if (string.IsNullOrWhiteSpace(subscriberName))
            {
                throw new ArgumentNullException("subscriberName");
            }

            if (subscribePatterns == null || subscribePatterns.Length == 0)
            {
                throw new ArgumentNullException("subscribePatterns");
            }

            try
            {
                var key = (typeof(TMessage), subscriberName);
                if (!dict_func_name.ContainsKey(key))
                {
                    var handler = dict_func_name.GetOrAdd((typeof(TMessage), subscriberName), p => Activator.CreateInstance<THandler>()) as THandler;
                    var channel = connection.CreateModel();
                    var consumer = new EventingBasicConsumer(channel);
                    /*
                      @param prefetchSize maximum amount of content (measured in octets) that the server will deliver, 0 if unlimited
                      @param prefetchCount maximum number of messages that the server will deliver, 0 if unlimited
                      @param global true if the settings should be applied to the entire channel rather than each consumer
                    */
                    channel.BasicQos(0, prefetch_count, false);
                    consumer.Received += async (model, ea) =>
                    {
                        var json = Encoding.UTF8.GetString(ea.Body);
                        var msg = JsonConvert.DeserializeObject<TMessage>(json);
                        if (redeliveryCheck)
                        {
                            if (!ea.Redelivered) // 之前一定没有处理过该条消息
                            {
                                await handler.Handle(msg);
                                if (msg.NeedNAck)
                                {
                                    channel.BasicNack(ea.DeliveryTag, false, true);
                                    _message_queue_helper.Update(
                                    msg.MsgHash,
                                    fromStatus: MsgStatus.ArrivedBroker,
                                    toStatus: MsgStatus.Exception);
                                }
                                else
                                {
                                    channel.BasicAck(ea.DeliveryTag, false);
                                    _message_queue_helper.Update(
                                    msg.MsgHash,
                                    fromStatus: MsgStatus.ArrivedBroker,
                                    toStatus: MsgStatus.Consumed);
                                }
                            }
                            else
                            {
                                var m = _message_queue_helper.GetModelBy(msg.MsgHash);
                                if (m.Status == MsgStatus.Exception)
                                {
                                    await handler.Handle(msg);
                                    if (msg.NeedNAck)
                                    {
                                        channel.BasicNack(ea.DeliveryTag, false, true);
                                    }
                                    else
                                    {
                                        channel.BasicAck(ea.DeliveryTag, false);
                                    }
                                }
                                else if (m.Status == MsgStatus.ArrivedBroker)
                                {
                                    // 相对特殊的一种情况，Redelivered为true，但是本地消息实际上只到达第三档状态
                                    // 说明在消息刚从broker出来，rabbitmq重置了链接
                                    await handler.Handle(msg);
                                    if (msg.NeedNAck)
                                    {
                                        channel.BasicNack(ea.DeliveryTag, false, true);
                                    }
                                    else
                                    {
                                        channel.BasicAck(ea.DeliveryTag, false);
                                    }
                                }
                                else
                                {
                                    // 为了保持幂等这里不做任何处理
                                }
                            }
                        }
                        else
                        {
                            await handler.Handle(msg);
                            if (msg.NeedNAck)
                            {
                                channel.BasicNack(ea.DeliveryTag, false, true);
                            }
                            else
                            {
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                        }
                    };

                    var exchange = string.Empty;
                    var queue = string.Empty;
                    EnsureQueue(channel, typeof(TMessage), subscriberName, out exchange, out queue, subscribePatterns);
                    channel.BasicConsume(queue, false, consumer);
                }
            }
            catch (Exception ex)
            {
                _logger.Debug("RegisterHandler(string subscriberName)出错，异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 注册消息处理器,fanout模式
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="THandler">消息处理器类型</typeparam>
        public void RegisterHandlerForFanout<TMessage, THandler>()
            where THandler : BaseHandleMessages<TMessage>
            where TMessage : BaseMessage
        {
            try
            {
                var type = typeof(TMessage);
                if (!dict_func.ContainsKey(type))
                {
                    var handler = dict_func_fanout.GetOrAdd(type, t => Activator.CreateInstance<THandler>()) as THandler;
                    var channel = connection.CreateModel();
                    var consumer = new EventingBasicConsumer(channel);

                    var exchange = string.Empty;
                    var queue = string.Empty;
                    ConsumerEnsureQueue(channel, type, out exchange, out queue);
                    TMessage msg = null;//闭包获取数据
                    try
                    {
                        consumer.Received += async (model, ea) =>
                        {
                            var json = Encoding.UTF8.GetString(ea.Body);
                            msg = JsonConvert.DeserializeObject<TMessage>(json);
                            await handler.Handle(msg);
                        };
                    }
                    catch (Exception ex)
                    {
                        _message_queue_helper.UpdateCanbeRemoveIsFalse(msg.MsgHash);
                        _logger.Error("RegisterHandler()出错，异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
                    }

                    channel.BasicConsume(queue, true, consumer);
                }
            }
            catch (Exception ex)
            {
                _logger.Debug("RegisterHandler()出错，异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
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

        private static void EnsureQueue(IModel channel, Type messageType, string subscriberName, out string exchange, out string queue, params string[] subscribePatterns)
        {
            var key = (messageType, subscriberName);
            if (!dict_info_name.ContainsKey(key))
            {
                var info = GetQueueInfo(messageType, subscriberName);
                exchange = "topic." + info.Exchange;
                queue = info.Queue + "." + subscriberName;
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
                channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
                foreach (var pattern in subscribePatterns)
                {
                    channel.QueueBind(queue, exchange, routingKey: pattern);
                }
            }
            else
            {
                var info = GetQueueInfo(messageType, subscriberName);
                exchange = "topic." + info.Exchange;
                queue = info.Queue + "." + subscriberName;
            }
        }

        private static void ConsumerEnsureQueue(IModel channel, Type messageType, out string exchange, out string queue)
        {
            var info = GetQueueInfoForFanout(messageType);
            exchange = info;
            channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);
            queue = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queue, exchange: exchange, routingKey: "", arguments: null);

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

        private static QueueInfo GetQueueInfo(Type messageType, string name)
        {
            var type_name = messageType.IsGenericType ? messageType.GenericTypeArguments[0].Name + "[" + name + "]" : messageType.Name;
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
    }
}
