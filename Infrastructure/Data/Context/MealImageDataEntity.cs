using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.Context
{
    public class MealImageDataEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }
        public int MealId { get; set; }

        [ForeignKey("MealId")]
        public virtual MealsDataEntity Meals { get; set; }
        public string? ImageData { get; set; }
        public string ImageName { get; set; }=string.Empty;
        public DateTime ValidUntil { get; set; }
    }
}
