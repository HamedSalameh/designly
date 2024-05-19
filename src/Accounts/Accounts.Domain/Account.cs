#pragma warning disable IDE0070 // Use 'System.HashCode'

using System.Text;
using static Accounts.Domain.Consts;

namespace Accounts.Domain
{
    public sealed class Account : Entity
    {
        public string Name { get; set; }

        public Guid? OwnerId { get; set; }
        public User? Owner { get; set; }

        public AccountStatus Status { get; private set; }

        public ICollection<Team> Teams { get; private set; }

        public Account(string Name, User accountOwner) : base()
        {
            ArgumentNullException.ThrowIfNull(Name);
            ArgumentNullException.ThrowIfNull(accountOwner);

            this.Name = Name;
            Owner = accountOwner;
            Status = AccountStatus.InProcessRegisteration;

            Teams = new List<Team>();
        }

        public Account(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentNullException(nameof(Name));
            }

            this.Name = Name;
            Status = AccountStatus.InProcessRegisteration;

            Teams = new List<Team>();
        }

        // Used by EF, Dapper, etc.
        private Account()
        {
            Name = string.Empty;
            Teams = new List<Team>();
        }

        public void AssignOwner(User accountOwner)
        {
            ArgumentNullException.ThrowIfNull(accountOwner);

            Owner = accountOwner;
        }

        public void CreateDefaultTeam()
        {
            Teams ??= new List<Team>();

            var defaultTeam = Teams.FirstOrDefault(t => t.Name == DefaultTeamName);

            if (defaultTeam == null)
            {
                defaultTeam = new Team(DefaultTeamName, this);
                Teams.Add(defaultTeam);
            }
        }

        public void AddUserToDefaultTeam(User user)
        {
            var defaultTeam = Teams.FirstOrDefault(t => t.Name == DefaultTeamName)
                ?? throw new AccountException("The default team is not created yet");
            defaultTeam.AddMember(user);
        }

        public void AddTeam(Team team)
        {
            ArgumentNullException.ThrowIfNull(team);

            Teams ??= new List<Team>();

            Teams.Add(team);
        }

        public void RemoveTeam(Team team)
        {
            ArgumentNullException.ThrowIfNull(team);

            Teams ??= new List<Team>();

            Teams.Remove(team);
        }

        /// <summary>
        /// Activates the current account, if it is not already activated.
        /// </summary>
        /// <exception cref="AccountException"></exception>
        public void ActivateAccount()
        {
            if (Status == AccountStatus.Active) return;

            if (Owner == null)
            {
                throw new AccountException("Account owner is not assigned yet");
            }
            if (IsTransient())
            {
                throw new AccountException("Account is not created yet");
            }
            if (Status == AccountStatus.Deleted)
            {
                throw new AccountException(AccountErrors.AccountIsDeleted.Description);
            }
            if (Status == AccountStatus.MarkedForDeletion)
            {
                throw new AccountException(AccountErrors.AccountIsMarkedForDeletion.Description);
            }
            
            Status = AccountStatus.Active;
        }

        public void SuspendAccount()
        {
            if (Status == AccountStatus.Suspended || Status == AccountStatus.Deleted) return;

            if (Status == AccountStatus.Deleted)
            {
                throw new AccountException(AccountErrors.AccountIsDeleted.Description);
            }
            if (Status == AccountStatus.MarkedForDeletion)
            {
                throw new AccountException(AccountErrors.AccountIsMarkedForDeletion.Description);
            }

            Status = AccountStatus.Suspended;
        }

        public void DisableAccount()
        {
            if (Status == AccountStatus.Disabled) return;

            if (Status == AccountStatus.Deleted)
            {
                throw new AccountException(AccountErrors.AccountIsDeleted.Description);
            }
            if (Status == AccountStatus.MarkedForDeletion)
            {
                throw new AccountException(AccountErrors.AccountIsMarkedForDeletion.Description);
            }

            Status = AccountStatus.Disabled;
        }

        public void MarkAccountForDeletion()
        {
            if (Status == AccountStatus.MarkedForDeletion) return;

            if (Status == AccountStatus.Deleted)
            {
                throw new AccountException(AccountErrors.AccountIsDeleted.Description);
            }

            Status = AccountStatus.MarkedForDeletion;
        }

        public override bool Equals(object? obj) => Equals(obj as Account);       

        public override bool Equals(Entity? other)
        {
            if (other is not Account)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            Account item = (Account)other;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id == Id;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(Name);
            sb.Append(", ").Append(Owner?.ToString());
            sb.Append(", ").Append(Status.ToString());
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 37 + Name.GetHashCode();
                hash = hash * 41 + OwnerId.GetHashCode();
                hash = hash * 41 + Status.GetHashCode();
                return hash;
            }
        }
    }
}
