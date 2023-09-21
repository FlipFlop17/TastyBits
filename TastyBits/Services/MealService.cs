using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TastyBits.Data;
using TastyBits.Interfaces;
using TastyBits.Model;
using TastyBits.Model.Dto;

namespace TastyBits.Services
{
    /// <summary>
    /// Maps the database service results to the appropriate property names of UserMeal class
    /// </summary>
    public class MealService
    {
        private readonly IDbService _databaseService;
        private readonly TastyDialogService _tastyDialogService;

        public MealService(IDbService databaseService,TastyDialogService dialogService)
        {
            _databaseService = databaseService;
            _tastyDialogService = dialogService;
        }

        /// <summary>
        /// Takes properties from UserMeal class (input form) and adds them to the appropriate tables to database
        /// </summary>
        /// <returns></returns>
        public async Task<TaskResult> AddNewMealAsync(UserMeal meal)
        {
            //pass Dto so you have all data for insertion
            MealDto dtoMeal=new MealDto();
            dtoMeal.Ingredients = new();
            dtoMeal.Name = meal.Name;
            foreach (UserMeal.Ingridient item in meal.Ingredients) {
                dtoMeal.Ingredients.Add(item.Name, item.Quantity);
            }
            dtoMeal.Description = meal.Description;
            dtoMeal.CookingTime = meal.CookingTime;
            dtoMeal.PrepTime = meal.PrepTime;
            dtoMeal.ServingsAmount = meal.ServingAmount;
            foreach (var item in meal.Images) {
                dtoMeal.Images.Add(item);
            }
            dtoMeal.UserId = meal.UserId;
            return await _databaseService.InsertNewMealAsync(dtoMeal);
        }

        /// <summary>
        /// Gets all users recipes and transforms the property data to UserMeal class
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserMeal>> GetAllUserMealsAsync()
        {
            List<UserMeal> mealList = new List<UserMeal>();
            var mealTable = await _databaseService.GetAllRecipes();
            //pass out the dto and the remap it to the UserMeal ??
            foreach (Meals item in mealTable) {
                UserMeal newMeal = new UserMeal();
                newMeal.MealId=Convert.ToString(item.Id);
                newMeal.Name = item.Name;
                newMeal.Description = item.Description;
                newMeal.CookingTime= item.CookingTime;
                newMeal.PrepTime = item.PrepTime;
                if (item.RecipeImages.Count>0) {
                    foreach (var image in item.RecipeImages) {
                        newMeal.Images.Add(image.ImageData.ToString());
                    }
                }else {
                    //TODO add a placeholder image
                    //newMeal.Images.Add("");
                }
                mealList.Add(newMeal);
            }
            return mealList;
        }
    }
}
