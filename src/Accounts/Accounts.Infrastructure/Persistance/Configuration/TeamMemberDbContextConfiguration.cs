using Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Persistance.Configuration
{
    public class TeamMemberDbContextConfiguration : IEntityTypeConfiguration<TeamMembers>
    {
        public void Configure(EntityTypeBuilder<TeamMembers> builder)
        {
            builder.ToTable("team_members")
                .HasKey(tm => new { tm.TeamId, tm.UserId });

            builder.Property(TeamMembers => TeamMembers.TeamId)
                .HasColumnName("team_id");

            builder.Property(TeamMembers => TeamMembers.UserId)
                .HasColumnName("user_id");
        }
    }
}
