using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TastyBits.Model.Dto
{
    public class RecipeImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }
        public int MealId { get; set; }

        [ForeignKey("MealId")]
        public virtual Meals Meals { get; set; }
        public string? ImageData { get; set; }
    }
}
