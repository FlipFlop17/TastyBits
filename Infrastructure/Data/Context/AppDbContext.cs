using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<MealsDataEntity> MealsTable { get; set; }
        public DbSet<IngredientsDataEntity> IngredientsTable { get; set; }
        public DbSet<MealImageDataEntity> RecipeImageTable { get; set; }
        public DbSet<MealIngredientsDataEntity> RecipeIngredientsTable { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("TastySchema");
            builder.Entity<MealsDataEntity>().ToTable("Meals");
            builder.Entity<IngredientsDataEntity>().ToTable("Ingredients");
            builder.Entity<MealImageDataEntity>().ToTable("RecipeImage");
            builder.Entity<MealIngredientsDataEntity>().ToTable("RecipeIngredients");
            base.OnModelCreating(builder);
        }
    }
}
