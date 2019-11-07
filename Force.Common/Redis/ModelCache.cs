using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Force.Common.RedisTool.Cache
{
    public class ModelCache
    {
        private readonly static ConcurrentDictionary<Type, Dictionary<string, MemberInfo>> _cache = new ConcurrentDictionary<Type, Dictionary<string, MemberInfo>>();

        private ModelCache()
        {

        }

        public static void Add(Type key, Dictionary<string, MemberInfo> value)
        {

            _cache.TryAdd(key, value);

        }

        public static void Remove(Type key)
        {
            _cache.TryRemove(key, out _);
        }

        public static void RemoveAll()
        {
            _cache.Clear();
        }

        public static Dictionary<string, MemberInfo> Get(Type key)
        {
            Dictionary<string, MemberInfo> result;
            _cache.TryGetValue(key, out result);
            return result;
        }
    }
}
