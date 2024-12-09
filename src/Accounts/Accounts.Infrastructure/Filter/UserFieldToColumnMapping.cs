namespace Accounts.Infrastructure.Filter
{
    public static class UserFieldToColumnMapping
    {
        public const string UsersTable = "users";

        public const string Id = "id";
        public const string TenantId = "account_id";     // tenant is account, hence tenant_id is mapped to account_id
        public const string FirstName = "first_name";
        public const string LastName = "last_name";
        public const string Email = "email";
        public const string Role = "role";
        public const string Status = "status";
    }
}
