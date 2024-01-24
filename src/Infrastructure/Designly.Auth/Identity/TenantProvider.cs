using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Designly.Auth.Identity
{
    public class TenantProvider : ITenantProvider
    {
        private Guid TenantId { get; set; }

        public void SetTenantId(Guid tenantId)
        {
            if (tenantId == default || tenantId == Guid.Empty) throw new ArgumentException(nameof(tenantId));

            TenantId = tenantId;
        }

        public Guid GetTenantId() => TenantId;

        public Guid? GetTenantIdFromRequest(HttpContext context)
        {
            if (context is null) return null;

            return GetTenantIdFromClaimsPrincipal(context.User);
        }

        private Guid? GetTenantIdFromClaimsPrincipal(ClaimsPrincipal user)
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
