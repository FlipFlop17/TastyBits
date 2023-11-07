using Application.Cache;
using Application.Common.Mappers;
using Application.Features.Meals.Commands.CreateMeal;
using Domain.Models;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Meals.Queries
{
    public class GetUserMealsById
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger<CreateMealUseCase> _logger;

        public GetUserMealsById(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<CreateMealUseCase> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        private AppDbContext _dbContext { get; set; }

        public async Task<List<UserMeal>> GetUserMeals(string userId)
        {
            _logger.LogInformation($"[TASTY BITS] fetching meals: [{userId}]");
            List<UserMeal> userMealList = new List<UserMeal>();
            await using (_dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                var result = _dbContext.MealsTable
                    .Where(m => m.ValidUntil == null & m.UserId == userId)
                    .Include(r => r.RecipeIngridients.Where(ing => ing.ValidUntil == null))
                        .ThenInclude(i => i.Ingredients)
                    .Include(m => m.RecipeImages.Where(m => m.ValidUntil == DateTime.MinValue)).ToList();
                foreach (var item in result)
                {
                    userMealList.Add(TastyMapper.ConvertUserMealDataEntityToUserMealModel(item));
                }
            }

            return userMealList;
        }
    }
}