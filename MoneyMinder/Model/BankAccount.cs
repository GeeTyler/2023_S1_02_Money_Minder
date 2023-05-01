using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class BankAccount
    {
        [Key, EmailAddress]
        public string Email { get; set; }
        [Required]
        public int AccountNum { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Balance { get; set; }
    }
}
