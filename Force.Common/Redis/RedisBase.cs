using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.RedisTools
{
    public class RedisBase : IRedisBase
    {
        private ConnectionMultiplexer _multiplexer;

        /// <summary>
        /// 其他链接的可复用，注册一个单例就行
        /// </summary>
        /// <param name="connString"></param>
        public RedisBase(string connString)
        {
            _multiplexer = ConnectionMultiplexer.Connect(connString);
        }

        public ConnectionMultiplexer GetConnection()
        {
            return _multiplexer;
        }

        public IDatabase GetDB(int index)
        {
            return _multiplexer.GetDatabase(index);
        }
    }
}
