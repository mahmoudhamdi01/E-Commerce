using CoreLayer.Interfaces;
using ServiceAbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CacheService(ICacheRepository _cacheRepo) : ICacheService
    {
        public async Task<string?> GetAsync(string CacheKey)
        => await _cacheRepo.GetAsync(CacheKey);

        public async Task SetAsync(string CacheKey, object Value, TimeSpan TimeToLive)
        {
            var value = JsonSerializer.Serialize(Value);
            await _cacheRepo.SetAsync(CacheKey, value, TimeToLive);
        }
    }
}
