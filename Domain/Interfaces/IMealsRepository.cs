using Domain.Models;
using Domain.ReturnModels;

namespace Domain.Interfaces
{
    public interface IMealsRepository
    {
        Task<List<UserMeal>> GetUserMealById(string userId);
        Task<TaskResult> AddMeal(UserMeal newMeal);
        Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal);
        Task<TaskResult> DeleteMeal(int mealId);
        event EventHandler<RepositoryEventArgs> RepositoryChanged;

    }
}
