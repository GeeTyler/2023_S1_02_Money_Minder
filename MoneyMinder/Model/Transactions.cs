using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class Transactions
    {
        [Key]
        [Required]
        public int AccountNum { get; set; }

        [Required]
        public int AccountTransferredToOrFrom { get; set; }

        [Required]
        public DateTime DateandTime { get; set; }

        [Required]
        public double Balance { get; set; }

        [Required]
        public double TransactionAmount { get; set; }

        [Required]
        public string TransactionType { get; set; }
    }
}
