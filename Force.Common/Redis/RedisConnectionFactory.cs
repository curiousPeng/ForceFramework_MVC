using StackExchange.Redis;
using System;

namespace Force.Common.RedisTools
{
    public class RedisConnectionFactory : IRedisConnectionFactory, IDisposable
    {
        private readonly Lazy<IConnectionMultiplexer> _connection;

        public RedisConnectionFactory(string connectionString)
        {
            _connection = new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }

        public IConnectionMultiplexer Get()
        {
            return _connection.Value;
        }

        public void Dispose()
        {
            _connection.Value.Dispose();
        }
    }
}
