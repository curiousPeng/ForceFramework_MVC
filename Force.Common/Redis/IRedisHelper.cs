using StackExchange.Redis;

namespace Force.Common.RedisTool.Helper
{
    public interface IRedisHelper
    {
        IDatabase DB();
        T HashGet<T>(string key, bool isLock = false) where T : class;
        void HashSet<T>(string key, T model, bool isLock = false) where T : class;
    }
}
