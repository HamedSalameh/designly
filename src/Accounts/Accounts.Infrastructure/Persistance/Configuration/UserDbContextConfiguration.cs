using Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Persistance.Configuration
{
    public class UserDbContextConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
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
                .HasMaxLength(Consts.FirstNameMaxLength);

            builder.Property(x => x.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(Consts.LastNameMaxLength);

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(Consts.EmailAddressMaxLength);

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
            builder.HasMany(user => user.Teams)
                .WithMany(team => team.Members)
                .UsingEntity<TeamMembers>(table => table.ToTable("team_members"));
        }
    }
}