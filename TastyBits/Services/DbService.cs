using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using TastyBits.Data;
using TastyBits.Interfaces;
using TastyBits.Model;
using TastyBits.Model.Dto;

namespace TastyBits.Services
{
    public class DbService : IDbService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CalorieApiService _calorieApi;

        private AppDbContext _dbContext { get; set; }

        public DbService(IDbContextFactory<AppDbContext> dbFactory,UserManager<IdentityUser> _userManager,CalorieApiService _calorieApi)
        {
            _dbFactory = dbFactory;
            this._userManager = _userManager;
            this._calorieApi = _calorieApi;
        }

        public async Task<TaskResult> InsertNewMealAsync(MealDto incomingMealDto)
        {
            TaskResult result=new TaskResult();
            await using (_dbContext =await _dbFactory.CreateDbContextAsync()) {

                using var transaction=await _dbContext.Database.BeginTransactionAsync();

                try {
                    Meals dbMeal = new Meals();
                    dbMeal.UserId = incomingMealDto.UserId;
                    dbMeal.Name = incomingMealDto.Name;
                    dbMeal.Description = incomingMealDto.Description;
                    dbMeal.CookingTime = incomingMealDto.CookingTime;
                    dbMeal.PrepTime = incomingMealDto.PrepTime;
                    dbMeal.ServingsAmount = incomingMealDto.ServingsAmount;
                    dbMeal.IsBreakfast = incomingMealDto.IsBreakfast;
                    dbMeal.IsDinner = incomingMealDto.IsDinner;
                    dbMeal.IsLunch = incomingMealDto.IsLunch;
                    dbMeal.IsDesert = incomingMealDto.IsDesert;
                    dbMeal.IsSnack = incomingMealDto.IsSnack;
                    dbMeal.Instructions = incomingMealDto.Instrunctions;
                    _dbContext.Meals.Add(dbMeal);
                    int insertedRows;
                    insertedRows = await _dbContext.SaveChangesAsync();
                    if (insertedRows > 0) {
                        Log.Information($"inserted rows: {insertedRows}");
                        
                        //insert each ingridient
                        foreach (var incomingIngrd in incomingMealDto.Ingredients) {
                            var ingred=new Ingredients() { 
                                Name=incomingIngrd.Name,
                                CaloriesPer100Gram=incomingIngrd.CaloriesPer100g 
                            };
                            _dbContext.Ingredients.Add(ingred);
                            insertedRows = await _dbContext.SaveChangesAsync();

                            if (insertedRows > 0) {
                                Log.Information($"inserted ingredient");
                                var recipeIngred = new RecipeIngredients() {
                                    IngredientId = ingred.Id,
                                    MealId = dbMeal.Id,
                                    Quantity = incomingIngrd.Quantity,
                                    QuantityUnit = incomingIngrd.QuantityUnit.ToString()
                                };
                                _dbContext.RecipeIngredients.Add(recipeIngred);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        foreach (var item in incomingMealDto.Images) {
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
        public async Task<List<Meals>> GetAllUserRecipesAsync(string userId)
        {
            await using (_dbContext = await _dbFactory.CreateDbContextAsync()) {
                var result = _dbContext.Meals
                    .Where(m=>m.ValidUntil==null & m.UserId== userId)
                    .Include(r=>r.RecipeIngridients)
                        .ThenInclude(i=>i.Ingredients)
                    .Include(m=>m.RecipeImages).ToList();
                return result;
            }
        }

        public async Task<TaskResult> UpdateMealValidUntil(MealDto mealToDelete)
        {
            TaskResult actionResult = new();
            
            await using (_dbContext = await _dbFactory.CreateDbContextAsync()) {
                //dodaj joinove na ostale tablice i spremi sve u neki DTO za prikaz po ekranima
                //string sql = "select * from Meals m inner join  RecipeImage ri on ri.MealId=m.Id";
                //var result = _dbContext.Meals.FromSqlInterpolated($"{sql}").ToList();

                var result = _dbContext.Meals.SingleOrDefault(m => m.Id == mealToDelete.MealId);
                if (result !=null) {
                    result.ValidUntil=DateTime.Now;
                    _dbContext.SaveChanges();
                } else {
                    actionResult.HasError = true;
                    actionResult.ErrorDesc = "unknown error while deleting record from database";
                }

                return actionResult;
            }
        }

        public async Task<TaskResult> GetIngridient(string ingridient)
        {
            TaskResult res = new();
            await using (_dbContext = await _dbFactory.CreateDbContextAsync()) {
                res.Result=_dbContext.Ingredients.Where(i => i.Name == ingridient).FirstOrDefault();
            }

            if (res.Result ==null) {
                res.HasError = true;
                res.ErrorDesc = "Not found";
                res.StatusCode=HttpStatusCode.NotFound;
            } else {
                res.StatusCode = HttpStatusCode.OK;
            }
            return res;
        }

        //TODO -implement EDIT meal funcionality
    }
}
