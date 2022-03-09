using LogicLayer.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var value = await _cache.GetStringAsync(key);

            if (value == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> SetAsync<T>(string key, T value) where T : class
        {
            await _cache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(value),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

            return value;
        }

        public async Task ClearCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
