using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class User
    {
        [Key, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
