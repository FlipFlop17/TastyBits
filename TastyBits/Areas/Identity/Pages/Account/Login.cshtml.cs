using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace TastyBits.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly NavigationManager _nav;
        private readonly SignInManager<IdentityUser> _signInManager;

        [BindProperty]
        public UserInput? newUserInput { get; set; }

        public IActionResult OnGet()
        {
            //Log.Information("ulogiran: "+_signInManager.IsSignedIn(User).ToString());
            if (_signInManager.IsSignedIn(User)) { //if already singed in
                return LocalRedirect("/dashboard/home");
            }
            return Page();
        }

        public LoginModel(SignInManager<IdentityUser> signInManager, NavigationManager nav)
        {
            _nav = nav;
            _signInManager = signInManager;
            UserInput newUserInput = new UserInput();
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
                }

            }
            return Page();
        }
    }
}
