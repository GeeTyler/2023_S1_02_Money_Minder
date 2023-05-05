using MoneyMinder.Model;
using System.Collections.Generic;

namespace MoneyMinder.Data
{
    public interface IDataAccessService
    {
        List<Stock> GetStocks();

        List<Stock> GetFilteredStocks(string SearchText);

        List<MarketPriceData> GetMarketPrices();

        List<Transactions> GetTransactions();

        User GetUser(string UserEmail);

        List<BankAccount> GetBankAccounts(string UserEmail);

        void SetChosenStock(string Code);

        string GetChosenStock();

        void ChangeLastName(string Input, string UserEmail);

        void ChangeFirstName(string Input, string UserEmail);

    }
}
