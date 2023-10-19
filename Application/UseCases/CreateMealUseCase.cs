using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;

namespace Application.UseCases
{
    public class CreateMealUseCase
    {
        private readonly IMealsRepository _mealsRepository;

        public CreateMealUseCase(IMealsRepository mealsRepository)
        {
            _mealsRepository = mealsRepository;
        }

        public async Task<TaskResult> InsertNewMealAsync(UserMeal meal)
        {
            return await _mealsRepository.AddMeal(meal);
        }

    }
}
