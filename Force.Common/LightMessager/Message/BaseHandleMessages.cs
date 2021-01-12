using Force.Common.LightMessager.Common;
using Force.Common.LightMessager.Exceptions;
using Newtonsoft.Json;
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
        private int _maxRetry;
        private int _maxRequeue;
        private int _backoffMs;
        private static Logger _logger = LogManager.GetLogger("MessageHandler");
        protected BaseHandleMessages()
        {
            _maxRetry = 2;
            _maxRequeue = 2;
            _backoffMs = 200;
        }
        public async Task Handle(TMessage message)
        {
            try
            {
                // 执行DoHandle可能会发生异常，如果是我们特定的异常则进行重试操作
                // 否则直接抛出异常
                await RetryHelper.RetryAsync(() => DoHandle(message), _maxRetry, _backoffMs, p =>
                {
                    var ex = p as Exception<LightMessagerExceptionArgs>;
                    if (ex != null)
                        return true;

                    return false;
                });
            }
            catch (Exception ex)
            {
                _logger.Error("handler执行未知异常,数据：" + JsonConvert.SerializeObject(message));
                _logger.Error("handler执行未知异常：" + ex.Message + "；堆栈：" + ex.StackTrace);
            }
        }

        protected virtual Task DoHandle(TMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
