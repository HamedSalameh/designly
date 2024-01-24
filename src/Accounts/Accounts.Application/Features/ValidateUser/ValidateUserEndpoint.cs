using Accounts.Application.Extensions;
using Designly.Auth.Identity;
using Mapster;
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
            var endpoint = routeBuilder.MapPost(pattern, ValidateUserEndpointAsync)
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

        private static async Task<IResult> ValidateUserEndpointAsync([FromBody] ValidateUserCommandDto validateUserCommandDto, 
            CancellationToken cancellationToken,
            ITenantProvider tenantProvider,
            HttpContext httpContext,
            ISender sender)
        {
            var validateUserIdCommand = new ValidateUserCommand(validateUserCommandDto.Email, validateUserCommandDto.tenantId);

            var operationResult = await sender.Send(validateUserIdCommand, cancellationToken).ConfigureAwait(false);

            return operationResult.ToActionResult();
        }
    }
}
