using System.ComponentModel.DataAnnotations;

namespace TastyBits.Areas.Identity.Pages.Account.Models
{
    public class UserRegister:UserInput
    {
        [Required]
        [DataType(DataType.Password)]
        [Compare("UserPassword",ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
