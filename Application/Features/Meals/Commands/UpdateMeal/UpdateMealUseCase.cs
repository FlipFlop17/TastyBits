using Application.Common.Mappers;
using Application.Common.ReturnModels;
using Application.Features.Meals.Commands.CreateMeal;
using Domain.Models;
using Domain.ReturnModels;
using Infrastructure.APIs;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Application.Features.Meals.Commands.UpdateMeal
{
    public class UpdateMealUseCase
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger<CreateMealUseCase> _logger;
        private readonly CalorieApiService _calorieApiService;

        private AppDbContext _dbContext { get; set; }

        public UpdateMealUseCase(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<CreateMealUseCase> logger, CalorieApiService calorieApi)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _calorieApiService = calorieApi;
        }

        public async Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal)
        {
            TaskResult result = new();
            IDbContextTransaction transaction = null;
            try
            {
                await using (_dbContext = await _dbContextFactory.CreateDbContextAsync())
                {
                    transaction = await _dbContext.Database.BeginTransactionAsync();

                    //add any new ingredients to the base ingredient table
                    foreach (var item in newUpdatedMeal.Ingredients)
                    {
                        IngredientsDataEntity ingr = new();
                        //check if the ingredient exists in the database, if not then add it
                        var queryResult = _dbContext.IngredientsTable.Where(ing => ing.Id.Equals(item.Name)).FirstOrDefault();
                        if (queryResult is null)
                        {
                            //new ingredient, add it
                            CalorieNinjaApiResultModel apiResult = await _calorieApiService.GetCalorieAsync(item.Name);
                            if (apiResult.Items.Count == 1)
                            {
                                item.CaloriesPer100g = apiResult.Items.First().CaloriesPer100;
                            }
                            ingr = TastyMapper.ConvertIngredientsToIngredientsDataEntity(item);
                            _dbContext.IngredientsTable.Add(ingr);
                            await _dbContext.SaveChangesAsync();
                            item.IngredientId = ingr.Id;
                        }
                        else
                        {
                            item.IngredientId = queryResult.Id;
                        }
                    }

                    //edit any new images to the images table
                    List<MealImage> forRemoval = new();
                    foreach (var img in newUpdatedMeal.Images)
                    {
                        MealImageDataEntity imgEntity = new();
                        var queryResult = _dbContext.RecipeImageTable.Where(ing => ing.ImageData.ToLower().Equals(img.Data.ToLower())).FirstOrDefault();
                        if (queryResult != null)
                        {
                            //image already in the database-update it for new values
                            // Update the image with new values. Can only be deleted!
                            if (img.ValidUntil > DateTime.MinValue)
                            {
                                queryResult.ValidUntil = img.ValidUntil;
                                await _dbContext.SaveChangesAsync();
                                //remove it from the list of meal since we have update it in the database
                                forRemoval.Add(img);
                            }
                        }
                        else
                        {
                            //new image, will be added by Update command /cascade
                        }
                    }
                    foreach (var item in forRemoval)
                    {
                        newUpdatedMeal.Images.Remove(item);
                    }
                    var mealEntity = TastyMapper.ConvertUserMealToMealsEntity(newUpdatedMeal);
                    //_dbContext.RecipeImageTable.AttachRange(mealEntity.RecipeImages);
                    _dbContext.MealsTable.Update(mealEntity);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result.StatusCode = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                result.HasError = true;
                result.ErrorDesc = $"Unexpected error while updating meal {e}";
                _logger.LogError("update meal in db: " + e.ToString());
                await transaction.RollbackAsync();
            }
            transaction.Dispose();
            return result;
        }
    }
}