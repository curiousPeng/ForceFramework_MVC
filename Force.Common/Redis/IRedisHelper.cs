using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Force.Common.RedisTools
{
    public interface IRedisHelper
    {
        bool Exists(string key);
        Task<bool> ExistsAsync(string key);
        bool Remove(string key);
        Task<bool> RemoveAsync(string key);
        void RemoveAll(IEnumerable<string> keys);
        Task RemoveAllAsync(IEnumerable<string> keys);
        T Get<T>(string key);
        T Get<T>(string key, DateTimeOffset expiresAt);
        T Get<T>(string key, TimeSpan expiresIn);
        Task<T> GetAsync<T>(string key);
        Task<T> GetAsync<T>(string key, DateTimeOffset expiresAt);
        Task<T> GetAsync<T>(string key, TimeSpan expiresIn);
        bool Add<T>(string key, T value);
        Task<bool> AddAsync<T>(string key, T value);
        bool Add<T>(string key, T value, DateTimeOffset expiresAt);
        Task<bool> AddAsync<T>(string key, T value, DateTimeOffset expiresAt);
        bool Add<T>(string key, T value, TimeSpan expiresIn);
        Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn);
        IDictionary<string, T> GetAll<T>(IEnumerable<string> keys);
        IDictionary<string, T> GetAll<T>(IEnumerable<string> keys, DateTimeOffset expiresAt);
        IDictionary<string, T> GetAll<T>(IEnumerable<string> keys, TimeSpan expiresIn);
        Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys);
        Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, DateTimeOffset expiresAt);
        Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, TimeSpan expiresIn);
        bool AddAll<T>(IDictionary<string, T> items);
        Task<bool> AddAllAsync<T>(IDictionary<string, T> items);
        bool AddAll<T>(IDictionary<string, T> items, DateTimeOffset expiresAt);
        Task<bool> AddAllAsync<T>(IDictionary<string, T> items, DateTimeOffset expiresAt);
        bool AddAll<T>(IDictionary<string, T> items, TimeSpan expiresOn);
        Task<bool> AddAllAsync<T>(IDictionary<string, T> items, TimeSpan expiresOn);
        T HashFieldGet<T>(string hashKey, string field);
        bool HashFieldSet<T>(string hashKey, string field, T value);
        T HashObjGet<T>(string hashKey, T model) where T : class;
        bool HashObjSet<T>(string hashKey, T model) where T : class;
        long Publish<T>(string channel, T message);
        Task<long> PublishAsync<T>(string channel, T message);
        void Subscribe<T>(string channel, Action<T> handler);
        Task SubscribeAsync<T>(string channel, Func<T, Task> handler);
        void Unsubscribe<T>(string channel, Action<T> handler);
        Task UnsubscribeAsync<T>(string channel, Func<T, Task> handler);
        void UnsubscribeAll();
        Task UnsubscribeAllAsync();
        bool UpdateExpiry(string key, DateTimeOffset expiresAt);
        bool UpdateExpiry(string key, TimeSpan expiresIn);
        Task<bool> UpdateExpiryAsync(string key, DateTimeOffset expiresAt);
        Task<bool> UpdateExpiryAsync(string key, TimeSpan expiresIn);
        IDictionary<string, bool> UpdateExpiryAll(string[] keys, DateTimeOffset expiresAt);
        IDictionary<string, bool> UpdateExpiryAll(string[] keys, TimeSpan expiresIn);
        Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, DateTimeOffset expiresAt);
        Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, TimeSpan expiresIn);

    }
}
