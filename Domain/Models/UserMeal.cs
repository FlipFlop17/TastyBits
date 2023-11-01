using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public enum QuantityUnit
    {
        Grams,Pieces
    }
    public enum TimeOfDayMealT
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
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Meal name is required")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [NotEmptyCollection(ErrorMessage ="Must have some ingredients")]
        public ObservableCollection<Ingridient> Ingredients { get; set; } =new ObservableCollection<Ingridient>();

        public List<MealImage> Images { get; set; } = new();

        public string? CookingTime { get; set; }

        public string? PrepTime { get; set; }
        public string? ServingAmount { get; set; }

        private string? _totalCalories;
        public string TotalCalories
        {
            get => GetTotalMealCalories(this.Ingredients).ToString();
        }

        [Required]
        public Collection<TimeOfDayMealT> TimeOfDayMeal { get; set; }

        public class Ingridient
        {
            public int Id { get; set; }
            public int IngredientId { get; set; }
            /// <summary>
            /// Order number for sorting
            /// </summary>
            public int Num { get; set; }
            public string Name { get; set; } = string.Empty;
            public double Quantity { get; set; }
            public double CaloriesPer100g { get; set; }
            public QuantityUnit QuantityUnit { get; set; }
            public bool IsDeleted { get; set; }
            public string? UnicodeChars { get; set; }
        }
        /// <summary>
        /// Instructions on how to prepare a meal
        /// </summary>
        public string? Instructions { get; set; }   
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
                double quantity=0;
                if (item.QuantityUnit == QuantityUnit.Grams) {
                    perG = item.CaloriesPer100g / 100;
                    quantity = item.Quantity;
                }else {
                    perG = item.CaloriesPer100g / 100;
                    quantity= item.Quantity*100; //per piece 1piece=100g
                }
                total +=(quantity * perG);
             }
            return Math.Round(total,2);
        }


    }

    public class NotEmptyCollection:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }

}
