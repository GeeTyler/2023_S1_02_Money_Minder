using MoneyMinder.Model;
using System.Collections.Generic;

namespace MoneyMinder.Data
{
    public interface IDataAccessService
    {
        List<Stock> GetStocks();
    }
}
