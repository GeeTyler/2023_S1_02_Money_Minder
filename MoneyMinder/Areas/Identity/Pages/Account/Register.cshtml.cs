using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoneyMinder.Model;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MoneyMinder.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly DatabaseContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DatabaseContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
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
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return LocalRedirect(ReturnUrl);
                }
                else 
                {
                    ModelState.AddModelError("Input.Password", "Incorrect Password. Try Again");
                }
            }

            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "First name is required.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required.")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }
    }
}
