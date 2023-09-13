using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TastyBits.Model
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Ingridients { get; set; }
        public DateTime ValidFrom { get; set; }= DateTime.Now;
        public DateTime? ValidUntil { get; set; }

        public Recipe() { }

    }
}
