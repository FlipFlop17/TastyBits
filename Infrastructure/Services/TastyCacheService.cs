using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class TastyCacheService : ICache,IDisposable
    {
        private readonly IDistributedCache _memoryCache;
        private readonly IMealsRepository _mealsRepository;
        private readonly ILogger<TastyCacheService> _logger;

        public event EventHandler<RepositoryEventArgs> RepositoryChanged;
        private const string _cacheKey= "meals";

        public TastyCacheService(IDistributedCache memoryCache,IMealsRepository mealsRepository,ILogger<TastyCacheService> logger)
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
            var mealsStringData=await _memoryCache.GetStringAsync(_cacheKey);
            if(!string.IsNullOrEmpty(mealsStringData)) {
                List<UserMeal> userMeals = JsonConvert.DeserializeObject<List<UserMeal>>(mealsStringData);
                userMeals=userMeals.Where(meal=>meal.UserId == userId).ToList();
                _logger.LogInformation("[TASTY BITS] fetched from cache");
                return userMeals;
            }else {
                //add to cache
                List<UserMeal> userMeals= await _mealsRepository.GetUserMealById(userId);
                if (userMeals.Count > 0) {
                    mealsStringData = JsonConvert.SerializeObject(userMeals);
                    await _memoryCache.SetStringAsync(_cacheKey, mealsStringData);
                    _logger.LogInformation("[TASTY BITS] added to cache");
                }
                return userMeals;
            }
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
