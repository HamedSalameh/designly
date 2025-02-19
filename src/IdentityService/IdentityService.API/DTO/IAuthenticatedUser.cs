namespace IdentityService.API.DTO
{
    public interface IAuthenticatedUser
    {
        string Name { get; }
        string GivenName { get; }
        string FamilyName { get; }
        string Email { get; }
        Guid TenantId { get; }
        string[] Roles { get; }
        string[] Permissions { get; }
        string ProfileImage { get; }
    }
}
