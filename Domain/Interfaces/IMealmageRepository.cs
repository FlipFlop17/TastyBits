using Domain.Models;

namespace Domain.Interfaces
{
    public interface IMealmageRepository
    {
        MealImage GetMealImageById(int id);
        void AddMealImage(MealImage newMealImg);
        void UpdateMealImage(MealImage newUpdatedMealImg);
        void DeleteMealImage(int mealImgId);
    }
}
