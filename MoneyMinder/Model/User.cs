using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class User : IdentityUser
    {
        [Key, EmailAddress]
        public override string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
