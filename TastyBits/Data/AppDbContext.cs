using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TastyBits.Data;

public class AppDbContext:IdentityDbContext
{
    public AppDbContext(DbContextOptions options):base(options)
    {
            
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("TastySchema");
        base.OnModelCreating(builder);
    }
}
