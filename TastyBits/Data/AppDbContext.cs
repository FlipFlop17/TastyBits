using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TastyBits.Model.Dto;

namespace TastyBits.Data;

public class AppDbContext:IdentityDbContext
{
    public DbSet<Meals> Meals { get; set; }
    public DbSet<Ingredients> Ingredients { get; set; }
    public DbSet<RecipeImage> RecipeImage { get; set; }
    public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
      
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("TastySchema");
        base.OnModelCreating(builder);
    }
}
