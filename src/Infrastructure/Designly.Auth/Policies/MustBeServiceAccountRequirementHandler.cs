using Designly.Auth.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Designly.Auth.Policies
{
    public class ServiceAccountAuthorizationHandler(ILogger<ServiceAccountAuthorizationHandler> logger) : IAuthorizationHandler
    {

        private readonly ILogger<ServiceAccountAuthorizationHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Checking if user is a service account");
            }

            var requirement = context.Requirements.FirstOrDefault(r => r is MustBeServiceAccountRequirement);
            if (requirement != null)
            {
                // search the claim in the token group membership
                var isServiceAccountUser = isServiceAccountGroupMember(context);

                if (isServiceAccountUser)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        private bool isServiceAccountGroupMember(AuthorizationHandlerContext context)
        {
            try
            {
                var isServiceAccount = context.User
                .Claims
                .Any(c => c.Type == IdentityData.CustomScopesClaimType && c.Value.Contains(IdentityData.ServiceAccountScopeValue));


                return isServiceAccount;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while checking the user group membership: {Error}", exception.Message);
            }

            return false;
        }
    }
}
