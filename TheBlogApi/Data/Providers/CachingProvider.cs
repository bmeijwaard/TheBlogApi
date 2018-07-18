using Microsoft.Extensions.Options;
using ProtoBuf;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Data.Providers
{
    public class CachingProvider : ICachingProvider
    {
        private readonly CacheSettings _settings;
        private ConnectionMultiplexer _connection;
        private static readonly object _lock = new object();
        private readonly IExceptionHandler _exceptionHandler;

        public CachingProvider(IOptions<CacheSettings> settingsOptions, IExceptionHandler exceptionHandler)
        {
            _settings = settingsOptions.Value;
            _exceptionHandler = exceptionHandler;
        }

        private ConnectionMultiplexer Connection
        {
            get
            {
                if (string.IsNullOrEmpty(_settings.ConnectionString))
                {
                    throw new ArgumentException("Not initialized, Redis connectionstring = null");
                }

                // this is to prevent cache storms
                if (_connection == null || !_connection.IsConnected)
                {
                    lock (_lock)
                    {
                        if (_connection == null || !_connection.IsConnected)
                        {
                            //kill old connection
                            if (_connection != null)
                            {
                                _connection.Close(false);
                                _connection.Dispose();
                                _connection = null;
                            }

                            _connection = ConnectionMultiplexer.Connect(_settings.ConnectionString);
                        }
                    }
                }

                return _connection;
            }
        }

        public IDatabase Database => Connection.GetDatabase();

        public async Task FlushAllAsync()
        {
            using (var adminConnection = await ConnectionMultiplexer.ConnectAsync($"{_settings.ConnectionString},allowAdmin=true"))
            {
                foreach (var endpoint in adminConnection.GetEndPoints())
                {
                    var server = adminConnection.GetServer(endpoint);
                    if (!server.IsSlave)
                    {
                        await server.FlushDatabaseAsync();
                    }
                }
            }
        }

        public async Task<T> GetSingleOrDefaultAsync<T>(Guid id)
        {
            return await GetAsync<T>(KeyExtensions.GetKey<T>(id));
        }

        public async Task<IEnumerable<T>> GetCollectionAsync<T>()
        {
            return await GetAsync<IEnumerable<T>>(KeyExtensions.GetEnumerableKey<T>());
        }

        public async Task<HashSet<T>> GetHashSetAsync<T>()
        {
            return await GetAsync<HashSet<T>>(KeyExtensions.GetEnumerableKey<T>());
        }

        public async Task<T> GetAsync<T>(string key = null)
        {
            if (!_settings.UseCache)
            {
                return default(T);
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            try
            {
                T result = Deserialize<T>(await Database.StringGetAsync(key));

                return result;
            }
            catch (Exception ex)
            {
                await _exceptionHandler.ProcessExceptionAsync(ex);
                return default(T);
            }
        }

        public async Task SetSingleAsync<T>(Guid id, T obj, TimeSpan? expiry = null)
        {
            await SetAsync(KeyExtensions.GetKey<T>(id), obj, expiry);
        }

        public async Task SetCollectionAsync<T>(IEnumerable<T> obj, TimeSpan? expiry = null)
        {
            await SetAsync(KeyExtensions.GetEnumerableKey<T>(), obj, expiry);
        }

        public async Task SetHashSetAsync<T>(HashSet<T> obj, TimeSpan? expiry = null)
        {
            await SetAsync(KeyExtensions.GetEnumerableKey<T>(), obj, expiry);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (!_settings.UseCache)
            {
                return;
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            if (expiry == null)
            {
                expiry = TimeSpan.Parse(_settings.DefaultExpiry);
            }

            try
            {
                var sValue = Serialize(value);
                await Database.StringSetAsync(key, sValue, expiry);
            }
            catch (Exception ex)
            {
                await DeleteAsync(key);
                await _exceptionHandler.ProcessExceptionAsync(ex);
            }
        }

        public async Task DeleteSingleAsync<T>(Guid id)
        {
            await DeleteAsync(KeyExtensions.GetKey<T>(id));
        }

        public async Task DeleteCollectionAsync<T>()
        {
            await DeleteAsync(KeyExtensions.GetEnumerableKey<T>());
        }

        public async Task DeleteAsync(string key)
        {
            if (!_settings.UseCache)
            {
                return;
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            try
            {
                await Database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                await _exceptionHandler.ProcessExceptionAsync(ex);
            }
        }

        private byte[] Serialize<T>(T input)
        {
            if (input == null)
            {
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, input);
                byte[] output = memoryStream.ToArray();
                return output;
            }
        }

        private T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                object result = Serializer.Deserialize<T>(memoryStream);
                return (T)result;
            }
        }
    }
}
