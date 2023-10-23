using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;

namespace Application.UseCases
{
    public class UpdateMealUseCase
    {
        private readonly ICache _mealsCacheRepository;

        public UpdateMealUseCase(ICache mealsCacheRepository)
        {
            _mealsCacheRepository = mealsCacheRepository;
        }

        public async Task<TaskResult> UpdateMeal(UserMeal userMeal)
        {
            return await _mealsCacheRepository.UpdateMeal(userMeal);
        }
    }
}
