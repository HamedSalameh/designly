using Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Persistance.Configuration
{
    public class AccountDbContextConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Map table
            builder.ToTable("accounts");

            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(Consts.AccountNameMaxLength);

            builder.Property(x => x.OwnerId)
                .HasColumnName("account_owner_id")
                .IsRequired(false);

            builder.HasOne(Account => Account.Owner)
                .WithOne(User => User.Account)
                .HasForeignKey<Account>(Account => Account.OwnerId)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.HasMany(acc => acc.Teams)
                .WithOne(team => team.Account)
                .HasForeignKey(team => team.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}