using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace TastyBits.Model
{
    public class UserMeal
    {
        public int MealId { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<Ingridient> Ingredients { get; set; } =new ObservableCollection<Ingridient>();
        public List<string> Images { get; set; } = new();
        public string CookingTime { get; set; }
        public string PrepTime { get; set; }
        public string ServingAmount { get; set; }   
        public string Calories { get; set; }
        public class Ingridient
        {
            public int Num { get; set; }
            public string Name { get; set; }
            public string Quantity { get; set; }
            public string Calories { get; set; }
        }
        /// <summary>
        /// Dummy method
        /// </summary>
        /// <returns></returns>
        public int GetCalories()
        {

            return 100;
        }
    }

}
