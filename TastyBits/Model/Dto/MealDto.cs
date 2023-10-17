namespace TastyBits.Model.Dto
{
    /// <summary>
    /// Represents the joined data from the MealsTable
    /// </summary>
    public class MealDto
    {
        public int MealId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserMeal.Ingridient> Ingredients { get; set; }
        public string CookingTime { get; set; }
        public string PrepTime { get; set; }
        public List<string> Images { get; set; }=new List<string>();
        public string ServingsAmount { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsDesert { get; set; }
        public bool IsSnack { get; set; }
        public bool IsLunch { get; set; }
        public bool IsBreakfast { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsDinner { get; set; }
        public string Instrunctions { get; set; }
    }
}
