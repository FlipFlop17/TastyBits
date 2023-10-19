using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class GetUserMealsById
    {
        private readonly IMealsRepository _mealsRepo;

        public GetUserMealsById(IMealsRepository mealsRepo)
        {
            _mealsRepo = mealsRepo;
        }

        public async Task<List<UserMeal>> GetUserMeals(string userId)
        {
            return await _mealsRepo.GetUserMealById(userId);
        }
    }
}
