using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Designly.Auth.Identity
{
    public class MustBeServiceAccountRequirementHandler : AuthorizationHandler<MustBeServiceAccountRequirement>
    {
        private readonly ILogger<MustBeServiceAccountRequirementHandler> _logger;

        public MustBeServiceAccountRequirementHandler(ILogger<MustBeServiceAccountRequirementHandler> logger)
        {
            _logger = logger;
        }

        // Handling MustBeServiceAccountRequirement
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeServiceAccountRequirement requirement)
        {
            // search the claim in the token group membership

            bool isWithServiceAccountMembership = isServiceAccountGroupMember(context);
            if (isWithServiceAccountMembership)
            {
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning($"User {context?.User?.Identity?.Name} is not authorized to access this resource");
                context?.Fail();
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
                _logger.LogError(exception, $"Error while checking the user group membership");
            }

            return false;
        }
    }
}
