using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System.Configuration;
using System.Diagnostics;
using TastyBits.Areas.Identity.Pages.Account.Models;

namespace TastyBits.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public UserLogin? newUserInput { get; set; }

       
        public async Task<IActionResult> OnGet(bool? IsDemo)
        {
            if(IsDemo.HasValue) {
                var demoUser = await _signInManager.UserManager.FindByEmailAsync("tastydemo@demo.com");
                await _signInManager.SignInAsync(demoUser,false);
                return LocalRedirect("/dashboard/home");
            }
            if (_signInManager.IsSignedIn(User)) { //if already singed in
                return LocalRedirect("/dashboard/home");
            }
            return Page();
        }

        public LoginModel(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
