using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System.Diagnostics;
using TastyBits.Areas.Identity.Pages.Account.Models;
using TastyBits.Services;

namespace TastyBits.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly LoggedUserService _loggedUserService;

        [BindProperty]
        public UserLogin? newUserInput { get; set; }

        public async Task<IActionResult> OnGet()
        {
            //Log.Information("ulogiran: "+_signInManager.IsSignedIn(User).ToString());
            if (_signInManager.IsSignedIn(User)) { //if already singed in
                var identityUser = await _signInManager.UserManager.GetUserAsync(User);
                if (identityUser.Email.Contains("demo@"))
                {
                    await _signInManager.UserManager.AddToRoleAsync(identityUser, "Demo");
                }
                else
                {
                    await _signInManager.UserManager.AddToRoleAsync(identityUser, "Standard");
                }
                return LocalRedirect("/dashboard/home");
            }
            return Page();
        }

        public LoginModel(SignInManager<IdentityUser> signInManager,LoggedUserService loggedUserService)
        {
            _signInManager = signInManager;
            _loggedUserService = loggedUserService;
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
                    var identityUser = await _signInManager.UserManager.GetUserAsync(User);
                    if (newUserInput.UserEmail.Contains("demo@"))
                    {
                        await _signInManager.UserManager.AddToRoleAsync(identityUser,"Demo");
                    }else
                    {
                        await _signInManager.UserManager.AddToRoleAsync(identityUser, "Standard");
                    }
                    return LocalRedirect("/dashboard/home");
                }else {
                    ModelState.AddModelError("unknown-err", "Incorrect login attempt");
                }

            }
            return Page();
        }

        private async Task StoreLoggedUser()
        {
            await _loggedUserService.GetUserDataAsync();
            Log.Information("user stored");
        }
    }
}
