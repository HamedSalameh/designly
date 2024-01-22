using Accounts.Domain;
using Accounts.Infrastructure.Persistance.Configuration;
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
            modelBuilder.ApplyConfiguration(new TeamMemberDbContextConfiguration());
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        private void OnBeforeSaving()
        {
            var enties = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in enties)
            {
                if (entry.Entity is Entity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.ModifiedAt = utcNow;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = utcNow;
                            trackable.ModifiedAt = utcNow;
                            break;
                    }
                }
            }
        }
    }
}
