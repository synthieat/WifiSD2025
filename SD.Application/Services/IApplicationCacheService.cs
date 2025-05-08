using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Services
{
    public interface IApplicationCacheService
    {
        T RetrieveFromCache<T>(string key, Func<T> callback, TimeSpan? absoluteExpirationToNow = null);
        Task<T> RetrieveFromCacheAsync<T>(string key, Func<Task<T>> callback, TimeSpan? absoluteExpirationToNow = null);

        void ClearCache();
        void RemoveFromCach(string key);
    }
}
