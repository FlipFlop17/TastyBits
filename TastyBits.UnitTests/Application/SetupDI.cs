using Application.Cache;
using Infrastructure.APIs;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TastyBits.UnitTests.Application
{
    public class SetupDI
    {
        private readonly IConfiguration _config;

        public SetupDI()
        {
            // Get the directory of the current executable
            var currentDirectory =Directory.GetCurrentDirectory();

            var userSecretsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Microsoft",
                "UserSecrets");
            _config = new ConfigurationBuilder()
                .AddJsonFile(@$"{currentDirectory}\appsettings.json")
                .AddJsonFile(@$"{userSecretsPath}\f21b57ed-a634-428b-bd82-cfa808fcdc6a\secrets.json")
                .Build();

        }
        public IServiceCollection Services ()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            IServiceCollection services = new ServiceCollection ();

            services.AddDbContextFactory<AppDbContext>(options =>
            {
                var conString = _config.GetConnectionString("DefaultConnection");
                options.UseNpgsql(conString);
            });

            services.AddLogging(options =>
            {
                options.AddDebug();
                options.AddConsole();
            });

            services.AddSingleton(_config);
            services.AddScoped<ICache, TastyCacheService>();
            services.AddScoped<CalorieApiService>();
            services.AddDistributedMemoryCache(opt =>
            {
                opt.ExpirationScanFrequency = TimeSpan.FromMinutes(2);
            });

            services.Configure<DistributedCacheEntryOptions>(options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                options.SlidingExpiration = TimeSpan.FromMinutes(1);
            });
            return services;
        }

    }
}
