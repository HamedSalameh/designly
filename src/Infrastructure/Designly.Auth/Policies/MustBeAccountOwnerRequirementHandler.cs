using Designly.Auth.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Designly.Auth.Policies
{
    public class MustBeAccountOwnerRequirementHandler : AuthorizationHandler<MustBeAccountOwnerRequirement>
    {
        private readonly ILogger<MustBeAccountOwnerRequirementHandler> _logger;

        public MustBeAccountOwnerRequirementHandler(ILogger<MustBeAccountOwnerRequirementHandler> logger)
        {
            _logger = logger;
        }

        // Handling MustBeAccountOwnerRequirement
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeAccountOwnerRequirement requirement)
        {
            // search the claim in the token group membership

            bool isWithAccountOwnerMembership = isAccountOwnerGroupMember(context);
            if (isWithAccountOwnerMembership)
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }

        private bool isAccountOwnerGroupMember(AuthorizationHandlerContext context)
        {
            try
            {
                var isAccountOwnerUser = context.User.Claims.FirstOrDefault(claim =>
                                       claim.Type == IdentityData.JwtClaimType &&
                                                              claim.Value == IdentityData.AccountOwnerGroup);

                return isAccountOwnerUser != null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while checking the user group membership: {Error}", exception.Message);
            }

            return false;
        }
    }
}