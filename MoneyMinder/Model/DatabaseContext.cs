using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MoneyMinder.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options)
        {
            
        }

        public DbSet<User> User { get; set; }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<BankAccount> BankAccount { get; set; }

    }
}
