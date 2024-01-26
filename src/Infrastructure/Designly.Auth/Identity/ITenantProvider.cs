using Microsoft.AspNetCore.Http;

namespace Designly.Auth.Identity
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
        void SetTenantId(Guid tenantId);
        Guid? GetTenantIdFromRequest(HttpContext context);
        bool IsServiceAccount(HttpContext context);
    }
}
