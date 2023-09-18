using Microsoft.EntityFrameworkCore;
using Serilog;
using TastyBits.Data;
using TastyBits.Interfaces;
using TastyBits.Model;
using TastyBits.Model.Dto;

namespace TastyBits.Services
{
    public class DbService : IDbService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        private AppDbContext _dbContext { get; set; }

        public DbService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<TaskResult> InsertNewMealAsync(MealDto newMealDto)
        {
            TaskResult result=new TaskResult();
            await using (_dbContext =await _dbFactory.CreateDbContextAsync()) {
                Meals dbMeal = new Meals();
                dbMeal.UserId=newMealDto.UserId;
                dbMeal.Name = newMealDto.Name;
                dbMeal.Description = newMealDto.Description;
                dbMeal.CookingTime = newMealDto.CookingTime;
                dbMeal.PrepTime=newMealDto.PrepTime;
                dbMeal.ServingsAmount= newMealDto.ServingsAmount;
                
                _dbContext.Meals.Add(dbMeal);
                //TODO add in table recipeingridients and in ingridients
                int insertedMealId = await _dbContext.SaveChangesAsync();
                if (insertedMealId >0) {
                    Log.Information($"inserted id: {insertedMealId} meal");
                    //insert to other tables
                    foreach (var item in newMealDto.Ingredients) {
                        _dbContext.Ingredients.Add(new Ingredients() { Name = item.Key });
                        int ingredientId = await _dbContext.SaveChangesAsync();
                        if (ingredientId > 0) {
                            Log.Information($"inserted ingredient id: {ingredientId}");
                            _dbContext.RecipeIngredients.Add(new RecipeIngredients() { IngredientId=ingredientId,Quantity=item.Value});
                            _= await _dbContext.SaveChangesAsync();
                        }
                    }
                    foreach (var item in newMealDto.Images) {
                        _dbContext.RecipeImage.Add(new RecipeImage() { ImageData = item, MealId = insertedMealId });
                    }
                } else {
                    result.HasError=true;
                    result.ErrorDesc = "Save to Meals table no success";
                }
                return result;
            }
        }
        public async Task<List<Meals>> GetAllRecipes()
        {
            await using (_dbContext = await _dbFactory.CreateDbContextAsync()) {
                //TODO implement the creation of list UserMeal
                return await _dbContext.Meals.ToListAsync();
            }
        }
    }
}
