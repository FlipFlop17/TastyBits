using Domain.Models;

namespace Domain.Interfaces
{
    public interface IIngredientsRepository
    {
        Ingredient GetUserMealById(int id);
        void AddIngredient(Ingredient newIngredient);
        void UpdateIngredient(Ingredient newUpdatedIngredient);
        void DeleteIngredient(int ingredientId);
    }
}
