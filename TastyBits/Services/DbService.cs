using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> InsertNewMealAsync(MealDto newMeal)
        {
            await using (_dbContext =await _dbFactory.CreateDbContextAsync()) {
                Meals dbMeal = new Meals();
                dbMeal.UserId=newMeal.UserId;
                dbMeal.Name = newMeal.Name;
                dbMeal.Description = newMeal.Description;
                dbMeal.CookingTime = Convert.ToInt32(newMeal.CookingTime);
                dbMeal.PrepTime=Convert.ToInt32(newMeal.PrepTime);

                _dbContext.Meals.Add(dbMeal);
                int insertedId = await _dbContext.SaveChangesAsync();
                if (insertedId >0) {
                    return true;
                }
                return false;
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
