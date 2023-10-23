using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.AddScoped<IMealsRepository, MealsRepository>();
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
