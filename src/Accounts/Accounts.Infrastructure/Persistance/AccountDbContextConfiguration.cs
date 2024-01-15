using Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Persistance
{
    public class AccountDbContextConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");

            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id")
                .IsRequired();

            // tenant id reflects the id of the account
            builder.Ignore(acc => acc.TenantId);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(Consts.AccountNameMaxLength);

            builder.Property(x => x.AccountOwner)
                .HasColumnName("account_owner")
                .IsRequired();

            builder.HasMany(acc => acc.Teams)
                .WithOne(team => team.Account)
                .HasForeignKey(team => team.AccountId)
                .HasPrincipalKey(acc => acc.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}