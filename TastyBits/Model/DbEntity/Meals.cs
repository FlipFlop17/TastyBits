using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyBits.Model.Dto
{
    public class Meals
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser Identity { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PrepTime { get; set; }

        public int CookingTime { get; set; }

        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime? ValidUntil { get; set; }

    }
}
