using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Balance { get; set; }
    }
}
