using System.Security.Claims;

namespace Designly.Shared.Identity
{
    public interface IAuthorizationProvider
    {
        Guid? GetTenantId(ClaimsPrincipal user);
    }
}
