using Application.Helpers;
using Azure.Core;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing.Text;

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

                            int rowsInserted=0;
                            IngredientsDataEntity ingr=new();
                            var ingredientAlreadyInDb = await GetIngredientAsync(row.Name,_dbContext);
                            //only add ingredients that are not already in the database
                            if (ingredientAlreadyInDb.StatusCode != System.Net.HttpStatusCode.OK) { //
                                //add to the ingredients list
                                ingr = TastyMapper.ConvertIngredientsToIngredientsDataEntity(row);
                                _dbContext.IngredientsTable.Add(ingr);
                                rowsInserted = await _dbContext.SaveChangesAsync();
                            }
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
                            MealImageDataEntity imageDataEntity= new MealImageDataEntity();
                            imageDataEntity = TastyMapper.ConvertMealImageToImageEntity(item, newMealDto.Id);
                            var imageAlreadyInDb = await GetImageAsync(imageDataEntity.ImageData,_dbContext);
                            //only add an image if it does not already exists
                            if(imageAlreadyInDb.StatusCode != System.Net.HttpStatusCode.OK) {
                                _dbContext.RecipeImageTable.Add(imageDataEntity);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        result.StatusCode = System.Net.HttpStatusCode.OK;
                        _logger.LogInformation("meal successfuly inserted");
                    }
                }
                catch (Exception e) {
                    result.HasError = true;
                    result.ErrorDesc = $"Unexpected error while saving new meal {e}";
                    _logger.LogError($"--greška!: {e}");
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

        public async Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal)
        {
            TaskResult result = new();
            try {
                    await using (_dbContext = await _dbContextFactory.CreateDbContextAsync()) {

                        var mealEntity = TastyMapper.ConvertUserMealToMealsEntity(newUpdatedMeal);
                        _dbContext.MealsTable.Attach(mealEntity);    
                        _dbContext.MealsTable.Update(mealEntity);
                        await _dbContext.SaveChangesAsync();
                    }
                    result.StatusCode = System.Net.HttpStatusCode.OK;
                }
            catch (Exception e) {
                result.HasError = true;
                result.ErrorDesc= $"Unexpected error while updating meal {e}";
                _logger.LogError("update meal in db: "+e.ToString());
            }
            return result;
        }

        /// <summary>
        /// Searches the database for the ingredient with the provided name
        /// </summary>
        /// <returns>TaskResult</returns>
        public async Task<TaskResult> GetIngredientAsync(string ingredientName,AppDbContext context=null)
        {
            TaskResult result = new();

            if(context != null) {
                var _dbContext = context;
                result = GetData(_dbContext);
            } else {
                await using (_dbContext = await _dbContextFactory.CreateDbContextAsync()) {
                    result = GetData(_dbContext);
                }
            }
            TaskResult GetData(AppDbContext dbCx)
            {
                TaskResult result = new();
                var queryResult = dbCx.IngredientsTable
                        .Where(ing => ing.Name.ToLower().Equals(ingredientName.ToLower())).FirstOrDefault();

                if (queryResult != null) {
                    result.Result = queryResult;
                    result.StatusCode = System.Net.HttpStatusCode.OK;
                } else {
                    result.StatusCode = System.Net.HttpStatusCode.NotFound;
                }
                return result;
            }

            return result;
        }
        /// <summary>
        /// Searches the database for the images with the provided name
        /// </summary>
        /// <returns>TaskResult</returns>
        public async Task<TaskResult> GetImageAsync(string imageData,AppDbContext dbContext=null)
        {
            TaskResult result = new();
            if(dbContext != null) {
                var _dbContext = dbContext;
                result = GetData(_dbContext);
            } else {
                await using (_dbContext = await _dbContextFactory.CreateDbContextAsync()) {
                    result=GetData(_dbContext);
                }
            }

            TaskResult GetData(AppDbContext dbCx)
            {
                TaskResult result = new();
                var queryResult = dbCx.RecipeImageTable
                        .Where(ing => ing.ImageData.ToLower().Equals(imageData.ToLower())).FirstOrDefault();

                if (queryResult != null) {
                    result.Result = queryResult;
                    result.StatusCode = System.Net.HttpStatusCode.OK;
                } else {
                    result.StatusCode = System.Net.HttpStatusCode.NotFound;
                }
                return result;
            } 
            return result;
        }
    }
}
