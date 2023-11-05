using Domain.Models;
using System.Collections.ObjectModel;

namespace TastyBits.Services
{
    public class DemoDataStore
    {
        public ObservableCollection<UserMeal> DemoMeals { get; set; } = new ObservableCollection<UserMeal>();
    }
}
