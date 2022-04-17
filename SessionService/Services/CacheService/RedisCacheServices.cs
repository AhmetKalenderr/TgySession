using Microsoft.Extensions.Caching.Distributed;

namespace SessionService.Services.CacheService
{
    public class RedisCacheServices
    {
        private readonly IDistributedCache _distributed;
        public RedisCacheServices(IDistributedCache distributed)
        {
            _distributed = distributed;
        }
        public async void DeleteRedisKey(string key)
        {
            if (_distributed.GetAsync(key) != null)
            {
               await _distributed.RemoveAsync(key);
                System.Console.WriteLine($"{key} id li data silindi");
            }
        }
    }
}
