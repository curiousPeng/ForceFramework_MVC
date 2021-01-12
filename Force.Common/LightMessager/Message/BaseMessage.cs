using Newtonsoft.Json;
using System;

namespace Force.Common.LightMessager.Message
{
    public class BaseMessage
    {
        internal long MsgHash { set; get; }

        [JsonIgnore]
        public string Source { set; get; }

        [JsonIgnore]
        internal bool NeedNAck { set; get; }

        /// <summary>
        /// publisher -> broker
        /// </summary>
        internal int RetryCount_Publish { set; get; }

        /// <summary>
        /// broker -> consumer
        /// </summary>
        internal int RetryCount_Deliver { set; get; }

        internal DateTime LastRetryTime { set; get; }

        /// <summary>
        /// 重试的时候会用到,topic消息必填，因为加了JsonIgnore,consumer的时候必须自己定义
        /// </summary>
        [JsonIgnore]
        public string routeKey { set; get; }
        /// <summary>
        /// 重试的时候会用到
        /// </summary>
        [JsonIgnore]
        public string exchangeName { set; get; }
        /// <summary>
        /// 重试的时候会用到
        /// </summary>
        [JsonIgnore]
        public string queueName { set; get; }

        public DateTime CreatedTime { set; get; }
    }
}
