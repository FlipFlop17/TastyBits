using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TastyBits.Model.Dto
{
    public class RecipeIngredients
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IngredientId { get; set; }
        
        public int MealId { get; set; }
        public double Quantity { get;set; }
        public string? QuantityUnit { get;set; }

        [ForeignKey("IngredientId")]
        public virtual Ingredients Ingredients { get; set; }

        [ForeignKey("MealId")]
        public virtual Meals Meals { get; set; }
    }
}
