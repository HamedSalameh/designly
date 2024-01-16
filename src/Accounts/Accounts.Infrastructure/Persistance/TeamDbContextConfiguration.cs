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
                .HasMaxLength(Consts.TeamNameMaxLength);

            builder.Property(team => team.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(team => team.AccountId)
                .HasColumnName("account_id")
                .IsRequired();

            //builder.HasMany(team => team.Members)
            //    .WithMany(user => user.Teams)
            //    .UsingEntity(j => j.ToTable("team_members"));

            builder.HasOne(team => team.Account)
                .WithMany(account => account.Teams)
                .HasForeignKey(team => team.AccountId)
                .HasPrincipalKey(account => account.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Define many-to-many relationship between Team and User
            builder.HasMany(team => team.Members)
                .WithMany(member => member.Teams)
                .UsingEntity<TeamMembers>(table => table.ToTable("team_members"));
        }
    }
}