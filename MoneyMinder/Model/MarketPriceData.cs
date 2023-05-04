using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class MarketPriceData
    {
        [Key]
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Open { get; set; }
        [Required]
        public double High { get; set; }
        [Required]
        public double Low { get; set; }
        [Required]
        public double Close { get; set; }
        [Required]
        public double AdjClose { get; set; }
        [Required]
        public string StockCode { get; set; }
        [Required]
        public string Volume { get; set; }

    }
}
