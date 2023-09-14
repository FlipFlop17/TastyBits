using Microsoft.EntityFrameworkCore;
using TastyBits.Data;
using TastyBits.Model;

namespace TastyBits.Services
{
    public class DbService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        private AppDbContext _dbContext { get; set; }

        public DbService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<bool> InsertNewRecipeAsync(Recipe recipe)
        {
            await using (_dbContext =await _dbFactory.CreateDbContextAsync()) {
                _dbContext.Recipes.Add(recipe);
                int result = await _dbContext.SaveChangesAsync();
                if (result != null) {
                    return true;
                }
                return false;
            }
        }
        public async Task<List<Recipe>> GetAllRecipes()
        {
            await using (_dbContext = await _dbFactory.CreateDbContextAsync()) {
                return await _dbContext.Recipes.ToListAsync();
            }
        }
    }
}
