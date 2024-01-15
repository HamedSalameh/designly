
using static Accounts.Domain.Consts;

namespace Accounts.Domain
{
    public class Account : Entity
    {
        // AccountId is the tenantId as well
        public override Guid TenantId => Id;

        public string Name { get; set; }

        public Guid AccountOwner { get; set; }

        public AccountStatus Status { get; private set; }

        public ICollection<Team> Teams { get; private set; }

        public Account(string Name, Guid accountOwner) : base()
        {
            if (string.IsNullOrEmpty(Name)) throw new ArgumentNullException(nameof(Name));
            if (accountOwner == Guid.Empty || accountOwner == default) throw new ArgumentNullException(nameof(accountOwner));

            this.Name = Name;
            AccountOwner = accountOwner;
            Teams = new List<Team>();
            Status = AccountStatus.Active;
        }

        // Used by EF, Dapper, etc.
        protected Account()
        {
            Name = string.Empty;
            Teams = new List<Team>();
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
