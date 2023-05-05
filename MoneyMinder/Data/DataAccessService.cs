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

        public List<BankAccount> GetBankAccounts(string UserEmail) 
        {
            if (_db.BankAccount == null) 
            {
                return new List<BankAccount>();
            }
            else
            {
                return _db.BankAccount.Where(account => account.Email.Contains(UserEmail)).ToList();
            }
        }

        public User GetUser(string UserEmail)
        {
            return _db.User.FirstOrDefault(user => user.Email.Equals(UserEmail));
        }

        public void ChangeFirstName(string Input, string UserEmail) 
        {
            _db.User.FirstOrDefault(user => user.Email.Equals(UserEmail)).FirstName = Input;
            _db.SaveChanges();
        }

        public void ChangeLastName(string Input, string UserEmail)
        {
            _db.User.FirstOrDefault(user => user.Email.Equals(UserEmail)).LastName = Input;
            _db.SaveChanges();
        }

        public List<Transactions> GetTransactions() 
        {
            if (_db.BankAccount == null)
            {
                return new List<Transactions>();
            }
            else
            {
                return _db.Transactions.ToList();
            }
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
