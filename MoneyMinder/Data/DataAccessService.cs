using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MoneyMinder.Model;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Threading.Tasks;

namespace MoneyMinder.Data
{
    public class DataAccessService : IDataAccessService
    {
        private readonly DatabaseContext _db;

        public DataAccessService(DatabaseContext db)
        {
            _db = db;
        }

        public List<Stock> GetStocks(string SortBy, string SortDirection)
        {
            //Create an IQueryable instance for querying the Stock table
            IQueryable<Stock> query = _db.Stock;

            //Apply sorting based on the specified sortBy parameter
            switch (SortBy)
            {
                case "CompanyName":
                    //Sort by CompanyName in ascending order
                    query = query.OrderBy(s => s.CompanyName);
                    break;
                case "StockCode":
                    //Sort by StockCode in ascending order
                    query = query.OrderBy(s => s.StockCode);
                    break;
                case "MarketPrice":
                    //Sort by MarketPrice in ascending order
                    query = query.OrderBy(s => s.MarketPrice);
                    break;
                case "MarketCap":
                    //Sort by MarketCap in ascending order
                    query = query.OrderBy(s => s.MarketCap);
                    break;
                default:
                    //No sorting specified, use the default order
                    break;
            }

            //Apply sorting direction (ascending or descending)
            if (SortDirection == "Descending")
            {
                //Reverse the query to sort in descending order
                query = query.Reverse();
            }

            //Execute the query and retrieve the result as a list
            List<Stock> stocks = query.ToList();

            //Return the list of stocks
            return stocks;
        }

        public List<Stock> GetFilteredStocks(string searchText, string sortBy, string order)
        {
            //Create an IQueryable instance for querying the Stock table
            IQueryable<Stock> query = _db.Stock;

            //Apply filtering based on the searchText parameter
            if (!string.IsNullOrEmpty(searchText))
            {
                //Filter stocks where the CompanyName starts with the searchText (case-insensitive)
                query = query.Where(stock => stock.CompanyName.ToLower().StartsWith(searchText.ToLower()));
            }

            //Apply sorting based on the specified sortBy parameter
            switch (sortBy)
            {
                case "CompanyName":
                    //Sort by CompanyName in ascending order
                    query = query.OrderBy(s => s.CompanyName);
                    break;
                case "StockCode":
                    //Sort by StockCode in ascending order
                    query = query.OrderBy(s => s.StockCode);
                    break;
                case "MarketPrice":
                    //Sort by MarketPrice in ascending order
                    query = query.OrderBy(s => s.MarketPrice);
                    break;
                case "MarketCap":
                    //Sort by MarketCap in ascending order
                    query = query.OrderBy(s => s.MarketCap);
                    break;
                default:
                    //No sorting specified, use the default order
                    break;
            }

            //Apply sorting direction (ascending or descending)
            if (order == "Descending")
            {
                //Reverse the query to sort in descending order
                query = query.Reverse();
            }

            //Execute the query and retrieve the result as a list
            List<Stock> stocks = query.ToList();

            //Return the list of stocks
            return stocks;
        }

        public List<string> GetFavoriteStockCodesForUser(string userEmail)
        {
            //Retrieve the list of favourite stock codes for the given user email
            //by querying the 'Favourite' table in the database.
            //The result is a list of strings representing the stock codes.

            return _db.Favourite
                .Where(f => f.Email == userEmail) //Filter the records where the email matches the provided user email
                .Select(f => f.StockCode) //Select only the stock codes from the filtered records
                .ToList(); //Convert the result to a list and return it
        }

        public void FavouriteStock(string Code, string Email)
        {
            //Create a new instance of DbContextOptionsBuilder to configure the database context options.
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite("Data Source=MoneyMinder.db") //Specify the SQLite database source
                .Options;

            //Create a new instance of DatabaseContext using the configured options.
            using (var dbContext2 = new DatabaseContext(options))
            {
                //Check if the specified stock code and email combination already exists in the 'Favourite' table.
                var existingFavourite = _db.Favourite.FirstOrDefault(f => f.StockCode == Code && f.Email == Email);

                if (existingFavourite != null)
                {
                    //User already favourited the company, so delete it from the 'Favourite' table.
                    _db.Favourite.Remove(existingFavourite);
                }
                else
                {
                    //User hasn't favourited the company yet, so add it to the 'Favourite' table.
                    var fav = new Favourite()
                    {
                        StockCode = Code,
                        Email = Email
                    };

                    _db.Favourite.Add(fav);
                }

                //Save the changes made to the database.
                _db.SaveChanges();
            }
        }

        public bool IsFavorite(string StockCode, string Email)
        {
            //Create a new instance of DbContextOptionsBuilder to configure the database context options.
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite("Data Source=MoneyMinder.db") //Specify the SQLite database source
                .Options;

            //Create a new instance of DatabaseContext using the configured options.
            using (var dbContext2 = new DatabaseContext(options))
            {
                //Check if there is any entry in the 'Favourite' table that matches the specified stock code and email.
                return dbContext2.Favourite.Any(f => f.Email == Email && f.StockCode == StockCode);
            }
        }

