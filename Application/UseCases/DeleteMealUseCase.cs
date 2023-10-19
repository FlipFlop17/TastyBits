using Domain.Interfaces;
using Domain.ReturnModels;

namespace Application.UseCases
{
    public class DeleteMealUseCase
    {
        private readonly IMealsRepository _mealsRepository;

        public DeleteMealUseCase(IMealsRepository mealsRepository)
        {
            _mealsRepository = mealsRepository;
        }

        public async Task<TaskResult> DeleteMealAsync(int mealId)
        {
            TaskResult result = new TaskResult();
            _mealsRepository.DeleteMeal(mealId);
            return result;
        }
    }
}
