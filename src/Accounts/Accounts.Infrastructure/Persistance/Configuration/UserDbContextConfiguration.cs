using Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Accounts.Infrastructure.Persistance.Configuration
{
    public class UserDbContextConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // map to table
            builder.ToTable("users");

            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(Designly.Shared.Consts.FirstNameMaxLength);

            builder.Property(x => x.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(Designly.Shared.Consts.LastNameMaxLength);

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(Designly.Shared.Consts.MaxEmailAddressLength);

            builder.Property(x => x.AccountId)
                .HasColumnName("account_id")
                .IsRequired();

            builder.Property(x => x.JobTitle)
                .HasColumnName("job_title")
                .HasMaxLength(Consts.JobTitleMaxLength);

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            // many to many between user and teams
            builder
                .HasMany(user => user.Teams)
                .WithMany(team => team.Members)
                .UsingEntity<TeamMembers>(
                    j => j
                        .HasOne(tm => tm.Team)
                        .WithMany()
                        .HasForeignKey(tm => tm.TeamId),
                    j => j
                        .HasOne(tm => tm.User)
                        .WithMany()
                        .HasForeignKey(tm => tm.UserId),
                    j => j
                        .ToTable("team_members"));
        }
    }
}