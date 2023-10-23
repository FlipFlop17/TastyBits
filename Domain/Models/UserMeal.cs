using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public enum QuantityUnit
    {
        Grams,Pieces
    }
    public enum TimeOfDayMeal
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack,
        Dessert
    }
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
        private string _totalCalories;
        public string TotalCalories
        {
            get => GetTotalMealCalories(this.Ingredients).ToString();
        }

        [Required]
        public Collection<TimeOfDayMeal> TimeOfDayMeal { get; set; }

        public class Ingridient
        {
            public int Id { get; set; }
            public int IngredientId { get; set; }
            /// <summary>
            /// Order number for sorting
            /// </summary>
            public int Num { get; set; }
            public string Name { get; set; }
            public double Quantity { get; set; }
            public double CaloriesPer100g { get; set; }
            public QuantityUnit QuantityUnit { get; set; }
            public bool IsDeleted { get; set; }
        }
        /// <summary>
        /// Instructions on how to prepare a meal
        /// </summary>
        public string Instructions { get; set; }   
        /// <summary>
        /// Gets the total calories in passed ingridients
        /// </summary>
        /// <returns></returns>
        private double GetTotalMealCalories(IEnumerable<Ingridient> ingridients)
        {
            double total = 0;
            foreach (var item in ingridients) {
                //exmpl. onion has 44.7 calories per 100g
                double perG=0;
                if (item.QuantityUnit == QuantityUnit.Grams) {
                    perG = item.CaloriesPer100g / 100;
                }else {
                    //per piece

                }
                total +=( item.Quantity * perG);
             }
            return Math.Round(total,2);
        }
    }

}
