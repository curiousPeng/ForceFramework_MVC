using Force.Common.LightMessager.Pool;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.LightMessager.Helper
{
    internal class EnsureQueue
    {
        static ConcurrentDictionary<Type, QueueInfo> dict_info;
        static ConcurrentDictionary<string, QueueInfo> dict_info_custom;
        static EnsureQueue()
        {
            dict_info = new ConcurrentDictionary<Type, QueueInfo>();
            dict_info_custom = new ConcurrentDictionary<string, QueueInfo>();

        }
        public static void DirectEnsureQueue(IModel channel, Type messageType, out string exchange, out string queue, int delaySend = 0)
        {
            var type = messageType;
            if (delaySend > 0)
            {//走新加延迟队列
                var type_name = messageType.IsGenericType ? messageType.GenericTypeArguments[0].Name : messageType.Name;
                var key = type_name + ".exchange.delay";
                exchange = key;
                queue = type_name + ".input.delay";
                if (!dict_info_custom.ContainsKey(key))
                {
                    var routeKey = type_name + ".input";
                    GetQueueInfoForCustom(key, exchange, queue, routeKey);
                    channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", type_name + ".exchange");//死掉过后转发到的exchange
                    args.Add("x-dead-letter-routing-key", routeKey);//转发的exchange的路由键
                    channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: args);
                    channel.QueueBind(queue, exchange, routeKey, null);
                }
            }
            else
            {
                if (!dict_info.ContainsKey(type))
                {
                    var info = GetQueueInfo(type);//不能放外面，这个方法会往dict_info里面记录，否则永远建不起exchange
                    exchange = info.Exchange;
                    queue = info.Queue;
                    channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
                    channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(queue, exchange, queue);
                }
                else
                {
                    var info = GetQueueInfo(type);
                    exchange = info.Exchange;
                    queue = info.Queue;
                }
            }
        }
        public static void DirectEnsureQueue(IModel channel, ref string exchange, ref string queue, string routeKey = null, int delaySend = 0)
        {
            string key;
            string realExchange = exchange;
            string realQueue = queue;
            if (delaySend > 0)
            {
                exchange = exchange + ".delay";
                queue = queue + ".delay";
            }
            key = queue;///为什么这儿是queue，而上面是exchange，因为上面是工具定义的，
                        ///只要是同一个消息结构，上面的queueName是定死了，所以直接返回Exchange就行了
                        ///而下面是用户自定义的queue，同一个exchange下可以有多个不同的queue，绑定就不同
            if (!dict_info_custom.ContainsKey(key))
            {
                GetQueueInfoForCustom(key, exchange, queue, routeKey);
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
                if (delaySend > 0)
                {
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", realExchange);
                    args.Add("x-dead-letter-routing-key", string.IsNullOrEmpty(routeKey) ? realQueue : routeKey);
                    channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: args);
                }
                else
                {
                    channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
                }
                channel.QueueBind(queue, exchange, string.IsNullOrEmpty(routeKey) ? queue : routeKey);
            }
        }

        public static void TopicEnsureQueue(IModel channel, Type messageType, string routeKey, out string exchange, out string queue, int delaySend = 0)
        {
            var type = messageType;
            if (delaySend > 0)
            {//走新加延迟队列
                var type_name = messageType.IsGenericType ? messageType.GenericTypeArguments[0].Name : messageType.Name;
                var key = routeKey;///为什么direct模式用Queue，而这里这里又用routekey
                                   ///因为topic模式唯一指定的就是routekey，其他的都不定
                exchange = string.Format("{0}{1}{2}", "topic.", type_name, ".exchange.delay");
                queue = type_name + ".input.delay";
                if (!dict_info_custom.ContainsKey(key))
                {
                    GetQueueInfoForCustom(key, exchange, queue, routeKey);
                    channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", string.Format("{0}{1}{2}", "topic.", type_name, ".exchange"));//死掉过后转发到的exchange
                    args.Add("x-dead-letter-routing-key", routeKey);//转发的exchange的路由键
                    channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: args);
                    channel.QueueBind(queue, exchange, routeKey, null);
                }
            }
            else
            {
                if (!dict_info.ContainsKey(type))
                {
                    var info = GetQueueInfo(type);
                    exchange = "topic." + info.Exchange;
                    queue = info.Queue;
                    channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
                    channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(queue, exchange, routeKey);
                }
                else
                {
                    var info = GetQueueInfo(type);
                    exchange = "topic." + info.Exchange;
                    queue = info.Queue;
                }
            }
        }
        public static void TopicEnsureQueue(IModel channel, string routeKey, ref string exchange, ref string queue, int delaySend = 0)
        {
            string realExchange = exchange;
            if (delaySend > 0)
            {
                exchange = exchange + ".delay";
                queue = queue + ".delay";
            }
            var key = routeKey;
            if (!dict_info_custom.ContainsKey(key))
            {
                GetQueueInfoForCustom(key, exchange, queue, routeKey);
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);

                if (delaySend > 0)
                {
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", realExchange);
                    args.Add("x-dead-letter-routing-key", routeKey);
                    channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, arguments: args);
                }
                else
                {
                    channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
                }
                channel.QueueBind(queue, exchange, routeKey);
            }
        }

        public static void FanoutEnsureQueue(IModel channel, Type messageType, out string exchange, int delaySend = 0)
        {
            var type = messageType;
            if (delaySend > 0)
            {
                var type_name = messageType.IsGenericType ? messageType.GenericTypeArguments[0].Name : messageType.Name;
                var key = string.Format("{0}{1}{2}", "fanout.", type_name, ".exchange.delay");
                exchange = key;///同direct模式
                var queue = type_name + ".input.delay";
                if (!dict_info_custom.ContainsKey(key))
                {
                    GetQueueInfoForCustom(key, exchange, queue, "");
                    channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", string.Format("{0}{1}{2}", "fanout.", type_name, ".exchange"));//死掉过后转发到的exchange
                    args.Add("x-dead-letter-routing-key", "");//转发的exchange的路由键
                    channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: args);
                    channel.QueueBind(queue, exchange, "", null);
                }
            }
            else
            {
                if (!dict_info.ContainsKey(type))
                {
                    var info = GetQueueInfo(messageType);
                    exchange = "fanout." + info.Exchange;
                    channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);
                }
                else
                {
                    var info = GetQueueInfo(messageType);
                    exchange = "fanout." + info.Exchange;
                }
            }
        }

        public static void FanoutEnsureQueue(IModel channel, ref string exchange, int delaySend = 0)
        {
            string realExchange = exchange;
            if (delaySend > 0)
            {
                exchange = exchange + ".delay";
            }
            var key = exchange;//routekey不可用,queue 几乎没用，exchange不唯一
            if (!dict_info_custom.ContainsKey(key))
            {
                GetQueueInfoForCustom(key, exchange, "", "");
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);

                if (delaySend > 0)
                {
                    var queue = "delayqueue";
                    var args = new Dictionary<string, object>();
                    args.Add("x-message-ttl", delaySend);
                    args.Add("x-dead-letter-exchange", realExchange);
                    args.Add("x-dead-letter-routing-key", "");
                    channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, arguments: args);
                    channel.QueueBind(queue, exchange, "");
                }
            }
        }

        private static QueueInfo GetQueueInfo(Type messageType)
        {
            var type_name = messageType.IsGenericType ? messageType.GenericTypeArguments[0].Name : messageType.Name;
            var info = dict_info.GetOrAdd(messageType, t => new QueueInfo
            {
                Exchange = type_name + ".exchange",
                RouteKey = type_name + ".input",
                Queue = type_name + ".input"
            });

            return info;
        }

        private static QueueInfo GetQueueInfoForCustom(string key, string exchangeName, string queueName, string routeName)
        {
            var info = dict_info_custom.GetOrAdd(key, t => new QueueInfo
            {
                Exchange = exchangeName,
                RouteKey = routeName,
                Queue = queueName
            });

            return info;
        }
    }
}
