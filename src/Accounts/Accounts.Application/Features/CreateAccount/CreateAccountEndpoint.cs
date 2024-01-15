using Designly.Auth.Identity;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Features.CreateAccount
{
    public static class CreateAccountEndpoint
    {
        public static IEndpointConventionBuilder MapCreateAccountFeature(this IEndpointRouteBuilder endpoints, string pattern = "")
        {
            var endPoint = endpoints
                .MapPost(pattern, CreateAccountEndpointMethodAsync)
                .RequireAuthorization(policyBuilder => policyBuilder.AddRequirements(new MustBeAdminRequirement()))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endPoint;
        }

        private static async Task<IResult> CreateAccountEndpointMethodAsync([FromBody] CreateAccountRequestDto createAccountRequestDto,
            IAuthorizationProvider authroizationProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            HttpContext httpContext,
            CancellationToken cancellationToken
            )
        {
            ILogger logger = loggerFactory.CreateLogger("CreateProjectFeature");

            if (createAccountRequestDto == null)
            {
                logger.LogError($"Invalid value for {nameof(createAccountRequestDto)}");
                return Results.BadRequest($"The submitted project object is not valid or empty");
            }

            var tenantId = authroizationProvider.GetTenantId(httpContext.User);
            if (tenantId is null || Guid.Empty == tenantId)
            {
                logger.LogError($"Invalid value for {nameof(tenantId)}");
                return Results.BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var createAccountCommand = createAccountRequestDto.Adapt<CreateAccountCommand>();
            createAccountCommand.TenantId = tenantId.Value;

            var accountId = await sender.Send(createAccountCommand, cancellationToken).ConfigureAwait(false);

            return Results.Ok(accountId);
        }
    }
}
