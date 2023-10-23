using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;

namespace Application.UseCases
{
    public class CreateMealUseCase
    {
        private readonly ICache _mealsCacheRepository;
        private readonly CalorieApiService _calorieApi;

        public CreateMealUseCase(ICache mealsCacheRepository,CalorieApiService calorieApi)
        {
            _mealsCacheRepository = mealsCacheRepository;
            _calorieApi = calorieApi;
        }

        public async Task<TaskResult> InsertNewMealAsync(UserMeal meal)
        {
            return await _mealsCacheRepository.AddMeal(meal);
        }

    }
}
