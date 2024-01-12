namespace Accounts.Domain
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? JobTitle { get; set; }

        public Guid MemberOf { get; set; }

        public User(string firstName, string lastName, string email, string? jobTitle, Guid memberOf)
        {
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentNullException(nameof(lastName));
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (memberOf == Guid.Empty || memberOf == default) throw new ArgumentNullException(nameof(memberOf));

            FirstName = firstName;
            LastName = lastName;
            JobTitle = jobTitle;
            Email = email;
            MemberOf = memberOf;
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
    }
}
