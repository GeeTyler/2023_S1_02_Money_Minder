using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MoneyMinder.Model
{
    public class Stock
    {
        [Key]
        public string StockCode { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public double MarketPrice { get; set; }

        [Required]
        public double MarketCap { get; set; }
    }
}
