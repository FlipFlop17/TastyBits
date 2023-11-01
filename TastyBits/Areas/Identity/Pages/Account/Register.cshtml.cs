using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System.Diagnostics;
using TastyBits.Areas.Identity.Pages.Account.Models;

namespace TastyBits.Areas.Identity.Pages.Account
{

    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly NavigationManager _nav;
        private readonly SignInManager<IdentityUser> _signInManager;
        [BindProperty]
        public List<IdentityError> ResultErrors { get; set; }

        [BindProperty]
        public UserRegister? newUserInput {get;set;}

        public string returnUrl { get;set;}

        public RegisterModel(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager,NavigationManager nav)
        {
            _userManager = userManager;
            _nav = nav;
            _signInManager = signInManager;
            UserRegister newUserInput = new UserRegister();
            ResultErrors = new List<IdentityError>();
        }
        public void OnGet()
        {
            returnUrl = "/";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid) {

                IdentityUser newUser = new IdentityUser()
                {
                    UserName=newUserInput.UserEmail,
                    Email=newUserInput.UserEmail,
                };
                var result = await _userManager.CreateAsync(newUser, newUserInput.UserPassword);
                
                ResultErrors=result.Errors.ToList();
                if (result.Succeeded) {
                    await _signInManager.SignInAsync(newUser,isPersistent: false);
                    Log.Debug("registration success!"); 
                    //_nav.NavigateTo("/dashboard/home");
                    return LocalRedirect("/Identity/Account/login");
                }else {
                    Debug.Print($"registration error: {result.Errors.First().Description}");
                    ModelState.AddModelError("unknown-err", result.Errors.First().Description);
                }

            }
            return Page();
        }
    }
}
