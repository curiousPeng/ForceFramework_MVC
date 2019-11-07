using Force.Common.LightMessager.Exceptions;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Force.Common.LightMessager.Message
{
    public class BaseHandleMessages<TMessage> : IHandleMessages<TMessage> 
        where TMessage : BaseMessage
    {
        private static Logger _logger = LogManager.GetLogger("MessageHandler");
        private static readonly int retry_wait = 1000; // 1秒
        private static readonly ConcurrentDictionary<long, int> retry_list = new ConcurrentDictionary<long, int>();

        public async Task Handle(TMessage message)
        {
            var sleep = 0;
            try
            {
                await DoHandle(message);
            }
            catch (Exception<LightMessagerExceptionArgs> ex)
            {
                // 如果异常设定了不能被吞掉，则进行延迟重试
                if (!ex.Args.CanBeSwallow)
                {
                    var current_value = retry_list.AddOrUpdate(message.MsgHash, 1, (key, oldValue) => oldValue * 2);
                    if (current_value > 4) // 1, 2, 4 最大允许重试3次
                    {
                        _logger.Debug("重试超过最大次数(3)，异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
                        retry_list.TryRemove(message.MsgHash, out _);
                    }
                    else
                    {
                        sleep = retry_wait * current_value;
                    }
                }
                else
                {
                    _logger.Debug("CanBeSwallowed=true，异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.Debug("未知异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
            }

            // 简单优化，将catch中的耗时逻辑移出来
            if (sleep > 0)
            {
                Thread.Sleep(sleep);
                message.NeedNAck = true;
            }
        }

        protected virtual Task DoHandle(TMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
