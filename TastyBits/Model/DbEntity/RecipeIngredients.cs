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

        public string Quantity { get; set; }

        [ForeignKey("IngredientId")]
        public virtual Ingredients Ingredients { get; set; }
    }
}
