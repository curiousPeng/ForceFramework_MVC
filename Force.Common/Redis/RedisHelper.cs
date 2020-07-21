using FastMember;
using RedisTools.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Force.Common.RedisTools
{
    public class RedisHelper : IRedisHelper
    {
        private IDatabase _db;
        private ConnectionMultiplexer _connector;
        private ISerializer _serializer;

        public IDatabase Database { get { return this._db; } }

        public RedisHelper(IRedisBase redisBase)
        {
            _connector = redisBase.GetConnection();
            _db = redisBase.GetDB(0);
            _serializer = new NewtonsoftSerializer();
        }

        public bool Exists(string key)
        {
            EnsureKey(key);

            return _db.KeyExists(key);
        }

        public Task<bool> ExistsAsync(string key)
        {
            EnsureKey(key);

            return _db.KeyExistsAsync(key);
        }

        public bool Remove(string key)
        {
            EnsureKey(key);

            return _db.KeyDelete(key);
        }

        public Task<bool> RemoveAsync(string key)
        {
            EnsureKey(key);

            return _db.KeyDeleteAsync(key);
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            EnsureNotNull(nameof(keys), keys);

            var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
            _db.KeyDelete(redisKeys);
        }

        public Task RemoveAllAsync(IEnumerable<string> keys)
        {
            EnsureNotNull(nameof(keys), keys);

            var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
            return _db.KeyDeleteAsync(redisKeys);
        }

        public T Get<T>(string key)
        {
            EnsureKey(key);

            var valueBytes = _db.StringGet(key);
            if (valueBytes.HasValue)
            {
                if (typeof(T).IsValueType)
                    return valueBytes.As<T>();
                else
                    return _serializer.Deserialize<T>(valueBytes);
            }

            return default(T);
        }

        public T Get<T>(string key, DateTimeOffset expiresAt)
        {
            EnsureKey(key);

            var valueBytes = _db.StringGet(key);
            if (valueBytes.HasValue)
            {
                _db.KeyExpire(key, expiresAt.UtcDateTime.Subtract(DateTime.UtcNow));
                if (typeof(T).IsValueType)
                    return valueBytes.As<T>();
                else
                    return _serializer.Deserialize<T>(valueBytes);
            }

            return default(T);
        }

        public T Get<T>(string key, TimeSpan expiresIn)
        {
            EnsureKey(key);

            var valueBytes = _db.StringGet(key);
            if (valueBytes.HasValue)
            {
                _db.KeyExpire(key, expiresIn);
                if (typeof(T).IsValueType)
                    return valueBytes.As<T>();
                else
                    return _serializer.Deserialize<T>(valueBytes);
            }

            return default(T);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            EnsureKey(key);

            var valueBytes = await _db.StringGetAsync(key).ConfigureAwait(false);
            if (valueBytes.HasValue)
            {
                if (typeof(T).IsValueType)
                    return valueBytes.As<T>();
                else
                    return _serializer.Deserialize<T>(valueBytes);
            }

            return default(T);
        }

        public async Task<T> GetAsync<T>(string key, DateTimeOffset expiresAt)
        {
            EnsureKey(key);

            var valueBytes = await _db.StringGetAsync(key).ConfigureAwait(false);
            if (valueBytes.HasValue)
            {
                await _db.KeyExpireAsync(key, expiresAt.UtcDateTime.Subtract(DateTime.UtcNow))
                    .ConfigureAwait(false);
                if (typeof(T).IsValueType)
                    return valueBytes.As<T>();
                else
                    return _serializer.Deserialize<T>(valueBytes);
            }

            return default(T);
        }

        public async Task<T> GetAsync<T>(string key, TimeSpan expiresIn)
        {
            EnsureKey(key);

            var valueBytes = await _db.StringGetAsync(key).ConfigureAwait(false);
            if (valueBytes.HasValue)
            {
                await _db.KeyExpireAsync(key, expiresIn).ConfigureAwait(false);
                if (typeof(T).IsValueType)
                    return valueBytes.As<T>();
                else
                    return _serializer.Deserialize<T>(valueBytes);
            }

            return default(T);
        }

        public bool Add<T>(string key, T value)
        {
            EnsureKey(key);

            // 说明：
            // 这里T有可能是基础类型，这在redisvalue中是有完善的转换支持的，但是：
            // 1. 顶层接口的样子变得很别扭
            // 2. 仍然会有额外的对象生成
            // 因此最后还是选择统一过一次Serialize
            // link: https://stackoverflow.com/questions/15958830/c-sharp-generics-cast-generic-type-to-value-type
            var entryBytes = _serializer.Serialize(value);

            return _db.StringSet(key, entryBytes);
        }

        public async Task<bool> AddAsync<T>(string key, T value)
        {
            EnsureKey(key);

            var entryBytes = _serializer.Serialize(value);

            return await _db.StringSetAsync(key, entryBytes).ConfigureAwait(false);
        }

        public bool Add<T>(string key, T value, DateTimeOffset expiresAt)
        {
            EnsureKey(key);

            var entryBytes = _serializer.Serialize(value);
            var expiration = expiresAt.UtcDateTime.Subtract(DateTime.UtcNow);

            return _db.StringSet(key, entryBytes, expiration);
        }

        public async Task<bool> AddAsync<T>(string key, T value, DateTimeOffset expiresAt)
        {
            EnsureKey(key);

            var entryBytes = _serializer.Serialize(value);
            var expiration = expiresAt.UtcDateTime.Subtract(DateTime.UtcNow);

            return await _db.StringSetAsync(key, entryBytes, expiration).ConfigureAwait(false);
        }

        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            EnsureKey(key);

            var entryBytes = _serializer.Serialize(value);

            return _db.StringSet(key, entryBytes, expiresIn);
        }

        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn)
        {
            EnsureKey(key);

            var entryBytes = _serializer.Serialize(value);

            return await _db.StringSetAsync(key, entryBytes, expiresIn).ConfigureAwait(false);
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            EnsureNotNull(nameof(keys), keys);

            var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
            var result = _db.StringGet(redisKeys);

            var dict = new Dictionary<string, T>(StringComparer.Ordinal);
            for (var index = 0; index < redisKeys.Length; index++)
            {
                var value = result[index];
                dict.Add(redisKeys[index], value == RedisValue.Null ? default(T) : _serializer.Deserialize<T>(value));
            }

            return dict;
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys, DateTimeOffset expiresAt)
        {
            var result = GetAll<T>(keys);
            UpdateExpiryAll(keys.ToArray(), expiresAt);

            return result;
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys, TimeSpan expiresIn)
        {
            var result = GetAll<T>(keys);
            UpdateExpiryAll(keys.ToArray(), expiresIn);

            return result;
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            EnsureNotNull(nameof(keys), keys);

            var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
            var result = await _db.StringGetAsync(redisKeys).ConfigureAwait(false);

            var dict = new Dictionary<string, T>(StringComparer.Ordinal);
            for (var index = 0; index < redisKeys.Length; index++)
            {
                var value = result[index];
                dict.Add(redisKeys[index], value == RedisValue.Null ? default(T) : _serializer.Deserialize<T>(value));
            }

            return dict;
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, DateTimeOffset expiresAt)
        {
            var result = await GetAllAsync<T>(keys).ConfigureAwait(false);
            await UpdateExpiryAllAsync(keys.ToArray(), expiresAt).ConfigureAwait(false);

            return result;
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, TimeSpan expiresIn)
        {
            var result = await GetAllAsync<T>(keys).ConfigureAwait(false);
            await UpdateExpiryAllAsync(keys.ToArray(), expiresIn).ConfigureAwait(false);

            return result;
        }

        public bool AddAll<T>(IDictionary<string, T> items)
        {
            EnsureNotNull(nameof(items), items);

            var values = items
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, _serializer.Serialize(p.Value)))
                .ToArray();

            return _db.StringSet(values);
        }

        public async Task<bool> AddAllAsync<T>(IDictionary<string, T> items)
        {
            EnsureNotNull(nameof(items), items);

            var values = items
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, _serializer.Serialize(p.Value)))
                .ToArray();

            return await _db.StringSetAsync(values).ConfigureAwait(false);
        }

        public bool AddAll<T>(IDictionary<string, T> items, DateTimeOffset expiresAt)
        {
            EnsureNotNull(nameof(items), items);

            var values = items
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, _serializer.Serialize(p.Value)))
                .ToArray();

            var result = _db.StringSet(values);
            foreach (var value in values)
                _db.KeyExpire(value.Key, expiresAt.UtcDateTime);

            return result;
        }

        public async Task<bool> AddAllAsync<T>(IDictionary<string, T> items, DateTimeOffset expiresAt)
        {
            EnsureNotNull(nameof(items), items);

            var values = items
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, _serializer.Serialize(p.Value)))
                .ToArray();

            var result = await _db.StringSetAsync(values).ConfigureAwait(false);
            Parallel.ForEach(values, async value =>
                await _db.KeyExpireAsync(value.Key, expiresAt.UtcDateTime)
                .ConfigureAwait(false));

            return result;
        }

        public bool AddAll<T>(IDictionary<string, T> items, TimeSpan expiresOn)
        {
            EnsureNotNull(nameof(items), items);

            var values = items
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, _serializer.Serialize(p.Value)))
                .ToArray();

            var result = _db.StringSet(values);
            foreach (var value in values)
                _db.KeyExpire(value.Key, expiresOn);

            return result;
        }

        public async Task<bool> AddAllAsync<T>(IDictionary<string, T> items, TimeSpan expiresOn)
        {
            EnsureNotNull(nameof(items), items);

            var values = items
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, _serializer.Serialize(p.Value)))
                .ToArray();

            var result = await _db.StringSetAsync(values).ConfigureAwait(false);
            Parallel.ForEach(values, async value =>
                await _db.KeyExpireAsync(value.Key, expiresOn).ConfigureAwait(false));

            return result;
        }

        #region hash
        public T HashFieldGet<T>(string hashKey, string field)
        {
            EnsureKey(hashKey);
            EnsureKey(field);

            var redisValue = _db.HashGet(hashKey, field);
            if (redisValue.HasValue)
            {
                if (typeof(T).IsValueType)
                    return redisValue.As<T>();
                else
                    return _serializer.Deserialize<T>(redisValue);
            }

            return default(T);
        }

        public bool HashFieldSet<T>(string hashKey, string field, T value)
        {
            EnsureKey(hashKey);
            EnsureKey(field);

            return _db.HashSet(hashKey, field, _serializer.Serialize(value), When.Always);
        }

        public async Task<T> HashFieldGetAsync<T>(string hashKey, string field)
        {
            EnsureKey(hashKey);
            EnsureKey(field);

            var redisValue = await _db.HashGetAsync(hashKey, field).ConfigureAwait(false);
            if (redisValue.HasValue)
            {
                if (typeof(T).IsValueType)
                    return redisValue.As<T>();
                else
                    return _serializer.Deserialize<T>(redisValue);
            }

            return default(T);
        }

        public Task<bool> HashFieldSetAsync<T>(string hashKey, string field, T value)
        {
            EnsureKey(hashKey);
            EnsureKey(field);

            return _db.HashSetAsync(hashKey, field, _serializer.Serialize(value), When.Always);
        }

        public T HashObjGet<T>(string hashKey, T model)
            where T : class
        {
            EnsureKey(hashKey);
            EnsureNotNull(nameof(model), model);

            var hashValues = _db.HashGetAll(hashKey);
            var obj = Activator.CreateInstance<T>();
            var acc = TypeAccessor.Create(typeof(T));
            foreach (var val in hashValues)
                acc[obj, val.Name] = val.Value;

            return obj;
        }

        public bool HashObjSet<T>(string hashKey, T model)
            where T : class
        {
            EnsureKey(hashKey);
            EnsureNotNull(nameof(model), model);

            var acc = ObjectAccessor.Create(model);
            var members = acc.TypeAccessor.GetMembers();
            var entrys = members.Select(p => new HashEntry(p.Name, _serializer.Serialize(acc[p.Name]))).ToArray();
            _db.HashSet(hashKey, entrys);

            return true;
        }

        public async Task<T> HashObjGetAsync<T>(string hashKey, T model)
            where T : class
        {
            EnsureKey(hashKey);
            EnsureNotNull(nameof(model), model);

            var hashValues = await _db.HashGetAllAsync(hashKey).ConfigureAwait(false);
            var obj = Activator.CreateInstance<T>();
            var acc = TypeAccessor.Create(typeof(T));
            foreach (var val in hashValues)
                acc[obj, val.Name] = val.Value;

            return obj;
        }

        public Task HashObjSetAsync<T>(string hashKey, T model)
            where T : class
        {
            EnsureKey(hashKey);
            EnsureNotNull(nameof(model), model);

            var acc = ObjectAccessor.Create(model);
            var members = acc.TypeAccessor.GetMembers();
            var entrys = members.Select(p => new HashEntry(p.Name, _serializer.Serialize(acc[p.Name]))).ToArray();
            return _db.HashSetAsync(hashKey, entrys);
        }
        #endregion

        #region pub/sub
        public long Publish<T>(string channel, T message)
        {
            var sub = _connector.GetSubscriber();
            return sub.Publish(channel, _serializer.Serialize(message));
        }

        public async Task<long> PublishAsync<T>(string channel, T message)
        {
            var sub = _connector.GetSubscriber();
            return await sub.PublishAsync(channel, _serializer.Serialize(message))
                .ConfigureAwait(false);
        }

        public void Subscribe<T>(string channel, Action<T> handler)
        {
            EnsureNotNull(nameof(handler), handler);

            var sub = _connector.GetSubscriber();
            sub.Subscribe(channel, (redisChannel, value) => handler(_serializer.Deserialize<T>(value)));
        }

        public async Task SubscribeAsync<T>(string channel, Func<T, Task> handler)
        {
            EnsureNotNull(nameof(handler), handler);

            var sub = _connector.GetSubscriber();
            await sub.SubscribeAsync(channel, async (redisChannel, value) =>
                await handler(_serializer.Deserialize<T>(value)).ConfigureAwait(false));
        }

        public void Unsubscribe<T>(string channel, Action<T> handler)
        {
            EnsureNotNull(nameof(handler), handler);

            var sub = _connector.GetSubscriber();
            sub.Unsubscribe(channel, (redisChannel, value) => handler(_serializer.Deserialize<T>(value)));
        }

        public async Task UnsubscribeAsync<T>(string channel, Func<T, Task> handler)
        {
            EnsureNotNull(nameof(handler), handler);

            var sub = _connector.GetSubscriber();
            await sub.UnsubscribeAsync(channel, (redisChannel, value) => handler(_serializer.Deserialize<T>(value)))
                .ConfigureAwait(false);
        }

        public void UnsubscribeAll()
        {
            var sub = _connector.GetSubscriber();
            sub.UnsubscribeAll();
        }

        public async Task UnsubscribeAllAsync()
        {
            var sub = _connector.GetSubscriber();
            await sub.UnsubscribeAllAsync().ConfigureAwait(false);
        }
        #endregion

        public bool UpdateExpiry(string key, DateTimeOffset expiresAt)
        {
            EnsureKey(key);

            return _db.KeyExpire(key, expiresAt.UtcDateTime.Subtract(DateTime.UtcNow));
        }

        public bool UpdateExpiry(string key, TimeSpan expiresIn)
        {
            EnsureKey(key);

            return _db.KeyExpire(key, expiresIn);
        }

        public async Task<bool> UpdateExpiryAsync(string key, DateTimeOffset expiresAt)
        {
            EnsureKey(key);

            return await _db.KeyExpireAsync(key, expiresAt.UtcDateTime.Subtract(DateTime.UtcNow))
                .ConfigureAwait(false);
        }

        public async Task<bool> UpdateExpiryAsync(string key, TimeSpan expiresIn)
        {
            EnsureKey(key);

            return await _db.KeyExpireAsync(key, expiresIn).ConfigureAwait(false);
        }

        public IDictionary<string, bool> UpdateExpiryAll(string[] keys, DateTimeOffset expiresAt)
        {
            EnsureNotNull(nameof(keys), keys);

            var results = new Dictionary<string, bool>(StringComparer.Ordinal);

            for (var i = 0; i < keys.Length; i++)
                results.Add(keys[i], _db.KeyExpire(keys[i], expiresAt.UtcDateTime.Subtract(DateTime.UtcNow)));

            return results;
        }

        public IDictionary<string, bool> UpdateExpiryAll(string[] keys, TimeSpan expiresIn)
        {
            EnsureNotNull(nameof(keys), keys);

            var results = new Dictionary<string, bool>(StringComparer.Ordinal);

            for (var i = 0; i < keys.Length; i++)
                results.Add(keys[i], _db.KeyExpire(keys[i], expiresIn));

            return results;
        }

        public async Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, DateTimeOffset expiresAt)
        {
            EnsureNotNull(nameof(keys), keys);

            var results = new Dictionary<string, bool>(StringComparer.Ordinal);

            for (var i = 0; i < keys.Length; i++)
                results.Add(keys[i],
                    await _db.KeyExpireAsync(keys[i], expiresAt.UtcDateTime.Subtract(DateTime.UtcNow))
                    .ConfigureAwait(false));

            return results;
        }

        public async Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, TimeSpan expiresIn)
        {
            EnsureNotNull(nameof(keys), keys);

            var results = new Dictionary<string, bool>(StringComparer.Ordinal);

            for (var i = 0; i < keys.Length; i++)
                results.Add(keys[i], await _db.KeyExpireAsync(keys[i], expiresIn).ConfigureAwait(false));

            return results;
        }

        private void EnsureKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("缓存键不能为空");
        }

        private void EnsureNotNull<T>(string name, IEnumerable<T> values)
        {
            if (values == null || !values.Any())
                throw new InvalidOperationException("提供的集合为空或未包含项");
        }

        private void EnsureNotNull(string name, object value)
        {
            if (value == null)
                throw new ArgumentNullException($"{name}不允许为空");
        }
    }
}
