using Domain.Models;
using System.Collections.ObjectModel;

namespace Infrastructure.Data.Samples
{
    /// <summary>
    /// Dummy UserMeal object for running without database access or when no local or online database is available
    /// </summary>
    public class SampleMeal:UserMeal
    {
        public SampleMeal()
        {
            Name = "Test meal";
            MealId = 123;
            Ingredients = new ObservableCollection<UserMeal.Ingridient>();
            CookingTime = "23";
            Images = null;
            PrepTime = "2";
            ServingAmount = "1";
            Description = "My test example meal";
            Instructions = "Just test everything on a pan and ship it to production";
            //TimeOfDayMeal = new Collection<TimeOfDayMealT>().Add(TimeOfDayMealT.Breakfast);
        }
    }

}
