using Designly.Auth.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace Designly.Auth
{
    public class TenantProviderMiddleware(ITenantProvider authorizationProvider) : IMiddleware
    {
        private readonly ITenantProvider authorizationProvider = authorizationProvider ?? throw new ArgumentNullException(nameof(authorizationProvider));

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (authorizationProvider.IsServiceAccount(context))
            {
                await next(context);
                return;
            }

            var tenantId = authorizationProvider.GetTenantIdFromRequest(context);

            if (tenantId is null || tenantId == Guid.Empty)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var problemDetails = new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = "Tenant Id is missing or empty"
                };
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                return;
            }

            authorizationProvider.SetTenantId(tenantId.Value);

            await next(context);
        }
    }
}
