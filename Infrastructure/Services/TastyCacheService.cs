using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class TastyCacheService : ICache,IDisposable
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMealsRepository _mealsRepository;
        private readonly ILogger<TastyCacheService> _logger;

        public event EventHandler<RepositoryEventArgs> RepositoryChanged;
        private const string _cacheKey= "meals";

        public TastyCacheService(IMemoryCache memoryCache,IMealsRepository mealsRepository,ILogger<TastyCacheService> logger)
        {
            _memoryCache = memoryCache;
            _mealsRepository = mealsRepository;
            _logger = logger;
            _mealsRepository.RepositoryChanged += DataRepositoryChanged;
        }

        private void DataRepositoryChanged(object? sender, RepositoryEventArgs e)
        {
            //repo data has changed
            //clear cache so it can be updated
            _logger.LogDebug("[TASTY BITS] repo has been changed-removing cache");
            _memoryCache.Remove(_cacheKey);
        }


        public async Task<TaskResult> AddMeal(UserMeal newMeal)
        {
            return await _mealsRepository.AddMeal(newMeal);
        }

        public async Task<TaskResult> DeleteMeal(int mealId)
        {
            return await _mealsRepository.DeleteMeal(mealId);
        }

        public async Task<List<UserMeal>> GetUserMealById(string userId)
        {
            //inspect
            if(!_memoryCache.TryGetValue(_cacheKey,out List<UserMeal> cachedEntities)) {

                // If cache not found, retrieve the data from the database.
                cachedEntities = await _mealsRepository.GetUserMealById(userId);
                // Cache the data.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2)); // Define your preferred expiration time

                _memoryCache.Set(_cacheKey, cachedEntities, cacheEntryOptions);
                _logger.LogInformation("[TASTY BITS] fetched from cache");
            }
            return cachedEntities;
        }

        public Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal)
        {
            return _mealsRepository.UpdateMeal(newUpdatedMeal);
        }

        public void Dispose()
        {
            _mealsRepository.RepositoryChanged -= RepositoryChanged;
        }
    }
}
