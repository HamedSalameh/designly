namespace IdentityService.API.DTO
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public AuthenticatedUser(string? givenName, string? familyName, string email, Guid tenantId, string[] roles, string[] permissions, string? profileImage)
        {
            GivenName = givenName;
            FamilyName = familyName;
            Email = email;
            TenantId = tenantId;
            Roles = roles;
            Permissions = permissions;
            ProfileImage = profileImage;
        }

        public string Name => $"{GivenName} {FamilyName}";

        public string? GivenName { get; init; } = string.Empty;

        public string? FamilyName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public Guid TenantId { get; init; } = Guid.Empty;

        public string[] Roles { get; init; } = [];

        public string[] Permissions { get; init; } = [];

        public string? ProfileImage { get; init; } = string.Empty;
    }
}
