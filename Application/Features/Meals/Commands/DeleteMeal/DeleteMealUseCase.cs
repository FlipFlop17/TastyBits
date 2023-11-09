using Application.Cache;
using Application.Features.Meals.Commands.CreateMeal;
using Domain.ReturnModels;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Meals.Commands.DeleteMeal
{
    public class DeleteMealUseCase
    {
        private readonly ICache _mealsCacheRepository;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger<CreateMealUseCase> _logger;
        private AppDbContext _dbContext { get; set; }

        public DeleteMealUseCase(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<CreateMealUseCase> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<TaskResult> DeleteMealAsync(int mealId)
        {
            TaskResult actionResult = new();

            try
            {
                await using (_dbContext = await _dbContextFactory.CreateDbContextAsync())
                {
                    //dodaj joinove na ostale tablice i spremi sve u neki DTO za prikaz po ekranima
                    //string sql = "select * from Meals m inner join  RecipeImage ri on ri.MealId=m.Id";
                    //var result = _dbContext.Meals.FromSqlInterpolated($"{sql}").ToList();

                    var result = _dbContext.MealsTable.SingleOrDefault(m => m.Id == mealId);
                    if (result != null)
                    {
                        result.ValidUntil = DateTime.Now;
                        _dbContext.SaveChanges();
                        actionResult.StatusCode = System.Net.HttpStatusCode.OK;
                        _logger.LogInformation("meal deleted from database");
                    }
                    else
                    {
                        actionResult.HasError = true;
                        actionResult.ErrorDesc = "unknown error while deleting record from database";
                        actionResult.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _logger.LogError($"error when deleting: insert result: {result}");
                    }
                }
            }
            catch (Exception e)
            {
                actionResult.HasError = true;
                actionResult.ErrorDesc = "unknown error while deleting record from database";
                actionResult.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _logger.LogError($"error when deleting: {e}");
            }
            return actionResult;
        }
    }
}
