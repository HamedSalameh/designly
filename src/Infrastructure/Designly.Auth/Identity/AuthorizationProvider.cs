using System.Security.Claims;

namespace Designly.Auth.Identity
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        public Guid? GetTenantId(ClaimsPrincipal user)
        {
            if (user != null && user.Claims != null)
            {
                var tenantIdClaim = user.Claims?.FirstOrDefault(claim => claim.Type == IdentityData.TenantId);
                if (tenantIdClaim != null)
                {
                    var tenantId = tenantIdClaim.Value;
                    Guid.TryParse(tenantId, out var tenantIdGuid);

                    return tenantIdGuid;
                }
            }
            return null;
        }
    }
}
