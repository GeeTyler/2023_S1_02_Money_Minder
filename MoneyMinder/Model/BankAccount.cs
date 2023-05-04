using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class BankAccount
    {
        [Key, Required]
        public int AccountNum { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Balance { get; set; }
    }
}
