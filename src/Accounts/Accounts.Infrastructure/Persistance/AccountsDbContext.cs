using Accounts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Persistance
{
    public class AccountsDbContext : DbContext
    {
        public AccountsDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountDbContextConfiguration());
            modelBuilder.ApplyConfiguration(new TeamDbContextConfiguration());
            modelBuilder.ApplyConfiguration(new UserDbContextConfiguration());
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
