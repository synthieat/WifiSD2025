using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SD.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Services
{
    public class ApplicationCacheService : IApplicationCacheService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger logger;

        private readonly int ApplicationCacheAgeSize = 30;// 60 * 60 * 6; /* 6 Stunden */

        public ApplicationCacheService(IMemoryCache memoryCache,
                                       ILogger<ApplicationCacheService> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public T RetrieveFromCache<T>(string key, Func<T> callback, TimeSpan? absoluteExpirationToNow = null)
        {
            return memoryCache.RetrieveFromCache(key, callback, absoluteExpirationToNow ?? TimeSpan.FromSeconds(this.ApplicationCacheAgeSize));
        }

        public async Task<T> RetrieveFromCacheAsync<T>(string key, Func<Task<T>> callback, TimeSpan? absoluteExpirationToNow = null)
        {
            return await memoryCache.RetrieveFromCacheAsync(key, callback, absoluteExpirationToNow ?? TimeSpan.FromSeconds(this.ApplicationCacheAgeSize));
        }

        public void RemoveFromCach(string key)
        {
            this.memoryCache.Remove(key);
        }

        public void ClearCache()
        {
            var success = this.memoryCache.ClearCache();
            if (success)
            {
                this.logger.LogInformation("Application cache cleared!");
            }
            else
            {
                this.logger.LogError("Could not cast cache object to MemoryCache. Cache NOT cleared.");
            }
        }            

    }
}
