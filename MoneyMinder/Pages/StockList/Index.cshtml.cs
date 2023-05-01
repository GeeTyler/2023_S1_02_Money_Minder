using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoneyMinder.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMinder.Pages.StockList
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _db;
        
        public IndexModel (DatabaseContext db)
        {
            _db = db;
        }

        public IEnumerable<Stock> Stocks { get; set; }
        public async Task OnGet()
        {
            Stocks = await _db.Stock.ToListAsync();
        }
    }
}
