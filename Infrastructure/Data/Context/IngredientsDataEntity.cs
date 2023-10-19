using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.Context
{
    public class IngredientsDataEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public double CaloriesPer100Gram { get; set; }
        public double Fat_g { get; set; }
        public double Proteing_g { get; set; }
        public double Sodium_mg { get; set; }
        public double Potassium_mg { get; set; }
        public double Carbs_g { get; set; }
        public double Fiber_g { get; set; }
        public double Sugar_g { get; set; }
    }
}
