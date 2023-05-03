using MoneyMinder.Model;
using System.Collections.Generic;
using System.Linq;

namespace MoneyMinder.Data
{
    public class DataAccessService : IDataAccessService
    {
        private readonly DatabaseContext _db;

        public DataAccessService(DatabaseContext db)
        {
            _db = db;
        }

        public List<Stock> GetStocks() 
        {
            return _db.Stock.ToList();
        }

        string ChosenStockCode;

        public void SetChosenStock(string Code)
        {
            ChosenStockCode = Code;
        }

        public string GetChosenStock() 
        {
            return ChosenStockCode;
        }
    }
}
