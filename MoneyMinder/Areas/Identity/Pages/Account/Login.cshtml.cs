using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MoneyMinder.Areas.Identity.Pages.Account.RegisterModel;

namespace MoneyMinder.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet()
        {
            ReturnUrl = Url.Content("/Home");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("/Home");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Input.Email", "Email doesn't exist.");
                }
                else
                {
                    var passwordValid = await _userManager.CheckPasswordAsync(user, Input.Password);
                    if (passwordValid)
                    {
                        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password,
                            isPersistent: false, lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            return LocalRedirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Input.Password", "Incorrect Password.");
                    }
                }
            }
            return Page();
        }


        public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
