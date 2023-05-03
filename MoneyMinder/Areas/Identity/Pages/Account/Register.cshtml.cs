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
                var identity = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(identity, Input.Password);

                var registered = new User { Email = Input.Email, FirstName = Input.FirstName,
                    LastName = Input.LastName };
                _db.User.Add(registered);
                _db.SaveChanges();

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(identity, isPersistent: false);
                    return LocalRedirect(ReturnUrl);
                }
            }
            return Page();
        }

        public class InputModel
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }
    }
}
