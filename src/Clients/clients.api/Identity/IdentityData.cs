namespace Clients.API.Identity
{
    public class IdentityData
    {
        public const string CognitoGroupsClaimType = "cognito:groups"; // Claim type
        public const string CognitoAdminGroup = "designly_admins"; // Group name
        public const string CognitoAccountOwnerGroup = "designly_account_owners"; // Group name

        public const string AdminUserPolicyName = "AdminUserPolicy"; // Policy name
        public const string AccountOwnerPolicyName = "AccountOwnerPolicy"; // Policy name
    }
}
