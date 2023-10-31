using Application.Helpers;
using Application.ReturnModels;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Repositories
{
    public class MealsRepository : IMealsRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger _logger;
        private readonly CalorieApiService _calorieApiService;

        public event EventHandler<RepositoryEventArgs> RepositoryChanged;

        private AppDbContext _dbContext { get; set; }

        public MealsRepository(IDbContextFactory<AppDbContext> dbContextFactory,ILogger<MealsRepository> logger,CalorieApiService calorieApiService)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _calorieApiService = calorieApiService;
        }

        public async Task<TaskResult> AddMeal(UserMeal newMeal)
        {
            TaskResult result = new();

            await using (_dbContext=await _dbContextFactory.CreateDbContextAsync()) {

                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                try {
                    //add ingredient to the base table of ingredients
                    foreach (UserMeal.Ingridient row in newMeal.Ingredients) {
                        IngredientsDataEntity ingr = new();
                        var ingredientAlreadyInDb = await GetIngredientAsync(row.Name, _dbContext);
                        //only add ingredients that are not already in the database
                        if (ingredientAlreadyInDb.StatusCode != System.Net.HttpStatusCode.OK) { //
                            //add to the ingredients list
                            CalorieNinjaApiResultModel apiResult=await _calorieApiService.GetCalorieAsync(row.Name);
                            if (apiResult.Items.Count == 1) {
                                row.CaloriesPer100g = apiResult.Items.First().CaloriesPer100;
                            }
                            ingr = TastyMapper.ConvertIngredientsToIngredientsDataEntity(row);
                            _dbContext.IngredientsTable.Add(ingr);
                            //_logger.LogInformation("ing added");
                            await _dbContext.SaveChangesAsync();
                            //assign the ingredient primary key to the list of new meal ingredients
                            row.IngredientId = ingr.Id;
                        }else {
                            row.IngredientId = (ingredientAlreadyInDb.Result as IngredientsDataEntity).Id;
                        }
                    }
                    MealsDataEntity newMealDto = TastyMapper.ConvertUserMealToMealsEntity(newMeal);
                    _dbContext.MealsTable.AddRange(newMealDto);

                    int insertedRows = await _dbContext.SaveChangesAsync();
                    if (insertedRows > 0) {
                        await transaction.CommitAsync();
                        result.StatusCode = System.Net.HttpStatusCode.OK;
                        _logger.LogInformation("meal successfuly inserted");
                        RepoChanged(ChangeType.Add);
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
                        RepoChanged(ChangeType.Delete);
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
                    .Include(r=> r.RecipeIngridients.Where(ing=> ing.ValidUntil==null))
                        .ThenInclude(i => i.Ingredients)
                    .Include(m => m.RecipeImages.Where(m=>m.ValidUntil ==DateTime.MinValue)).ToList();
                foreach (var item in result) {
                    userMealList.Add(TastyMapper.ConvertUserMealDataEntityToUserMealModel(item));
                }
            }

            return userMealList;
        }

        public async Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal)
        {
            TaskResult result = new();
            IDbContextTransaction transaction=null;
            try {
                await using (_dbContext = await _dbContextFactory.CreateDbContextAsync()) {

                    transaction = await _dbContext.Database.BeginTransactionAsync();

                    //add any new ingredients to the base ingredient table
                    foreach (var item in newUpdatedMeal.Ingredients) {
                        IngredientsDataEntity ingr = new();
                        //check if the ingredient exists in the database, if not then add it
                        var ingredientAlreadyInDb = await GetIngredientAsync(item.Name, _dbContext);
                        if (ingredientAlreadyInDb.StatusCode != System.Net.HttpStatusCode.OK) {
                            //new ingredient, add it
                            CalorieNinjaApiResultModel apiResult = await _calorieApiService.GetCalorieAsync(item.Name);
                            if (apiResult.Items.Count == 1) {
                                item.CaloriesPer100g = apiResult.Items.First().CaloriesPer100;
                            }
                            ingr = TastyMapper.ConvertIngredientsToIngredientsDataEntity(item);
                            _dbContext.IngredientsTable.Add(ingr);
                            await _dbContext.SaveChangesAsync();
                            item.IngredientId=ingr.Id;
                        }else {
                            item.IngredientId= (ingredientAlreadyInDb.Result as IngredientsDataEntity).Id;
                        }
                    }

                    //edit any new images to the images table
                    List<MealImage> forRemoval = new();
                    foreach (var img in newUpdatedMeal.Images) {
                        MealImageDataEntity imgEntity = new();
                        var imgAlreadyInDatabase = await GetImageAsync(img.Data, _dbContext);
                        if (imgAlreadyInDatabase.StatusCode == System.Net.HttpStatusCode.OK) {
                            //image already in the database-update it for new values
                            // Update the image with new values. Can only be deleted!
                            var dbImg = (imgAlreadyInDatabase.Result as MealImageDataEntity);
                            if (img.ValidUntil > DateTime.MinValue) {
                                dbImg.ValidUntil = img.ValidUntil;
                                await _dbContext.SaveChangesAsync();
                                //remove it from the list of meal since we have update it in the database
                                forRemoval.Add(img);
                            }
                        } else {
                            //new image, will be added by Update command /cascade
                        }
                    }
                    foreach (var item in forRemoval) {
                        newUpdatedMeal.Images.Remove(item);
                    }
                    var mealEntity = TastyMapper.ConvertUserMealToMealsEntity(newUpdatedMeal);
                    //_dbContext.RecipeImageTable.AttachRange(mealEntity.RecipeImages);
                    _dbContext.MealsTable.Update(mealEntity);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result.StatusCode = System.Net.HttpStatusCode.OK;
                    RepoChanged(ChangeType.Update);
                }
            }
            catch (Exception e) {
                result.HasError = true;
                result.ErrorDesc= $"Unexpected error while updating meal {e}";
                _logger.LogError("update meal in db: "+e.ToString());
                await transaction.RollbackAsync();
            }
            transaction.Dispose();
            return result;
        }

        /// <summary>
        /// Searches the database for the ingredient with the provided name
        /// </summary>
        /// <returns>TaskResult</returns>
        public async Task<TaskResult> GetIngredientAsync(string ingredientName,AppDbContext context=null,int ingredientId=0)
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
                IngredientsDataEntity queryResult;
                if (ingredientId>0) {
                    queryResult = dbCx.IngredientsTable
                        .Where(ing => ing.Id.Equals(ingredientId)).FirstOrDefault();
                } else {
                    queryResult = dbCx.IngredientsTable
                        .Where(ing => ing.Name.ToLower().Equals(ingredientName.ToLower())).FirstOrDefault();
                }
                

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
        
        private void RepoChanged(ChangeType changeType)
        {
            RepositoryChanged?.Invoke(this, new RepositoryEventArgs() { ChangeType = changeType });
        }
    }
}
