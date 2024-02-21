using System.Text;
using static Accounts.Domain.Consts;

namespace Accounts.Domain
{
    public class Team : Entity
    {
        public Guid AccountId { get; set; }
        
        public Account Account { get; set; }
        
        public TeamStatus Status { get; private set; }

        public string Name { get; set; }

        public ICollection<User> Members { get; set; }

        public Team(string name, Account account) : base()
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name;
            Members = new List<User>();
            Status = TeamStatus.Active;
            Account = account;
            AccountId = account.Id;
        }

        // Used by EF, Dapper, etc.
        protected Team()
        {
            Name = string.Empty;
            Members = new List<User>();
            Status = TeamStatus.Active;
            Account = default!;
        }

        public override string ToString()
        {
            // return the team name and its members
            StringBuilder sb = new();
            sb.AppendLine($"Team: {Name}");
            sb.AppendLine("Members:");
            foreach (var member in Members)
            {
                sb.AppendLine(member.ToString());
            }

            return sb.ToString();
        }

        public void AddMember(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            Members ??= new List<User>();

            Members.Add(user);
        }

        public void RemoveMember(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            Members ??= new List<User>();

            Members.Remove(user);
        }

        public void AddMembers(IEnumerable<User> users)
        {
            ArgumentNullException.ThrowIfNull(users);

            Members ??= new List<User>();
            
            foreach (var user in users)
            {
                Members.Add(user);
            }
        }

        public void RemoveMembers(IEnumerable<User> users)
        {
            ArgumentNullException.ThrowIfNull(users);

            Members ??= new List<User>();

            foreach (var user in users)
            {
                Members.Remove(user);
            }
        }

        public void RemoveAllMembers()
        {
            Members ??= new List<User>();

            Members.Clear();
        }

        public void ChangeTeamStatus(TeamStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
