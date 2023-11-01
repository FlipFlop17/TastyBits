using System.ComponentModel.DataAnnotations;

namespace TastyBits.Areas.Identity.Pages.Account.Models
{
    public class UserInput
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
