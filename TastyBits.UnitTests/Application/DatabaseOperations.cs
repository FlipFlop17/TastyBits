using Application.UseCases;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.ObjectModel;
using System.Net;
using Xunit.Abstractions;

namespace TastyBits.UnitTests.Application
{

    public class DatabaseOperations
    {
        private readonly ITestOutputHelper _outputLog;
        private readonly MealsRepository _mealsRepository;
        private readonly IServiceCollection _services;
        private readonly ServiceProvider _serviceGetter;

        public DatabaseOperations(ITestOutputHelper output)
        {
            _outputLog = output;
            _services = new SetupDI().Services();
            _serviceGetter=_services.BuildServiceProvider();
        }


        [Fact]
        public async Task Test_ShouldInsertMealToDatabase()
        {

            var createMeal = new CreateMealUseCase(_serviceGetter.GetRequiredService<IMealsRepository>());
            UserMeal myMeal = GetUserMealTestData();

            TaskResult insertResult=await createMeal.InsertNewMealAsync(myMeal);
            _outputLog.WriteLine($"my output {insertResult.ErrorDesc}");
            Assert.True(insertResult.HasError==false);
        }

        [Fact]
        public async Task Test_ShouldUpdateMealAtDatabase()
        {
            IMealsRepository mealRepo = _serviceGetter.GetRequiredService<IMealsRepository>();
            var updateMealCase = new UpdateMealUseCase(mealRepo);

            List<UserMeal> sviMealovi = await mealRepo.GetUserMealById("6ec7c9f6-92a9-4240-88a7-540a9acc6719");

            UserMeal mealForEdit=sviMealovi.FirstOrDefault();
            //_outputLog.WriteLine($"ime prije azuriranja {mealForEdit.Name}");
            
            mealForEdit.Name = "sdfsdf";
            TaskResult insertResult=await updateMealCase.UpdateMeal(mealForEdit);
            Assert.True(insertResult.HasError ==false,insertResult.ErrorDesc);
        }


        private UserMeal GetUserMealTestData()
        {
            return new UserMeal
            {
                Name = "azuirani meal",
                UserId = "6ec7c9f6-92a9-4240-88a7-540a9acc6719",
                Description = "Test Description",
                Ingredients = new ObservableCollection<UserMeal.Ingridient>
                {
                    new UserMeal.Ingridient
                    {
                        Id = 1,
                        Num = 1,
                        Name = "Test Ingredient",
                        Quantity = 100,
                        CaloriesPer100g = 50,
                        QuantityUnit = QuantityUnit.Grams
                    }
                },
                Images = new List<string> { "testImage.jpg" },
                CookingTime = "30 minutes",
                PrepTime = "15 minutes",
                ServingAmount = "2 servings",
                Instructions = "Test Instructions",
                TimeOfDayMeal = new Collection<TimeOfDayMeal> { TimeOfDayMeal.Breakfast }
            };
        }
    }
}
