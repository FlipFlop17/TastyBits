using Application.Interfaces;
using Domain.Models;

namespace Application.UseCases
{
    public class GetUserMealsById
    {
        private readonly ICache _mealsCacheRepository;

        public GetUserMealsById(ICache mealsCacheRepository)
        {
            _mealsCacheRepository = mealsCacheRepository;
        }

        public async Task<List<UserMeal>> GetUserMeals(string userId)
        {
            return await _mealsCacheRepository.GetUserMealById(userId);
        }
    }
}
