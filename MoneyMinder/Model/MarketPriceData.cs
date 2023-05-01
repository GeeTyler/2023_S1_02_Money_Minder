using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMinder.Model
{
    public class MarketPriceData
    {
        [Key]
        [Required]
        public string StockCode { get; set; }

        [Required]
        public float MarketPrice { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public int Volume { get; set; }

    }
}
