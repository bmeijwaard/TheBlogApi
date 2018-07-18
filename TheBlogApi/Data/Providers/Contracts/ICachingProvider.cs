using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TheBlogApi.Data.Providers.Contracts
{
    public interface ICachingProvider
    {
        IDatabase Database { get; }

        Task DeleteAsync(string key);
        Task DeleteCollectionAsync<T>();
        Task DeleteSingleAsync<T>(Guid id);
        Task FlushAllAsync();
        Task<T> GetAsync<T>(string key = null);
        Task<IEnumerable<T>> GetCollectionAsync<T>();
        Task<HashSet<T>> GetHashSetAsync<T>();
        Task<T> GetSingleOrDefaultAsync<T>(Guid id);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task SetCollectionAsync<T>(IEnumerable<T> obj, TimeSpan? expiry = null);
        Task SetHashSetAsync<T>(HashSet<T> obj, TimeSpan? expiry = null);
        Task SetSingleAsync<T>(Guid id, T obj, TimeSpan? expiry = null);
    }
}