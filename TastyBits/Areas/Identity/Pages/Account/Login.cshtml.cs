using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using TastyBits.Areas.Identity.Pages.Account.Models;

namespace TastyBits.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        [BindProperty]
        public UserLogin? newUserInput { get; set; }

        public IActionResult OnGet()
        {
            //Log.Information("ulogiran: "+_signInManager.IsSignedIn(User).ToString());
            if (_signInManager.IsSignedIn(User)) { //if already singed in
                return LocalRedirect("/dashboard/home");
            }
            return Page();
        }

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            UserLogin newUserInput = new UserLogin();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid) {

                var result=await _signInManager.PasswordSignInAsync(newUserInput.UserEmail,
                    newUserInput.UserPassword,
                    isPersistent:false,
                    lockoutOnFailure:false);

                if (result.Succeeded) {
                    Log.Debug("login success");
                    return LocalRedirect("/dashboard/home");
                }else {
                    ModelState.AddModelError("unknown-err", "Incorrect login attempt");
                }

            }
            return Page();
        }
    }
}
