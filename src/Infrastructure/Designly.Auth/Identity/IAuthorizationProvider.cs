using System.Security.Claims;

namespace Designly.Auth.Identity
{
    public interface IAuthorizationProvider
    {
        Guid? GetTenantId(ClaimsPrincipal user);
    }
}
