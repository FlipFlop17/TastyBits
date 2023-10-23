using Application.Interfaces;
using Application.Services;
using Application.UseCases;
using Domain.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
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

            var createMeal = new CreateMealUseCase(_serviceGetter.GetRequiredService<ICache>(),
                _serviceGetter.GetRequiredService<CalorieApiService>());
            UserMeal myMeal = GetUserMealTestData();

            TaskResult insertResult=await createMeal.InsertNewMealAsync(myMeal);
            _outputLog.WriteLine($"my output {insertResult.ErrorDesc} ");
            Assert.True(insertResult.HasError==false);
        }

        [Fact]
        public async Task Test_ShouldUpdateMealAtDatabase()
        {
            var mealRepo = _serviceGetter.GetRequiredService<ICache>();
            var updateMealCase = new UpdateMealUseCase(mealRepo);

            List<UserMeal> sviMealovi = await mealRepo.GetUserMealById("6ec7c9f6-92a9-4240-88a7-540a9acc6719");
            Assert.True(sviMealovi.Any(),"No meal by this user ID");

            UserMeal mealForEdit=sviMealovi.FirstOrDefault();
            //_outputLog.WriteLine($"ime prije azuriranja {mealForEdit.Name}");
            //mealForEdit.Name = "mijenjam";
            //mealForEdit.Ingredients.FirstOrDefault().Quantity=58;
            mealForEdit.Ingredients.Add(new UserMeal.Ingridient()
            {
                Num = 2,
                Name = "Eggs",
                Quantity = 1,
                CaloriesPer100g = 70,
                QuantityUnit = QuantityUnit.Pieces
            });
            TaskResult insertResult=await updateMealCase.UpdateMeal(mealForEdit);
            Assert.True(insertResult.HasError ==false,insertResult.ErrorDesc);
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
                TimeOfDayMeal = new Collection<TimeOfDayMeal> { TimeOfDayMeal.Breakfast }
            };
        }
    }
}
