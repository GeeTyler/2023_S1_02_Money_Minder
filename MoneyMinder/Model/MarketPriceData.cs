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
        public string Open { get; set; }
        [Required]
        public string High { get; set; }
        [Required]
        public string Low { get; set; }
        [Required]
        public string Close { get; set; }
        [Required]
        public string AdjClose { get; set; }
        [Required]
        public string StockCode { get; set; }
        [Required]
        public string Volume { get; set; }

    }
}
