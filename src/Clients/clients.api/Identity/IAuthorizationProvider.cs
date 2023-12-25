using System.Security.Claims;

namespace Clients.API.Identity
{
    public interface IAuthorizationProvider
    { 
        Guid? GetTenantId(ClaimsPrincipal user);
    }
}
