using Application.Interfaces;
using Domain.Models;
using Domain.ReturnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class CachingService : ICache
    {
        public Task<TaskResult> AddMeal(UserMeal newMeal)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> DeleteMeal(int mealId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserMeal>> GetUserMealById(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> UpdateMeal(UserMeal newUpdatedMeal)
        {
            throw new NotImplementedException();
        }
    }
}
