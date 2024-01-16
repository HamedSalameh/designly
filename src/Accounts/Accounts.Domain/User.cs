using static Accounts.Domain.Consts;

namespace Accounts.Domain
{
    public class User : Entity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? JobTitle { get; set; }
        public UserStatus Status { get; set; }
        public ICollection<Team> Teams { get; set; }
        
        public User(string firstName, string lastName, string email, string? jobTitle, Account account) : base()
        {
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentNullException(nameof(lastName));
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            FirstName = firstName;
            LastName = lastName;
            JobTitle = jobTitle;
            Email = email;
            Status = UserStatus.BeforeActivation;
            Teams = new List<Team>();
            Account = account;
            AccountId = account.Id;
        }

        // Used by EF, Dapper, etc.
        protected User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Email})";
        }

        public void Activate()
        {
            if (Status == UserStatus.Active) return;
            if (Status == UserStatus.BeforeActivation)
            {
                Status = UserStatus.Active;
            }
            else
            {
                throw new InvalidOperationException($"Cannot activate user with status {Status}. Only users in '{nameof(UserStatus.BeforeActivation)}' status can be activated.");
            }
        }

        public void Suspend()
        {
            if (Status == UserStatus.Suspended) return;
            if (Status == UserStatus.Active)
            {
                Status = UserStatus.Suspended;
            }
            else
            {
                throw new InvalidOperationException($"Cannot suspend user with status {Status}. Only active users can be suspended.");
            }
        }

        public void Disable()
        {
            if (Status == UserStatus.Disabled) return;
            if (Status == UserStatus.BeforeActivation || Status == UserStatus.Active || Status == UserStatus.Suspended)
            {
                Status = UserStatus.Disabled;
            }
            else
            {
                throw new InvalidOperationException($"Cannot disable user with status {Status}. Only '{nameof(UserStatus.BeforeActivation)}', '{nameof(UserStatus.Active)}', and '{nameof(UserStatus.Suspended)}' users can be disabled.");
            }
        }

        public void MarkForDeletion()
        {
            if (Status == UserStatus.MarkedForDeletion) return;
            if (Status == UserStatus.Deleted)
            {
                throw new InvalidOperationException($"Cannot mark for deletion user with status {Status}. Already deleted users cannot be marked for deletion.");
            }
            Status = UserStatus.MarkedForDeletion;
        }

        public void MarkDeleted()
        {
            if (Status == UserStatus.Deleted) return;
            if (Status == UserStatus.MarkedForDeletion)
            {
                Status = UserStatus.Deleted;
            }
            else
            {
                throw new InvalidOperationException($"Cannot delete user with status {Status}. Only '{nameof(UserStatus.MarkedForDeletion)}' users can be deleted.");
            }
        }
    }
}
