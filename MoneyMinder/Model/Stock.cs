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

        public string CompanyDescription { get; set; }

        [Required]
        public string MarketPrice { get; set; }

        [Required]
        public string MarketCap { get; set; }
    }
}
