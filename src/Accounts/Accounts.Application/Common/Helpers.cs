using Microsoft.AspNetCore.Http;

namespace Projects.Application.Common
{
    internal static class Helpers
    {
        // build resource uri (REST)
        public static string BuildResourceUri<T>(HttpContext httpContext, T Id)
        {
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + $"/{Id}";
            return locationUrl;
        }
    }
}
