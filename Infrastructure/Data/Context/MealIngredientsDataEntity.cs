using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.Context
{
    public class MealIngredientsDataEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IngredientId { get; set; }

        public int MealId { get; set; }
        public double Quantity { get; set; }
        public string? QuantityUnit { get; set; }

        [ForeignKey("IngredientId")]
        public virtual IngredientsDataEntity Ingredients { get; set; }

        [ForeignKey("MealId")]
        public virtual MealsDataEntity Meals { get; set; }
    }
}
