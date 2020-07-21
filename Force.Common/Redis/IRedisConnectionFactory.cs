using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.RedisTools
{
    public interface IRedisConnectionFactory
    {
        IConnectionMultiplexer Get();
    }
}
