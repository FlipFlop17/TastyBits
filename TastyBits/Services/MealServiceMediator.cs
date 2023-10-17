using Serilog;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using TastyBits.Interfaces;
using TastyBits.Model;
using TastyBits.Model.Dto;

namespace TastyBits.Services
{
    /// <summary>
    /// Maps the database service results to the appropriate property names of UserMeal class for display
    /// ...and the other way around
    /// </summary>
    public class MealServiceMediator
    {
        private readonly IDbService _databaseService;
        private readonly CalorieApiService _calorieApi;

        public MealServiceMediator(IDbService databaseService,CalorieApiService _calorieApiService)
        {
            _databaseService = databaseService;
            this._calorieApi = _calorieApiService;
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
                if (item.Name != null && item.Name.Length>0) {
                    //get the calories of the entered ingredient
                    CalorieNinjaApiResultModel apiResult = new();
                    //get macros per 100g of inputed ingridient
                    var taskRes = await _databaseService.GetIngridient(item.Name);
                    if (taskRes.StatusCode==System.Net.HttpStatusCode.OK) {
                        //already in database
                        item.CaloriesPer100g = ConvertIngridientDbToUserMealIng((Ingredients)taskRes.Result).CaloriesPer100g;
                    } else {
                        apiResult = await _calorieApi.GetCalorieAsync(item.Name);
                        if (apiResult.Items.Count > 0) {
                            item.CaloriesPer100g = apiResult.Items.FirstOrDefault().CaloriesPer100;
                            dtoMeal.Ingredients.Add(item); //insert into database
                        } else {
                            Log.Warning($"calorie api returned no results for:{item.Name}");
                        }
                    }
                }
            }
            foreach (var item in meal.TimeOfDayMeal) {
                switch (item) {
                    case TimeOfDayMeal.Breakfast:
                        dtoMeal.IsBreakfast = true;
                        break;
                    case TimeOfDayMeal.Dinner:
                        dtoMeal.IsDinner = true;
                        break;
                    case TimeOfDayMeal.Lunch:
                        dtoMeal.IsLunch = true;
                        break;
                    case TimeOfDayMeal.Snack:
                        dtoMeal.IsSnack = true;
                        break;
                    case TimeOfDayMeal.Dessert:
                        dtoMeal.IsDesert = true;
                        break;
                    default:
                        break;
                }
            }
            //TODO add help link inside dashboard
            dtoMeal.Description = meal.Description;
            dtoMeal.CookingTime = meal.CookingTime;
            dtoMeal.PrepTime = meal.PrepTime;
            dtoMeal.ServingsAmount = meal.ServingAmount;
            dtoMeal.Instrunctions= meal.Instructions;
            foreach (var item in meal.Images) {
                dtoMeal.Images.Add(item);
            }
            dtoMeal.Ingredients.Clear();
            foreach (var item in meal.Ingredients) {
                dtoMeal.Ingredients.Add(item);
            }
            dtoMeal.UserId = meal.UserId;
            return await _databaseService.InsertNewMealAsync(dtoMeal);
        }

        /// <summary>
        /// Gets all users recipes and transforms the property data to UserMeal class
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserMeal>> GetAllUserMealsAsync(string userId)
        {
            List<UserMeal> mealList = new List<UserMeal>();
            var mealTable = await _databaseService.GetAllUserRecipesAsync(userId);
            //pass out the dto and the remap it to the UserMeal ??
            foreach (Meals dbMeal in mealTable) {
                UserMeal newMeal = new UserMeal();
                newMeal.MealId=(dbMeal.Id);
                newMeal.Name = dbMeal.Name;
                newMeal.Description = dbMeal.Description;
                newMeal.CookingTime= dbMeal.CookingTime;
                newMeal.PrepTime = dbMeal.PrepTime;
                newMeal.ServingAmount = dbMeal.ServingsAmount;
                newMeal.Instructions=dbMeal.Instructions;
                foreach (var mIng in dbMeal.RecipeIngridients) {
                    UserMeal.Ingridient ingridient = new ();
                    ingridient.Quantity=mIng.Quantity;
                    ingridient.Name = mIng.Ingredients.Name;
                    ingridient.CaloriesPer100g = mIng.Ingredients.CaloriesPer100Gram;
                    ingridient.Quantity = mIng.Quantity;
                    ingridient.QuantityUnit = (QuantityUnit)Enum.Parse(typeof(QuantityUnit), mIng.QuantityUnit); 
                    newMeal.Ingredients.Add(ingridient);
                }
                newMeal.TimeOfDayMeal = GetTimesOfDay(dbMeal);
                if (dbMeal.RecipeImages.Count>0) {
                    foreach (var image in dbMeal.RecipeImages) {
                        newMeal.Images.Add(image.ImageData.ToString());
                    }
                }
                mealList.Add(newMeal);
            }
            return mealList;
        }

        private Collection<TimeOfDayMeal> GetTimesOfDay(Meals dbMeal)
        {
            Collection<TimeOfDayMeal> times = new();
            if (dbMeal.IsBreakfast) {
                times.Add(TimeOfDayMeal.Breakfast);
            }
            if (dbMeal.IsDesert) {
                times.Add(TimeOfDayMeal.Dessert);
            }
            if (dbMeal.IsDinner) {
                times.Add(TimeOfDayMeal.Dinner);
            }
            if (dbMeal.IsLunch) {
                times.Add(TimeOfDayMeal.Lunch);
            }
            return times;
        }
        public async Task<TaskResult> DeleteMealFromDatabaseAsync(UserMeal meal)
        {
            MealDto dtoMeal = new MealDto();
            dtoMeal.MealId = meal.MealId;

            var deleteResults = await _databaseService.UpdateMealValidUntil(dtoMeal);
            return deleteResults;
        }

        /// <summary>
        /// Converts the ingridient from database to workable object
        /// </summary>
        /// <param name="ingredientsDb"></param>
        /// <returns>UserMeal.Ingridient</returns>
        private UserMeal.Ingridient ConvertIngridientDbToUserMealIng(Ingredients ingredientsDb)
        {
            UserMeal.Ingridient ingridient = new UserMeal.Ingridient();
            ingridient.CaloriesPer100g = ingredientsDb.CaloriesPer100Gram;
            return ingridient;
        }
    }
}
