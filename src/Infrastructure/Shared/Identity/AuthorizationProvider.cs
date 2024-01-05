using System.Security.Claims;

namespace Designly.Shared.Identity
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        public Guid? GetTenantId(ClaimsPrincipal user)
        {
            if (user != null && user.Claims != null)
            {
                var tenantId = "";
                tenantId = user.Claims?.FirstOrDefault(claim =>
                               claim.Type == IdentityData.JwtClaimType && claim.Value.StartsWith(IdentityData.TenantIdClaimType))?.Value;

                Guid.TryParse(tenantId.Split('_')[1], out var tenantIdGuid);

                return tenantIdGuid;
            }
            return null;
        }
    }
}
