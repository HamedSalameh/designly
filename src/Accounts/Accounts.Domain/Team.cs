using System.Text;
using static Accounts.Domain.Consts;

namespace Accounts.Domain
{
    public class Team : Entity
    {
        public Guid AccountId { get; set; }
        
        public Account Account { get; set; }
        
        // For the root team, this is the tenantId because the root team is the tenant
        public Guid MemberOf { get; set; }

        public TeamStatus TeamStatus { get; private set; }

        public string Name { get; set; }

        public ICollection<User> Members { get; set; }

        public Team(string name, Guid memberOf, Guid tenantId) : base(tenantId)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (memberOf == Guid.Empty || memberOf == default) throw new ArgumentNullException(nameof(memberOf));

            Name = name;
            MemberOf = memberOf;
            Members = new List<User>();
            TeamStatus = TeamStatus.Active;
        }

        public Team(string name, Guid memberOf, List<User> members, Guid tenantId) : this(name, memberOf, tenantId)
        {
            if (members == null) throw new ArgumentNullException(nameof(members));
            Members = members;
        }

        // Used by EF, Dapper, etc.
        protected Team()
        {
            Name = string.Empty;
            Members = new List<User>();
        }

        public override string ToString()
        {
            // return the team name and its members
            StringBuilder sb = new StringBuilder();
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
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (Members == null)
            {
                Members = new List<User>();
            }

            Members.Add(user);
        }

        public void RemoveMember(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (Members == null)
            {
                Members = new List<User>();
            }

            Members.Remove(user);
        }

        public void AddMembers(IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));

            if (Members == null)
            {
                Members = new List<User>();
            }
            
            foreach (var user in users)
            {
                Members.Add(user);
            }
        }

        public void RemoveMembers(IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));

            if (Members == null)
            {
                Members = new List<User>();
            }

            foreach (var user in users)
            {
                Members.Remove(user);
            }
        }

        public void RemoveAllMembers()
        {
            if (Members == null)
            {
                Members = new List<User>();
            }

            Members.Clear();
        }

        public void ChangeTeamStatus(TeamStatus newStatus)
        {
            // TODO: Add team status change rules
            TeamStatus = newStatus;
        }
    }
}
