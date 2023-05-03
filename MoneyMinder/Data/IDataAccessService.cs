using MoneyMinder.Model;
using System.Collections.Generic;

namespace MoneyMinder.Data
{
    public interface IDataAccessService
    {
        List<Stock> GetStocks();

        public void SetChosenStock(string Code);

        public string GetChosenStock();
    }
}
