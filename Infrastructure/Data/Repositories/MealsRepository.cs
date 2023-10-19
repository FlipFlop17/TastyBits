using Application.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Infrastructure.Data.Repositories
{
    public class MealsRepository : IMealsRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger _logger;

        private AppDbContext _dbContext { get; set; }

        public MealsRepository(IDbContextFactory<AppDbContext> dbContextFactory,ILogger<MealsRepository> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<TaskResult> AddMeal(UserMeal newMeal)
        {
            TaskResult result = new();
            MealsDataEntity newMealDto=TastyMapper.ConvertUserMealToMealsEntity(newMeal);
            await using (_dbContext=await _dbContextFactory.CreateDbContextAsync()) {

                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                try {
                    _dbContext.MealsTable.Add(newMealDto);
                    int insertedRows = await _dbContext.SaveChangesAsync();
                    if (insertedRows>0) {
                        //meal inserted-insert ingredients used
                        foreach (UserMeal.Ingridient row in newMeal.Ingredients) {
                            //TODO only add ingredients that are not already in the database
                            //add to the ingredients list
                            IngredientsDataEntity ingr = TastyMapper.ConvertIngredientsToIngredientsDataEntity(row);
                            _dbContext.IngredientsTable.Add(ingr);
                            int rowsInserted=await _dbContext.SaveChangesAsync();
                            
                            //insert to mealingredients table
                            if(rowsInserted>0) {
                                row.Id = ingr.Id;
                                MealIngredientsDataEntity mealIngr = TastyMapper.ConvertMealIngredientToMealIngEntity(row, newMealDto.Id);
                                _dbContext.RecipeIngredientsTable.Add(mealIngr);
                            }
                        }
                        _dbContext.SaveChanges();
                        // add images to database
                        foreach (var item in newMeal.Images) {
                            _dbContext.RecipeImageTable.Add(TastyMapper.ConvertMealImageToImageEntity(item, newMealDto.Id));
                        }
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        _logger.LogInformation("meal successfuly inserted");
                    }
                }
                catch (Exception e) {
                    result.HasError = true;
                    result.ErrorDesc = "Unexpected error while saving new meal";
                    _logger.LogError($"{e}");
                    await transaction.RollbackAsync();
                }
            }
            return result;
        }

        public async Task<TaskResult> DeleteMeal(int mealId)
        {
            TaskResult actionResult = new();

            try {
                await using (_dbContext = await _dbContextFactory.CreateDbContextAsync()) {
                    //dodaj joinove na ostale tablice i spremi sve u neki DTO za prikaz po ekranima
                    //string sql = "select * from Meals m inner join  RecipeImage ri on ri.MealId=m.Id";
                    //var result = _dbContext.Meals.FromSqlInterpolated($"{sql}").ToList();

                    var result = _dbContext.MealsTable.SingleOrDefault(m => m.Id == mealId);
                    if (result != null) {
                        result.ValidUntil = DateTime.Now;
                        _dbContext.SaveChanges();
                        actionResult.StatusCode = System.Net.HttpStatusCode.OK;
                        _logger.LogInformation("meal deleted from database");
                    } else {
                        actionResult.HasError = true;
                        actionResult.ErrorDesc = "unknown error while deleting record from database";
                        actionResult.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _logger.LogError($"error when deleting: insert result: {result}");
                    }
                }
            }
            catch (Exception e) {
                actionResult.HasError = true;
                actionResult.ErrorDesc = "unknown error while deleting record from database";
                actionResult.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _logger.LogError($"error when deleting: {e}");
            }
            return actionResult;
        }

        public async Task<List<UserMeal>> GetUserMealById(string userId)
        {
            _logger.LogInformation($"[TASTY BITS] fetching meals: [{userId}]");
            List<UserMeal> userMealList = new List<UserMeal>();
            await using (_dbContext = await _dbContextFactory.CreateDbContextAsync()) {
                var result = _dbContext.MealsTable
                    .Where(m => m.ValidUntil == null & m.UserId == userId)
                    .Include(r=> r.RecipeIngridients)
                        .ThenInclude(i => i.Ingredients)
                    .Include(m => m.RecipeImages).ToList();
                foreach (var item in result) {
                    userMealList.Add(TastyMapper.ConvertUserMealDataEntityToUserMealModel(item));
                }
            }

            return userMealList;
        }

        public Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal)
        {
            throw new NotImplementedException();
        }
    }
}
