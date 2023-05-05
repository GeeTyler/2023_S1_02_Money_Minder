using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class BankAccount
    {
        [Key, Required]
        [Range(1, 99999999999, ErrorMessage = "Account number must be between {1} and {99999999999}.")]
        public int AccountNum { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public bool IsLocked { get; set; }
    }
}
