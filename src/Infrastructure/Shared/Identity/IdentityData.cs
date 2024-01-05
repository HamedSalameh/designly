namespace Designly.Shared.Identity
{
    public class IdentityData
    {
        public const string JwtClaimType = "cognito:groups"; // Claim type
        public const string AdminGroup = "designly_admins"; // Group name
        public const string AccountOwnerGroup = "designly_account_owners"; // Group name
        public const string AccountUsersGroup = "designly_account_users"; // Group name

        public const string AdminUserPolicyName = "AdminUserPolicy"; // Policy name
        public const string AccountOwnerPolicyName = "AccountOwnerPolicy"; // Policy name
        public const string AccountUserPolicyName = "AccountUserPolicy"; // Policy name

        public static string TenantIdClaimType = "tenant_"; // Claim type
        public static string TenantId = "tenant_id"; // Claim type
    }
}
