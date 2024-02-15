using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Designly.Auth.Identity
{
    public class TenantProvider : ITenantProvider
    {
        private Guid TenantId { get; set; }

        public void SetTenantId(Guid tenantId)
        {
            if (tenantId == Guid.Empty) throw new ArgumentException($"Invalid value for {nameof(tenantId)} : {tenantId}", nameof(tenantId));

            TenantId = tenantId;
        }

        public Guid GetTenantId() => TenantId;

        public Guid? GetTenantIdFromRequest(HttpContext context)
        {
            if (context is null) return null;

            return GetTenantIdFromClaimsPrincipal(context.User);
        }

        private static Guid? GetTenantIdFromClaimsPrincipal(ClaimsPrincipal user)
        {
            if (user != null && user.Claims != null)
            {
                var tenantIdClaim = user.Claims?.FirstOrDefault(claim => claim.Type == IdentityData.TenantId);
                if (tenantIdClaim != null)
                {
                    var tenantId = tenantIdClaim.Value;
                    _ = Guid.TryParse(tenantId, out var tenantIdGuid);

                    return tenantIdGuid;
                }
            }
            return null;
        }

        public bool IsServiceAccount(HttpContext context)
        {
            if (context is null) return false;

            var isServiceAccount = context.User
                .Claims
                .Any(c => c.Type == IdentityData.CustomScopesClaimType && c.Value.Contains(IdentityData.ServiceAccountScopeValue));

            return isServiceAccount;
        }
    }
}
