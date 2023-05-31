using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MoneyMinder.Model
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options)
        {
            
        }

        public DbSet<User> User { get; set; }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<BankAccount> BankAccount { get; set; }

        public DbSet<Transactions> Transactions { get; set; }

        public DbSet<MarketPriceData> MarketPriceData { get; set; }

        public DbSet<Favourite> Favourite { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=MoneyMinder.db");
            }
        }

    }
}
