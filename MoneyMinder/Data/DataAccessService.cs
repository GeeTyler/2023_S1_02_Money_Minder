using Microsoft.AspNetCore.Components;
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

        public List<Stock> GetFilteredStocks(string SearchText)
        {
            return _db.Stock.Where(stock => stock.CompanyName.ToLower().StartsWith(SearchText.ToLower())).ToList();
        }

        public List<MarketPriceData> GetMarketPrices() 
        {
            return _db.MarketPriceData.ToList();
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
