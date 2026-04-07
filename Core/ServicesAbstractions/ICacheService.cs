using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface ICacheService
    {
        Task<T?> GetCachedDataAsync<T>(string cacheKey);
        Task SetCachedDataAsync<T>(string cacheKey, T data, TimeSpan expireTime);
        Task RemoveCachedDataAsync(string cacheKey);
    }
}
