namespace Accounts.Domain
{
    public class TeamMembers
    {
        public required Guid TeamId;
        public required Team Team;

        public required Guid UserId;
        public required User User;
    }
}
