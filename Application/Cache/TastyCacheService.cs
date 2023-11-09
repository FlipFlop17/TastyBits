using Domain.Models;
using Domain.ReturnModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Cache
{
    public class TastyCacheService : ICache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TastyCacheService> _logger;
        private const string _cacheKey= "meals";

        public TastyCacheService(IMemoryCache memoryCache,ILogger<TastyCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<TaskResult> AddMeal(UserMeal newMeal)
        {
            return new TaskResult();
        }

        public async Task<TaskResult> DeleteMeal(int mealId)
        {
            return new TaskResult();
        }

        public async Task<List<UserMeal>> GetUserMealById(string userId)
        {
            //inspect
            if(!_memoryCache.TryGetValue(_cacheKey,out List<UserMeal> cachedEntities)) {

                // If cache not found, retrieve the data from the database.
               // cachedEntities = await _mealsRepository.GetUserMealById(userId);
                // Cache the data.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2)); // Define your preferred expiration time

                _memoryCache.Set(_cacheKey, cachedEntities, cacheEntryOptions);
                _logger.LogInformation("[TASTY BITS] fetched from cache");
            }
            return cachedEntities;
        }

        public async Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal)
        {
            return new TaskResult();
        }
    }
}
