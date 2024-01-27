using Designly.Auth.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Designly.Auth.Policies
{
    // AdminUser authorization handler can handle any requirement that derives from IAuthorizationRequirement
    public class AdminUserAuthorizationHandler : IAuthorizationHandler
    {
        private readonly ILogger<AdminUserAuthorizationHandler> _logger;

        public AdminUserAuthorizationHandler(ILogger<AdminUserAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Checking if user is a system admin");
            }

            var requirement = context.Requirements.FirstOrDefault();
            if (requirement != null)
            {
                // search the claim in the token group membership
                var isSystemAdminUser = isAdminGroupMember(context);

                if (isSystemAdminUser)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        private bool isAdminGroupMember(AuthorizationHandlerContext context)
        {
            try
            {
                var isAdminUser = context.User.Claims.FirstOrDefault(claim =>
                        claim.Type == IdentityData.JwtClaimType &&
                        claim.Value == IdentityData.AdminGroup);

                return isAdminUser != null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error while checking the user group membership");
            }

            return false;
        }
    }
}