using Domain.Models;

namespace Domain.Interfaces
{
    public interface IMealIngredientsRepository
    {
        MealIngredient GetUserMealById(int id);
        void AddMealIngredient(MealIngredient newMealIngredient);
        void UpdateMealIngredient(UserMeal newUpdatedMealIngredient);
        void DeleteMealIngredient(int mealId);
    }
}
