﻿using Microsoft.AspNetCore.Authorization;

namespace Clients.API.Identity
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
            else
            {
                _logger.LogWarning($"User {context?.User?.Identity?.Name} is not authorized to access this resource");
                context?.Fail();
            }

            return Task.CompletedTask;
        }

        private bool isAccountOwnerGroupMember(AuthorizationHandlerContext context)
        {
            try
            {
                var isAccountOwnerUser = context.User.Claims.FirstOrDefault(claim =>
                                       claim.Type == IdentityData.CognitoGroupsClaimType &&
                                                              claim.Value == IdentityData.CognitoAccountOwnerGroup);

                return isAccountOwnerUser != null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error while checking the user group membership");
            }

            return false;
        }
    }
}