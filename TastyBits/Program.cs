using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;
using TastyBits.Data;
using TastyBits.Interfaces;
using TastyBits.Services;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug()
    .CreateLogger();

builder.Services.AddRazorPages().AddRazorPagesOptions(options=>
{
    options.Conventions.AddAreaPageRoute("Identity","/Pages/Account/Login","login");
    //options.Conventions.AddAreaPageRoute("Identity", "/Pages/Account/Register", "register");
});
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = "Bottom-Center";
});
builder.Services.AddTransient<IDbService, DbService>();
builder.Services.AddTransient<TastyDialogService>();
builder.Services.AddTransient<MealServiceMediator>();
builder.Services.AddScoped<LoggedUserService>();

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
    options.UseNpgsql(conString);
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
