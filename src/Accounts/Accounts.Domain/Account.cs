
using static Accounts.Domain.Consts;

namespace Accounts.Domain
{
    public class Account : Entity
    {
        public string Name { get; set; }

        public User? Owner { get; set; }

        public AccountStatus Status { get; private set; }

        public ICollection<Team> Teams { get; private set; }

        public Account(string Name, User accountOwner) : base()
        {
            if (string.IsNullOrEmpty(Name)) throw new ArgumentNullException(nameof(Name));
            if (accountOwner == default) throw new ArgumentNullException(nameof(accountOwner));

            this.Name = Name;
            Owner = accountOwner;
            Status = AccountStatus.Active;

            Teams = new List<Team>();
        }

        public Account(string Name)
        {
            if (string.IsNullOrEmpty(Name)) throw new ArgumentNullException(nameof(Name));

            this.Name = Name;
            Status = AccountStatus.Pending;

            Teams = new List<Team>();
        }

        // Used by EF, Dapper, etc.
        protected Account()
        {
            Name = string.Empty;
            Teams = new List<Team>();
        }

        public void AssignOwner(User accountOwner)
        {
            if (accountOwner == default) throw new ArgumentNullException(nameof(accountOwner));

            Owner = accountOwner;
            Status = AccountStatus.Active;
        }

        public void CreateDefaultTeam()
        {
            if (Teams == null)
            {
                Teams = new List<Team>();
            }

            var defaultTeam = Teams.FirstOrDefault(t => t.Name == Consts.DefaultTeamName);

            if (defaultTeam == null)
            {
                defaultTeam = new Team(Consts.DefaultTeamName, this);
                Teams.Add(defaultTeam);
            }
        }

        public void AddUserToDefaultTeam(User user)
        {
            var defaultTeam = Teams.FirstOrDefault(t => t.Name == Consts.DefaultTeamName);
            if (defaultTeam == null)
            {
                throw new Exception("The default team is not created yet");
            }
            defaultTeam.AddMember(user);
        }

        public void AddTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            if (Teams == null)
            {
                Teams = new List<Team>();
            }

            Teams.Add(team);
        }

        public void RemoveTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            if (Teams == null)
            {
                Teams = new List<Team>();
            }

            Teams.Remove(team);
        }

        public void ChangeAccountStatus(AccountStatus newStatus)
        {
            // TODO: Add account status change rules
            Status = newStatus;
        }
    }
}
