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

                using var transaction=await _dbContext.Database.BeginTransactionAsync();

                try {
                    Meals dbMeal = new Meals();
                    dbMeal.UserId = newMealDto.UserId;
                    dbMeal.Name = newMealDto.Name;
                    dbMeal.Description = newMealDto.Description;
                    dbMeal.CookingTime = newMealDto.CookingTime;
                    dbMeal.PrepTime = newMealDto.PrepTime;
                    dbMeal.ServingsAmount = newMealDto.ServingsAmount;

                    _dbContext.Meals.Add(dbMeal);
                    int insertedRows;
                    insertedRows = await _dbContext.SaveChangesAsync();
                    if (insertedRows > 0) {
                        Log.Information($"inserted id: {insertedRows} meal");
                        //insert to other tables
                        foreach (var item in newMealDto.Ingredients) {
                            var ingred=new Ingredients() { Name=item.Key};
                            _dbContext.Ingredients.Add(ingred);
                            insertedRows = await _dbContext.SaveChangesAsync();
                            if (insertedRows > 0) {
                                Log.Information($"inserted ingredient");
                                var recipeIngred = new RecipeIngredients() { IngredientId = ingred.Id, Quantity = item.Value };
                                _dbContext.RecipeIngredients.Add(recipeIngred);
                                _ = await _dbContext.SaveChangesAsync();
                            }
                        }
                        foreach (var item in newMealDto.Images) {
                            _dbContext.RecipeImage.Add(new RecipeImage() { ImageData = item, MealId = dbMeal.Id });
                        }
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    } else {
                        result.HasError = true;
                        result.ErrorDesc = "Save to Meals table no success";
                        await transaction.RollbackAsync();
                    }
                }
                catch (Exception) {
                    result.HasError = true;
                    result.ErrorDesc = "Save to Meals table no success";
                    await transaction.RollbackAsync();
                }
               
                return result;
            }
        }
        public async Task<List<Meals>> GetAllRecipes()
        {
            await using (_dbContext = await _dbFactory.CreateDbContextAsync()) {
                //dodaj joinove na ostale tablice i spremi sve u neki DTO za prikaz po ekranima
                //string sql = "select * from Meals m inner join  RecipeImage ri on ri.MealId=m.Id";
                //var result = _dbContext.Meals.FromSqlInterpolated($"{sql}").ToList();

                var result = _dbContext.Meals.Include(m=>m.RecipeImages).ToList();

                return result;
            }
        }
    }
}
