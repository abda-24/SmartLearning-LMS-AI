using ServicesAbstractions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class CacheService(IConnectionMultiplexer _multiplexer) : ICacheService
    {
        public async Task<T?> GetCachedDataAsync<T>(string cacheKey)
        {
            var db= _multiplexer.GetDatabase();
            var cachedData = await db.StringGetAsync(cacheKey);

            if (cachedData.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(cachedData!);
        }

        public async Task RemoveCachedDataAsync(string cacheKey)
        {
            var db = _multiplexer.GetDatabase();
            await db.KeyDeleteAsync(cacheKey);
        }

        public async Task SetCachedDataAsync<T>(string cacheKey, T data, TimeSpan expireTime)
        {
            var db = _multiplexer.GetDatabase();
            var serializedData = JsonSerializer.Serialize(data);
            await db.StringSetAsync(cacheKey, serializedData, expireTime);
        }
    }
}
