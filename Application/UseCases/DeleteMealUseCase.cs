using Application.Interfaces;
using Domain.Interfaces;
using Domain.ReturnModels;

namespace Application.UseCases
{
    public class DeleteMealUseCase
    {
        private readonly ICache _mealsCacheRepository;

        public DeleteMealUseCase(ICache mealsCacheRepository)
        {
            this._mealsCacheRepository = mealsCacheRepository;
        }

        public async Task<TaskResult> DeleteMealAsync(int mealId)
        {
            TaskResult result = new TaskResult();
            await _mealsCacheRepository.DeleteMeal(mealId);
            return result;
        }
    }
}
