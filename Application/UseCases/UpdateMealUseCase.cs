using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;

namespace Application.UseCases
{
    public class UpdateMealUseCase
    {
        private readonly IMealsRepository _mealRepository;

        public UpdateMealUseCase(IMealsRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<TaskResult> UpdateMeal(UserMeal userMeal)
        {
            return await _mealRepository.UpdateMeal(userMeal);
        }
    }
}
