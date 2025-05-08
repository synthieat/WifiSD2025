using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static bool ClearCache(this IMemoryCache memoryCache)
        {
            // Info: _memoryCache is of type IMemoryCache and was set via dependency injection!
            var _memoryCache = memoryCache as MemoryCache;
            if (_memoryCache != null)
            {
                ///ToDo: Use Clear method with .NET 7.x
                _memoryCache.Clear();
                return true;
            }
            return false;
        }

        public static T RetrieveFromCache<T>(this IMemoryCache memoryCache, string key, Func<T> callback, TimeSpan absoluteExpirationRelativeToNow = default)
        {
            if (!memoryCache.TryGetValue(key, out T result))
            {
                result = callback();
                memoryCache.SetValueByAbsoluteExpiration(key, callback, absoluteExpirationRelativeToNow);
            }

            return result;
        }

        public static async Task<T> RetrieveFromCacheAsync<T>(this IMemoryCache memoryCache, string key, Func<Task<T>> callback, TimeSpan absoluteExpirationRelativeToNow = default)
        {
            if (!memoryCache.TryGetValue(key, out T result))
            {
                T? callResult = default;
                try
                {
                    callResult = await callback();
                    memoryCache.SetValueByAbsoluteExpiration(key, callResult, absoluteExpirationRelativeToNow);
                    result = callResult;
                }
                catch
                {
                    result = callResult;
                }


            }
            return result;
        }

        private static T SetValueByAbsoluteExpiration<T>(this IMemoryCache memoryCache, string key, T value, TimeSpan absoluteExpirationRelativeToNow)
        {
            return absoluteExpirationRelativeToNow == default
                                                    ? memoryCache.Set(key, value)
                                                    : memoryCache.Set(key, value, absoluteExpirationRelativeToNow);
        }
    }
}
