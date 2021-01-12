using Force.Common.LightMessager.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.LightMessager.Helper
{
    public interface IRabbitMQConsumer
    {
        void RegisterDirectHandler<TMessage, THandler>(string exchangeName, string queueName, string routeKey) where THandler : BaseHandleMessages<TMessage> where TMessage : BaseMessage;
        void RegisterTopicHandler<TMessage, THandler>(string routeKey, string exchangeName, string queueName) where THandler : BaseHandleMessages<TMessage> where TMessage : BaseMessage;
        void RegisterFanoutHandler<TMessage, THandler>(string exchangeName) where THandler : BaseHandleMessages<TMessage> where TMessage : BaseMessage;
    }
}