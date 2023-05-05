using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MoneyMinder.Model
{
    public class BankAccount
    {
        [Key]
        public int AccountNum { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public bool blocked { get; set; }
    }
}
