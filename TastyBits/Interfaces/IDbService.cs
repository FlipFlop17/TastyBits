using TastyBits.Model;
using TastyBits.Model.Dto;

namespace TastyBits.Interfaces
{
    public interface IDbService
    {
        public Task<bool> InsertNewMealAsync(MealDto newMeal);
        public Task<List<Meals>> GetAllRecipes();
    }
}
