using Accounts.Application.Extensions;
using Designly.Auth.Policies;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Accounts.Application.Features.ValidateUser
{
    public static class ValidateUserEndpoint
    {
        public static IEndpointConventionBuilder MapValidateUserEndpoint(this IEndpointRouteBuilder routeBuilder, string pattern = "{accountId}/users/{userId}/validate") {
            var endpoint = routeBuilder.MapPost(pattern, ValidateUserEndpointAsync)
                .RequireAuthorization(policyBuilder => policyBuilder.AddRequirements(new MustBeServiceAccountRequirement()))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> ValidateUserEndpointAsync([FromRoute] Guid accountId, [FromRoute] Guid userId, 
            ISender sender,
            CancellationToken cancellationToken)
        {
            var validateUserIdCommand = new ValidateUserCommand(userId, accountId);

            var operationResult = await sender.Send(validateUserIdCommand, cancellationToken).ConfigureAwait(false);

            return operationResult.ToActionResult();
        }
    }
}
