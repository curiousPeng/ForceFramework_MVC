using System;
using System.Threading.Tasks;

namespace Force.Common.LightMessager.Helper
{
    internal static class RetryHelper
    {
        private const string ErrorMessage = "Maximum number of tries exceeded to perform the action: {0}.";
        private const string WarningMessage = "Exception occurred performing an action. Retrying... {0}/{1}";

        public static T Retry<T>(Func<T> retryme, int timeOut, int retries)
        {
            var tries = 0;
            do
            {
                tries++;
                try
                {
                    return retryme();
                }
                catch
                {
                    if (tries >= retries)
                    {
                        //logger.Error(ErrorMessage, ex);
                        throw;
                    }

                    //logger.Warn(WarningMessage, ex);
                    Task.Delay(timeOut).Wait();
                }
            }
            while (tries < retries);

            return default(T);
        }

        public static void Retry(Action retryme, int timeOut, int retries)
        {
            Retry(
                () =>
                {
                    retryme();
                    return true;
                },
                timeOut,
                retries);
        }
    }
}