using Force.Common.LightMessager.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.LightMessager.Helper
{
    public interface IRabbitMQProducer
    {
        bool Send(BaseMessage message, int delaySend = 0);
        bool Publish(BaseMessage message, string pattern, int delaySend = 0);
        bool FanoutPublish(BaseMessage message);
    }
}
