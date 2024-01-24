using Accounts.Application.Extensions;
using Designly.Auth.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Accounts.Application.Features.ValidateUser
{
    public static class ValidateUserEndpoint
    {
        public static IEndpointConventionBuilder MapValidateUserFeature(this IEndpointRouteBuilder routeBuilder, string pattern = "validateuser") {
            var endpoint = routeBuilder.MapGet(pattern, ValidateUserEndpointAsync)
                .RequireAuthorization(policyBuilder => policyBuilder
                    .AddRequirements(new MustBeAccountOwnerRequirement())
                    .AddRequirements(new MustBeAdminRequirement()))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> ValidateUserEndpointAsync([FromQuery] Guid userId, 
            CancellationToken cancellationToken,
            ITenantProvider tenantProvider,
            HttpContext httpContext,
            ISender sender)
        {
            var tenanId = tenantProvider.GetTenantId();

            var validateUserIdCommand = new ValidateUserCommand(userId, tenanId);

            var operationResult = await sender.Send(validateUserIdCommand, cancellationToken).ConfigureAwait(false);

            return operationResult.ToActionResult();
        }
    }
}
