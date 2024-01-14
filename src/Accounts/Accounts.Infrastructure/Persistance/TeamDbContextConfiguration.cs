using Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Persistance
{
    public class TeamDbContextConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("teams");

            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");

            // primary key
            builder.HasKey(team => team.Id);
            builder.Property(team => team.Id).HasColumnName("id")
                .IsRequired();

            builder.Property(team => team.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(Consts.Team.NameMaxLength);

            builder.Property(team => team.MemberOf)
                .HasColumnName("member_of")
                .IsRequired();

            builder.HasMany(team => team.Members)
                .WithOne(user => user.Team)
                .HasForeignKey(user => user.MemberOf)
                .HasPrincipalKey(team => team.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(team => team.Account)
                .WithMany(account => account.Teams)
                .HasForeignKey(team => team.AccountId)
                .HasPrincipalKey(account => account.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}