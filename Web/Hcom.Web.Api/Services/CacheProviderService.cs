using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;

namespace Hcom.Web.Api.Services
{
    /// <summary>
    /// Uses Microsoft Distributed Caching
    /// </summary>
    public class CacheProviderService : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;

        public CacheProviderService(IDistributedCache distributedCache)
        {
            if (distributedCache == null)
            {
                throw new ArgumentNullException("distributedCache", "IDistributedCache was not Provided. Please add IDistributedCache to IoC container.");
            }

            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Adds object to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(string key, T value)
        {
            //var _byteValue = ObjectToByteArray(value);  
            using (var _ms = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(_ms, value);
                await _distributedCache.SetAsync(key, _ms.ToArray());
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var _btyeValue = await _distributedCache.GetAsync(key);
            if (_btyeValue == null)
                return default;

            //var _readOnlySpan = new ReadOnlySpan<byte>(_byteValue);
            var _value = JsonSerializer.Deserialize<T>(_btyeValue);
            return _value;
        }

        public async Task Remove(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetWithSlidingExpirationAsync<T>(string key, T value, int expiresInSecs)
        {
            using (var _ms = new MemoryStream())
            {
                var _opts = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(expiresInSecs));

                await JsonSerializer.SerializeAsync(_ms, value);
                await _distributedCache.SetAsync(key, _ms.ToArray(), _opts);
            }
        }

        public async Task SetWithAbsoluteExpirationAsync<T>(string key, T value, int expiresInSecs)
        {
            using (var _ms = new MemoryStream())
            {
                var _opts = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(expiresInSecs));

                await JsonSerializer.SerializeAsync(_ms, value);
                await _distributedCache.SetAsync(key, _ms.ToArray(), _opts);
            }
        }

        public async Task SetWithAbsoluteExpirationRelativeToNowAsync<T>(string key, T value, int expiresInSecs)
        {
            using (var _ms = new MemoryStream())
            {
                var _opts = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(expiresInSecs));

                await JsonSerializer.SerializeAsync(_ms, value);
                await _distributedCache.SetAsync(key, _ms.ToArray(), _opts);
            }
        }
    }
}
