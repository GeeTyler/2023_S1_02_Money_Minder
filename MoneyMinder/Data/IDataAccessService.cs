using MoneyMinder.Model;
using System.Collections.Generic;

namespace MoneyMinder.Data
{
    public interface IDataAccessService
    {
        List<Stock> GetStocks(string sortBy, string sortDirection);

        List<Stock> GetFilteredStocks(string SearchText, string SortBy, string Order);

        void FavouriteStock(string Code, string Email);

        List<string> GetFavoriteStockCodesForUser(string userEmail);

        bool IsFavorite(string stockCode, string Email);

        List<MarketPriceData> GetMarketPrices();

        List<Transactions> GetTransactions(int AccountNum);

        User GetUser(string UserEmail);

        List<BankAccount> GetBankAccounts(string UserEmail);

        void AddBankAccount(string UserEmail, string AccountName);

        void ChangeBankAccountName(int AccountNum, string AccountName);

        void SetChosenStock(string Code);

        string GetChosenStock();

        void ChangeLastName(string Input, string UserEmail);

        void ChangeFirstName(string Input, string UserEmail);

        void DeleteUsersInfo(string Email);

        void DeleteBankAccount(int AccountNum);

        BankAccount GetBankAccount(int Account);

        void BlockBankAccount(int AccountNum, bool isBlocked);

        void AddTransfer(int accountNum, int ToThisAccount, double Amount);

        void GenerateRandomTransactions(int AccountNum);

        void ChangeUsersEmail(string currentEmail, string newEmail);
    }
}
