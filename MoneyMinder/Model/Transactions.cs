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
        public int AccountTransferredTo { get; set; }

        [Required]
        public DateTime DateandTime { get; set; }

        [Required]
        public double Balance { get; set; }

        [Required]
        public double TransactionAmount { get; set; }
    }
}
