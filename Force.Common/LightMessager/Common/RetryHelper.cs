using System;
using System.Threading.Tasks;

namespace Force.Common.LightMessager.Common
{
    internal static class RetryHelper
    {
        private const string ErrorMessage = "Maximum number of tries exceeded to perform the action.";
        private const string WarningMessage = "Exception occurred performing an action. Retrying... {0}/{1}";

        public static void Retry(Action action, int maxRetry, int backoffMs = 100,
                Predicate<Exception> shouldRetry = null)
        {
            Retry(() =>
            {
                action();
                return true;
            },
            maxRetry, backoffMs, shouldRetry, null);
        }

        public static Task RetryAsync(Action action, int maxRetry, int backoffMs = 100,
                Predicate<Exception> shouldRetry = null)
        {
            return RetryAsync(() =>
            {
                Task.Run(action);
                return Task.FromResult(true);
            },
            maxRetry, backoffMs, shouldRetry, null);
        }

        public static T Retry<T>(Func<T> func, int maxRetry, int backoffMs = 100,
                Predicate<Exception> shouldRetry = null, Predicate<T> retryOnResult = null)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    var result = func();
                    if (retryCount != maxRetry && retryOnResult?.Invoke(result) == true)
                        throw new Exception(string.Format(WarningMessage, retryCount, maxRetry));

                    return result;
                }
                catch (Exception ex)
                {
                    if (retryCount == maxRetry || shouldRetry?.Invoke(ex) == false)
                        throw new Exception(ErrorMessage, ex);

                    // 间隔一段时间+随机扰乱
                    var jitter = RandomUtil.Random.Next(0, 100);
                    var backoff = (int)Math.Pow(2, retryCount) * backoffMs;
                    Task.Delay(backoff + jitter).Wait();

                    retryCount++;
                    backoffMs *= 2;
                }
            }
        }

        public static async Task<T> RetryAsync<T>(Func<Task<T>> func, int maxRetry, int backoffMs = 100,
                Predicate<Exception> shouldRetry = null, Predicate<T> retryOnResult = null)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    var result = await func();
                    if (retryCount != maxRetry && retryOnResult?.Invoke(result) == true)
                        throw new Exception(string.Format(WarningMessage, retryCount, maxRetry));

                    return result;
                }
                catch (Exception ex)
                {
                    if (retryCount == maxRetry || shouldRetry?.Invoke(ex) == false)
                        throw new Exception(ErrorMessage, ex);

                    // 间隔一段时间+随机扰乱
                    var jitter = RandomUtil.Random.Next(0, 100);
                    var backoff = (int)Math.Pow(2, retryCount) * backoffMs;
                    await Task.Delay(backoff + jitter);

                    retryCount++;
                    backoffMs *= 2;
                }
            }
        }
    }
}