using System.ComponentModel.DataAnnotations;

namespace TastyBits.Areas.Identity.Pages.Account
{
    public class UserInput
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        //[Required]
        ////[Compare(UserPassword)]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }
    }
}
