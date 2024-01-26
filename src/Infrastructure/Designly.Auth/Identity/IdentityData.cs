namespace Designly.Auth.Identity
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

        // service account custom scope key and value
        public static string ServiceAccountPolicyName = "ServiceAccountPolicy"; // Policy name
        public static string CustomScopesClaimType = "scope"; // Claim type
        public static string ServiceAccountScopeValue = "service_account"; // Claim value

        public static string TenantIdClaimType = "tenant_"; // Claim type
        public static string TenantId = "tenant_id"; // Claim type
    }
}
