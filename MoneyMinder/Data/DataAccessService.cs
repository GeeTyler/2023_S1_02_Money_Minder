using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MoneyMinder.Model;
using System;
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

        public void AddBankAccount(string UserEmail, string AccountName)
        {
            var random = new Random();
            bool accountNumberExists = true;
            int randomAccountNum = 0;

            while (accountNumberExists)
            {
                randomAccountNum = new Random().Next(100000000, 999999999);

                var existingAccount = _db.BankAccount.FirstOrDefault(a => a.AccountNum == randomAccountNum);

                if (existingAccount == null)
                {
                    accountNumberExists = false;
                }
            }

            var balance = random.NextDouble() * (10000 - 500) + 500;

            if (!AccountName.Equals(null))
            {
                using (var transaction = _db.Database.BeginTransaction())
                {

                    var account = new BankAccount()
                    {
                        AccountNum = randomAccountNum,
                        Email = UserEmail,
                        Name = AccountName,
                        Balance = balance,
                        blocked = false,
                    };

                    _db.BankAccount.Add(account);
                    _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.BankAccount ON;");
                    _db.SaveChanges();
                    _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.BankAccount OFF;");
                    transaction.Commit();
                }
            }
        }

        public void ChangeBankAccountName(int AccountNum, string AccountName) 
        {
            _db.BankAccount.FirstOrDefault(account => account.AccountNum.Equals(AccountNum)).Name = AccountName;
            _db.SaveChanges();
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

        public void DeleteUsersInfo(string Email)
        {
            var accounts = _db.BankAccount.Where(a => a.Email == Email).ToList();

            _db.BankAccount.RemoveRange(accounts);

            var users = _db.User.Where(a => a.Email == Email).ToList();

            _db.User.RemoveRange(users);

            _db.SaveChanges();
        }

        public void DeleteBankAccount(int AccountNum)
        {
            var accounts = _db.BankAccount.Where(a => a.AccountNum == AccountNum).ToList();

            _db.BankAccount.RemoveRange(accounts);

            _db.SaveChanges();
        }
    }
}
