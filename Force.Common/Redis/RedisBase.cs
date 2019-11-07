using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Force.Common.RedisTool.Base
{
    public class RedisBase : IRedisBase
    {
        private ConnectionMultiplexer _multiplexer;

        private IDatabase redisDB;
        private RedisValue token;

        public RedisBase(string connString, int db = 0)
        {
            /// https://stackoverflow.com/questions/30895507/it-was-not-possible-to-connect-to-the-redis-servers-to-create-a-disconnected
            _multiplexer = ConnectionMultiplexer.Connect(connString);
            redisDB = _multiplexer.GetDatabase(db);

            token = Environment.MachineName;
        }

        public IDatabase DB()
        {
            return redisDB;
        }

        public T DoWithLock<T>(string lockName, Func<T> func, bool retry = false)
        {
            if (retry)
            {
                var ret = RetryHelper.Retry(
                () =>
                {
                    if (redisDB.LockTake(lockName, token, TimeSpan.FromMinutes(30)))
                    {
                        try
                        {
                            // you have the lock do work
                            return func();
                        }
                        finally
                        {
                            redisDB.LockRelease(lockName, token);
                        }
                    }
                    else
                    {
                        // RetryHelper.Retry里面的重试是针对网络异常情况的重试，跟此处获取锁失败的重试是两回事，请注意区分
                        // 然而考虑到代码的简单，所以此处仍然还是借用了retryhelper的机制，直接抛出异常
                        throw new Exception<GetLockExceptionArgs>("redisDB.LockTake@" + lockName + "获取锁失败，重试超过最大次数！");
                    }
                },
                timeOut: 2000,  // 每次重试间隔2000毫秒
                retries: 5); // 最大重试5次

                return ret;
            }
            else
            {
                if (redisDB.LockTake(lockName, token, TimeSpan.FromMinutes(30)))
                {
                    try
                    {
                        // you have the lock do work
                        return func();
                    }
                    finally
                    {
                        redisDB.LockRelease(lockName, token);
                    }
                }
                else
                {
                    throw new Exception<GetLockExceptionArgs>("redisDB.LockTake@" + lockName + "获取锁失败，重试超过最大次数！");
                }
            }
        }

        public void DoWithLock(string lockName, Action action, bool retry = false)
        {
            if (retry)
            {
                RetryHelper.Retry(
                () =>
                {
                    if (redisDB.LockTake(lockName, token, TimeSpan.FromMinutes(30)))
                    {
                        try
                        {
                            // you have the lock do work
                            action();
                        }
                        finally
                        {
                            redisDB.LockRelease(lockName, token);
                        }
                    }
                    else
                    {
                        throw new Exception<GetLockExceptionArgs>("redisDB.LockTake@" + lockName + "获取锁失败，重试超过最大次数！");
                    }
                },
                timeOut: 2000,  // 每次重试间隔2000毫秒
                retries: 5); // 最大重试5次
            }
            else
            {
                if (redisDB.LockTake(lockName, token, TimeSpan.FromMinutes(30)))
                {
                    try
                    {
                        // you have the lock do work
                        action();
                    }
                    finally
                    {
                        redisDB.LockRelease(lockName, token);
                    }
                }
                else
                {
                    throw new Exception<GetLockExceptionArgs>("redisDB.LockTake@" + lockName + "获取锁失败，重试超过最大次数！");
                }
            }
        }

        public string GetKeyStr(string prefix, string str, bool isGlobal = false)
        {
            if (isGlobal)
            {
                return string.Format("global:{0}:{1}", prefix, str);
            }
            else
            {
                return string.Format("{0}:{1}", prefix, str);
            }
        }

        public string GetKeyStr(string prefix, string str1, string str2, bool isGlobal = false)
        {
            if (isGlobal)
            {
                return string.Format("global:{0}:{1}:{2}", prefix, str1, str2);
            }
            else
            {
                return string.Format("{0}:{1}:{2}", prefix, str1, str2);
            }
        }

        public string GetKeyStr(string prefix, string str1, string str2, string str3, bool isGlobal = false)
        {
            if (isGlobal)
            {
                return string.Format("global:{0}:{1}:{2}:{3}", prefix, str1, str2, str3);
            }
            else
            {
                return string.Format("{0}:{1}:{2}:{3}", prefix, str1, str2, str3);
            }
        }

        public string GetKeyStr(string prefix, string str1, string str2, string str3, string str4, bool isGlobal = false)
        {
            if (isGlobal)
            {
                return string.Format("global:{0}:{1}:{2}:{3}:{4}", prefix, str1, str2, str3, str4);
            }
            else
            {
                return string.Format("{0}:{1}:{2}:{3}:{4}", prefix, str1, str2, str3, str4);
            }
        }


    }
}
