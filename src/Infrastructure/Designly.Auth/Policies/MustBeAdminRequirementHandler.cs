﻿using Designly.Auth.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Designly.Auth.Policies
{

    public class MustBeAdminRequirementHandler : AuthorizationHandler<MustBeAdminRequirement>
    {
        private readonly ILogger<MustBeAdminRequirementHandler> _logger;

        public MustBeAdminRequirementHandler(ILogger<MustBeAdminRequirementHandler> logger)
        {
            _logger = logger;
        }

        // Handling MustBeAdminRequirement
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeAdminRequirement requirement)
        {
            // search the claim in the token group membership

            bool isWithAdminMembership = isAdminGroupMember(context);
            if (isWithAdminMembership)
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