using TastyBits.Model;
using TastyBits.Model.Dto;

namespace TastyBits.Interfaces
{
    public interface IDbService
    {
        public Task<TaskResult> InsertNewMealAsync(MealDto newMeal);
        public Task<List<Meals>> GetAllUserRecipesAsync(string userId);
        public Task<TaskResult> UpdateMealValidUntil(MealDto newMeal);
    }
}
