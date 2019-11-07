namespace Force.Common.LightMessager.DAL
{
    /*
     * 说明：
     * 在消息状态的变迁上非常纠结，主要是因为消息状态的记录是落地到sql server中的
     * 如果每一次状态改变都需要同步更新的话则整个msg bus吞吐率会大大降低
     * 所以折中采用了一些其他手段：
     *  1. 状态目前分了5档，但其实只要成功到达第三档也就是ArrivedBroker，那么该条消息就可以从本地删除了。
     *  因为队列目前是durable的消息也是persistent的，如果被consumer消费掉那么会自动消失，反之无脑redeliver即可
     *  2. RegisterHandler方法上增加了一个名为redeliveryCheck的参数，意为如果你知道自己的消息处理逻辑实现为了
     *  幂等，那么就可以不用跟踪消息状态了
     */
    public sealed class MsgStatus
    {
        /// <summary>
        /// Created: 1
        /// </summary>
        public static readonly short Created = 1;
        /// <summary>
        /// Retrying: 2
        /// </summary>
        public static readonly short Retrying = 2;
        /// <summary>
        /// ArrivedBroker: 3
        /// </summary>
        public static readonly short ArrivedBroker = 3;
        /// <summary>
        /// Exception: 4
        /// </summary>
        public static readonly short Exception = 4;
        /// <summary>
        /// Processed: 5
        /// </summary>
        public static readonly short Consumed = 5;
    }
}
