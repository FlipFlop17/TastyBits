using Application.Interfaces;
using Application.Services;
using Application.UseCases;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor.Services;
using Serilog;
using TastyBits.Services;

var builder = WebApplication.CreateBuilder(args);

var mySerilog =  new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Debug()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Services.AddLogging(loggB =>
{
    //loggB.AddFilter("Microsoft", LogLevel.Warning);
    //loggB.AddFilter("MudBlazor", LogLevel.Information);
    loggB.ClearProviders();
    loggB.AddSerilog(mySerilog,dispose: true);
});

//TODO dotnet ef --startup-project .\TastyBits\TastyBits.csproj --project .\Infrastructure\Infrastructure.csproj migrations add refreshFolder


builder.Services.AddRazorPages().AddRazorPagesOptions(options=>
{
    options.Conventions.AddAreaPageRoute("Identity","/Pages/Account/Login","login");
    //options.Conventions.AddAreaPageRoute("Identity", "/Pages/Account/Register", "register");
});
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<CreateMealUseCase>();
builder.Services.AddScoped<DeleteMealUseCase>();
builder.Services.AddScoped<UpdateMealUseCase>();
builder.Services.AddScoped<GetUserMealsById>();
builder.Services.AddScoped<IMealsRepository, MealsRepository>();
builder.Services.AddTransient<TastyDialogService>();
builder.Services.AddScoped<LoggedUserService>();
builder.Services.AddScoped<CalorieApiService>() ;
builder.Services.AddScoped<ICache, TastyCacheService>();
builder.Services.AddMemoryCache();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Configuration.AddEnvironmentVariables();

//DATABASE
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    string url = builder.Configuration.GetConnectionString("DefaultConnection");
    string conString;
    if (url == "DATABASE_URL") {
        url = builder.Configuration["DATABASE_URL"]; //get from environment, db is online
        conString = BuildConnectionStringFromUrl(url);
    } else { conString = url; }
    // log mysql connection string
    //Log.Information("url db context: " + builder.Configuration["DATABASE_URL"]);
    //options.UseNpgsql(conString);
    options.UseInMemoryDatabase("FlopsInMemory");
    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
});

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapRazorPages();
//
app.Run();

// Run this  update the database:
static string BuildConnectionStringFromUrl(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    string host = uri.Host;
    int port = uri.Port;
    string database = uri.AbsolutePath.TrimStart('/');
    string username = uri.UserInfo.Split(':')[0];
    string password = uri.UserInfo.Split(':')[1];

    string connectionString = $"Server={host};Port={port};Database={database};User Id={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

    return connectionString;
}