        public List<MarketPriceData> GetMarketPrices()
        {
            //Retrieve all market price data entries from the 'MarketPriceData' table and convert them to a list.
            return _db.MarketPriceData.ToList();
        }

        public List<BankAccount> GetBankAccounts(string UserEmail)
        {
            //Check if the 'BankAccount' table is empty
            if (_db.BankAccount == null)
            {
                //If it is empty, return an empty list
                return new List<BankAccount>();
            }
            else
            {
                //Retrieve bank accounts from the 'BankAccount' table where the email matches the provided user email
                return _db.BankAccount.Where(account => account.Email.Contains(UserEmail)).ToList();
            }
        }

        public BankAccount GetBankAccount(int Account)
        {
            //Retrieve the first bank account from the 'BankAccount' table where the account number matches the provided account number
            return _db.BankAccount.FirstOrDefault(selected => selected.AccountNum == Account);
        }

        public void AddBankAccount(string UserEmail, string AccountName)
        {
            var random = new Random();
            bool accountNumberExists = true;
            int randomAccountNum = 0;

            //Generate a unique random account number
            while (accountNumberExists)
            {
                randomAccountNum = new Random().Next(100000000, 999999999);

                //Check if the generated account number already exists in the 'BankAccount' table
                var existingAccount = _db.BankAccount.FirstOrDefault(a => a.AccountNum == randomAccountNum);

                if (existingAccount == null)
                {
                    //Account number is unique, so exit the loop
                    accountNumberExists = false;
                }
            }

            var balance = random.NextDouble() * (10000 - 500) + 500;

            //Check if AccountName is not null or empty
            if (!string.IsNullOrEmpty(AccountName))
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    //Create a new BankAccount object with the generated account number and provided details
                    var account = new BankAccount()
                    {
                        AccountNum = randomAccountNum,
                        Email = UserEmail,
                        Name = AccountName,
                        Balance = balance,
                        Blocked = false,
                    };

                    //Add the new bank account to the 'BankAccount' table, save changes, and commit the transaction
                    _db.BankAccount.Add(account);
                    _db.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void ChangeBankAccountName(int AccountNum, string AccountName) 
        {
            //Find the bank account with the specified account number and change the account name of the found bank account
            _db.BankAccount.FirstOrDefault(account => account.AccountNum.Equals(AccountNum)).Name = AccountName;

            //Save the changes to the database
            _db.SaveChanges();
        }

        public void BlockBankAccount(int AccountNum, bool isBlocked) 
        {
            //Find the bank account with the specified account number and set the 'Blocked' property of the found bank account to the specified value
            _db.BankAccount.FirstOrDefault(account => account.AccountNum.Equals(AccountNum)).Blocked = isBlocked;

            //Save the changes to the database
            _db.SaveChanges();
        }

        public User GetUser(string UserEmail)
        {
            //Find the user with the specified email
            return _db.User.FirstOrDefault(user => user.Email.Equals(UserEmail));
        }

        public void ChangeFirstName(string Input, string UserEmail) 
        {
            //Find the user by email and change the first name of the user
            _db.User.FirstOrDefault(user => user.Email.Equals(UserEmail)).FirstName = Input;

            //Save the changes to the database
            _db.SaveChanges();
        }

        public void ChangeLastName(string Input, string UserEmail)
        {
            //Find the user by email and change the last name of the user
            _db.User.FirstOrDefault(user => user.Email.Equals(UserEmail)).LastName = Input;

            //Save the changes to the database
            _db.SaveChanges();
        }

        public List<Transactions> GetTransactions(int AccountNum) 
        {
            //Retrieve the transactions associated with the account number
            return _db.Transactions.Where(a => a.AccountNum == AccountNum).ToList();
        }

        string ChosenStockCode;

        public void SetChosenStock(string Code)
        {
            //Set the chosen stock code
            ChosenStockCode = Code;
        }

        public string GetChosenStock() 
        {
            //Get the chosen stock code
            return ChosenStockCode;
        }

        public void DeleteUsersInfo(string Email)
        {
            //Delete all the user's information from the database

            //Retrieve the bank accounts associated with the user
            var accounts = _db.BankAccount.Where(a => a.Email == Email).ToList();

            //Remove the bank accounts
            _db.BankAccount.RemoveRange(accounts);

            //Retrieve the favorites associated with the user
            var favs = _db.Favourite.Where(a => a.Email == Email).ToList();

            //Remove the favorites
            _db.Favourite.RemoveRange(favs);

            //Retrieve the user
            var users = _db.User.Where(a => a.Email == Email).ToList();

            //Remove the user
            _db.User.RemoveRange(users);

            //Save the changes to the database
            _db.SaveChanges();
        }

        public void DeleteBankAccount(int AccountNum)
        {
            //Delete the bank account with the specified account number

            //Retrieve the bank account
            var accounts = _db.BankAccount.Where(a => a.AccountNum == AccountNum).ToList();

            //Remove the bank account
            _db.BankAccount.RemoveRange(accounts);

            //Save the changes to the database
            _db.SaveChanges();
        }

        public void AddTransfer(int accountNum, int ToThisAccount, double Amount)
        {
            //Add a transfer transaction from one account to another

            //Check if the amount is valid
            if (Amount <= 0)
            {
                return;
            }

            //Generate a unique transaction number
            var random = new Random();
            bool TranscationNumberExists = true;
            int RandomTransactionNum = 0;
            int RandomTransactionNumTwo = 0;

            while (TranscationNumberExists)
            {
                RandomTransactionNum = new Random().Next(1, 9999999);
                RandomTransactionNumTwo = new Random().Next(1, 9999999);

                //Check if the transaction number already exists
                var existingAccount = _db.Transactions.FirstOrDefault(a => a.TrasactionNum == RandomTransactionNum
                || a.TrasactionNum == RandomTransactionNumTwo);

                if (existingAccount == null)
                {
                    TranscationNumberExists = false;
                }
            }

            //Retrieve the accounts involved in the transfer
            var fromAccount = _db.BankAccount.FirstOrDefault(a => a.AccountNum == accountNum);
            var toAccount = _db.BankAccount.FirstOrDefault(a => a.AccountNum == ToThisAccount);

            //Update the balances of the accounts
            fromAccount.Balance -= Amount;
            toAccount.Balance += Amount;

            //Add the transactions to the database
            using (var transaction = _db.Database.BeginTransaction())
            {
                var fromTransaction = new Transactions()
                {
                    TrasactionNum = RandomTransactionNum,
                    TransactionType = "Transfer",
                    AccountNum = accountNum,
                    TransactionAmount = -Amount,
                    DateandTime = DateTime.Now,
                };

                var toTransaction = new Transactions()
                {
                    TrasactionNum = RandomTransactionNumTwo,
                    TransactionType = "Receive Transfer",
                    AccountNum = ToThisAccount,
                    TransactionAmount = Amount,
                    DateandTime = DateTime.Now,
                };

                _db.Transactions.Add(fromTransaction);
                _db.Transactions.Add(toTransaction);
                _db.SaveChanges();
                transaction.Commit();
            }
        }

        public void GenerateRandomTransactions(int AccountNum)
        {
            //Generate random transactions for the specified account

            var random = new Random();
            bool TranscationNumberExists = true;
            int RandomTransactionNum = 0;

            while (TranscationNumberExists)
            {
                RandomTransactionNum = new Random().Next(1, 9999999);

                //Check if the transaction number already exists
                var existingAccount = _db.Transactions.FirstOrDefault(a => a.TrasactionNum == RandomTransactionNum);

                if (existingAccount == null)
                {
                    TranscationNumberExists = false;
                }
            }

            //Array of transaction types
            string[] types = { "Bills", "Eating Out", "Entertainment", "Shopping", "Supermarket" };

            //Generate a random transaction type
            int RandomType = random.Next(0, 5);

            //Generate a random date within a specific range
            DateTime RandomDay()
            {
                DateTime start = new DateTime(2023, 1, 1);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(random.Next(range));
            }

            //Generate a random transaction amount
            double randomTransactionAmount = random.NextDouble() * (500 - 1) + 1;

            //Add the transaction to the database
            using (var transaction = _db.Database.BeginTransaction())
            {
                var transactionMade = new Transactions()
                {
                    TrasactionNum = RandomTransactionNum,
                    AccountNum = AccountNum,
                    DateandTime = RandomDay(),
                    TransactionAmount = randomTransactionAmount * -1,
                    TransactionType = types[RandomType],
                };

                _db.Transactions.Add(transactionMade);
                _db.SaveChanges();
                transaction.Commit();
            }
        }

        public async Task ChangeUsersEmail(string currentEmail, string newEmail)
        {
            // Update the User table
            var user = await _db.User.FirstOrDefaultAsync(u => u.Email.Equals(currentEmail));
            if (user != null)
            {
                // Check if the new email already exists
                var existingUser = await _db.User.FirstOrDefaultAsync(u => u.Email.Equals(newEmail));
                if (existingUser != null)
                {
                    // Email already exists
                    return;
                }

                user.Email = newEmail;
            }

            // Update the Favourite table
            var favourites = _db.Favourite.Where(f => f.Email.Equals(currentEmail));
            foreach (var favourite in favourites)
            {
                favourite.Email = newEmail;
            }

            // Update the BankAccount table
            var bankAccounts = _db.BankAccount.Where(b => b.Email.Equals(currentEmail));
            foreach (var bankAccount in bankAccounts)
            {
                bankAccount.Email = newEmail;
            }

            // Save the changes
            await _db.SaveChangesAsync();
        }
    }
}
