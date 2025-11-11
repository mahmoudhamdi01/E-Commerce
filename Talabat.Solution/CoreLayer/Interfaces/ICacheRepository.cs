using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Interfaces
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string Cachekey);
        Task SetAsync(string Cachekey, string CacheValue, TimeSpan TimeToLive);
    }
}
