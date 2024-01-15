using Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Persistance
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
                .HasMaxLength(Consts.MaxEmailAddressLength);

            builder.Property(x => x.JobTitle)
                .HasColumnName("job_title")
                .HasMaxLength(Consts.JobTitleMaxLength);

            builder.HasOne(user => user.Team)
                .WithMany(team => team.Members)
                .HasForeignKey(user => user.MemberOf)
                .HasPrincipalKey(team => team.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}