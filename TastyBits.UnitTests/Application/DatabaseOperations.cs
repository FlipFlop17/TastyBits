using Application.Cache;
using Application.Features.Meals.Commands.CreateMeal;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Xunit.Abstractions;

namespace TastyBits.UnitTests.Application
{
    public class DatabaseOperations
    {
        private readonly IConfiguration _config;
        private readonly ITestOutputHelper _outputLog;
        private readonly ServiceProvider _serviceGetter;
        private readonly IServiceCollection _services;
        public DatabaseOperations(ITestOutputHelper output)
        {
            _outputLog = output;
            _services = new SetupDI().Services();
            _serviceGetter = _services.BuildServiceProvider();
        }

        [Fact]
        public async Task Test_ShouldInsertMealToDatabase()
        {
            CreateMealUseCase insertMealFeature; 
            UserMeal myMeal = GetUserMealTestData();

            //TaskResult insertResult = await insertMealFeature.InsertNewMealAsync(myMeal);
            //_outputLog.WriteLine($"my output {insertResult.ErrorDesc} ");
            //Assert.True(insertResult.HasError == false);
        }

        [Fact]
        public async Task Test_ShouldUpdateMealAtDatabase()
        {
            var mealRepo = _serviceGetter.GetRequiredService<ICache>();
            //var updateMealCase = new UpdateMealUseCase(mealRepo);

            //List<UserMeal> sviMealovi = await mealRepo.GetUserMealById("6ec7c9f6-92a9-4240-88a7-540a9acc6719");
            //Assert.True(sviMealovi.Any(), "No meal by this user ID");

            //UserMeal mealForEdit = sviMealovi.FirstOrDefault();
            //_outputLog.WriteLine($"ime prije azuriranja {mealForEdit.Name}");
            //mealForEdit.Name = "mijenjam";
            //mealForEdit.Ingredients.FirstOrDefault().Quantity=58;
            //mealForEdit.Ingredients.Add(new UserMeal.Ingridient()
            //{
            //    Num = 2,
            //    Name = "Eggs",
            //    Quantity = 1,
            //    CaloriesPer100g = 70,
            //    QuantityUnit = QuantityUnit.Pieces
            //});
            //TaskResult insertResult = await updateMealCase.UpdateMeal(mealForEdit);
            //Assert.True(insertResult.HasError == false, insertResult.ErrorDesc);
        }

        private UserMeal GetUserMealTestData()
        {
            return new UserMeal
            {
                Name = "Jaja sa spinatom",
                UserId = "6ec7c9f6-92a9-4240-88a7-540a9acc6719",
                Description = "Test Description",
                Ingredients = new ObservableCollection<UserMeal.Ingridient>
                {
                    new UserMeal.Ingridient
                    {
                        Num = 1,
                        Name = "Potato",
                        Quantity = 500,
                        QuantityUnit = QuantityUnit.Grams
                    },
                    new UserMeal.Ingridient
                    {
                        Num = 2,
                        Name = "Tomato",
                        Quantity = 1,
                        QuantityUnit = QuantityUnit.Pieces
                    }
                },
                //Images = new List<string> { "testImage.jpg" },
                CookingTime = "30 minutes",
                PrepTime = "15 minutes",
                ServingAmount = "2 servings",
                Instructions = "Test Instructions",
                TimeOfDayMeal = new Collection<TimeOfDayMealT> { TimeOfDayMealT.Breakfast }
            };
        }
    }
}