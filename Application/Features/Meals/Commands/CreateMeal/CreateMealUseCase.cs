using Application.Common.Mappers;
using Application.Common.ReturnModels;
using Domain.Models;
using Domain.ReturnModels;
using Infrastructure.APIs;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Meals.Commands.CreateMeal
{
    public class CreateMealUseCase
    {
        private readonly CalorieApiService _calorieApiService;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger<CreateMealUseCase> _logger;

        public CreateMealUseCase(CalorieApiService calorieApi, IDbContextFactory<AppDbContext> dbContextFactory, ILogger<CreateMealUseCase> logger)
        {
            _calorieApiService = calorieApi;
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        private AppDbContext _dbContext { get; set; }

        public async Task<TaskResult> InsertNewMealAsync(UserMeal newMeal)
        {
            TaskResult result = new();

            await using (_dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                try
                {
                    //add ingredient to the base table of ingredients
                    foreach (UserMeal.Ingridient row in newMeal.Ingredients)
                    {
                        IngredientsDataEntity ingr = new();
                        var queryResult = _dbContext.IngredientsTable.Where(ing => ing.Id.Equals(row.Name)).FirstOrDefault();
                        //only add ingredients that are not already in the database
                        if (queryResult is null)
                        { //
                            //add to the ingredients list
                            CalorieNinjaApiResultModel apiResult = await _calorieApiService.GetCalorieAsync(row.Name);
                            if (apiResult.Items != null && apiResult.Items.Count == 1)
                            {
                                row.CaloriesPer100g = apiResult.Items.First().CaloriesPer100;
                            }
                            ingr = TastyMapper.ConvertIngredientsToIngredientsDataEntity(row);
                            _dbContext.IngredientsTable.Add(ingr);
                            //_logger.LogInformation("ing added");
                            await _dbContext.SaveChangesAsync();
                            //assign the ingredient primary key to the list of new meal ingredients
                            row.IngredientId = ingr.Id;
                        }
                        else
                        {
                            row.IngredientId = queryResult.Id;
                        }
                    }
                    MealsDataEntity newMealDto = TastyMapper.ConvertUserMealToMealsEntity(newMeal);
                    _dbContext.MealsTable.AddRange(newMealDto);

                    int insertedRows = await _dbContext.SaveChangesAsync();
                    if (insertedRows > 0)
                    {
                        await transaction.CommitAsync();
                        result.StatusCode = System.Net.HttpStatusCode.OK;
                        _logger.LogInformation("meal successfuly inserted");
                    }
                }
                catch (Exception e)
                {
                    result.HasError = true;
                    result.ErrorDesc = $"Unexpected error while saving new meal {e}";
                    _logger.LogError($"--greška!: {e}");
                    await transaction.RollbackAsync();
                }
            }
            return result;
        }

    }
}