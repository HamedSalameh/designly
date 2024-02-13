namespace Designly.Auth.Identity
{
    public class IdentityData
    {
        public static readonly string JwtClaimType = "cognito:groups"; // Claim type
        public static readonly string AdminGroup = "designly_admins"; // Group name
        public static readonly string AccountOwnerGroup = "designly_account_owners"; // Group name
        public static readonly string AccountUsersGroup = "designly_account_users"; // Group name

        public const string AdminUserPolicyName = "AdminUserPolicy"; // Policy name
        public const string AccountOwnerPolicyName = "AccountOwnerPolicy"; // Policy name
        public const string AccountUserPolicyName = "AccountUserPolicy"; // Policy name

        // service account custom scope key and value
        public static readonly string ServiceAccountPolicyName = "ServiceAccountPolicy"; // Policy name
        public static readonly string CustomScopesClaimType = "scope"; // Claim type
        public static readonly string ServiceAccountScopeValue = "service_account"; // Claim value

        public static readonly string TenantIdClaimType = "tenant_"; // Claim type
        public static readonly string TenantId = "tenant_id"; // Claim type
    }
}
