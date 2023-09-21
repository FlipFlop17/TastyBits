using System.Collections.ObjectModel;

namespace TastyBits.Model
{
    public class UserMeal
    {
        public string MealId { get; set; }
        public string UserId { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<Ingridient> Ingredients { get; set; } =new ObservableCollection<Ingridient>();
        public List<string> Images { get; set; } = new();
        public string CookingTime { get; set; }
        public string PrepTime { get; set; }
        public string ServingAmount { get; set; }   

        public class Ingridient
        {
            public int Num { get; set; }
            public string Name { get; set; }
            public string Quantity { get; set; }
        }
    }

}
