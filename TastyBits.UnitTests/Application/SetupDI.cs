using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public IServiceCollection Services ()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            IServiceCollection services = new ServiceCollection ();

            services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseNpgsql("Server=localhost;Port=5432;Database=FlipFlop_UAT;user id=postgres;sslmode=prefer;");
            });

            services.AddLogging(options =>
            {
                options.AddDebug();
                options.AddConsole();
            });
            
            services.AddScoped<IMealsRepository, MealsRepository>();
            return services;
        }

    }
}
