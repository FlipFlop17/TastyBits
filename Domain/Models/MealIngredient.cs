namespace Domain.Models
{
    public class MealIngredient
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public int MealId { get; set; }
        public double Quantity { get; set; }
        public string QuantityUnit { get; set; }
    }
}
