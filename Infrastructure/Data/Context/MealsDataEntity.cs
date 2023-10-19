using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.Context
{
    public class MealsDataEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser Identity { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? PrepTime { get; set; }

        public string? CookingTime { get; set; }

        public string? ServingsAmount { get; set; }
        public bool IsDesert { get; set; }
        public bool IsSnack { get; set; }
        public bool IsLunch { get; set; }
        public bool IsBreakfast { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsDinner { get; set; }

        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime? ValidUntil { get; set; }
        public string? Instructions { get; set; }
        public virtual ICollection<MealImageDataEntity>? RecipeImages { get; set; }
        public virtual ICollection<MealIngredientsDataEntity>? RecipeIngridients { get; set; }
    }
}
