using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TastyBits.Model;

namespace TastyBits.Data;

public class AppDbContext:IdentityDbContext
{
    public DbSet<Recipe> Recipes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
      
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("TastySchema");
        base.OnModelCreating(builder);
    }
}
