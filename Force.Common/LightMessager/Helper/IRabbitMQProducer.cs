using Force.Common.LightMessager.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.LightMessager.Helper
{
    public interface IRabbitMQProducer
    {
        bool DirectSend(BaseMessage message, int delaySend = 0);
        bool TopicSend(BaseMessage message, int delaySend = 0);
        bool FanoutSend(BaseMessage message, int delaySend = 0);
    }
}