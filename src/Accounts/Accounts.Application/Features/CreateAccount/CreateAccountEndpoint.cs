using Accounts.Application.Extensions;
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
        public static IEndpointConventionBuilder MapCreateAccountFeature(this IEndpointRouteBuilder endpoints, string pattern = "accounts")
        {
            var endPoint = endpoints
                .MapPost(pattern, CreateAccountEndpointMethodAsync)
                .AllowAnonymous()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endPoint;
        }

        private static async Task<IResult> CreateAccountEndpointMethodAsync([FromBody] CreateAccountRequestDto createAccountRequestDto,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken
            )
        {
            ILogger logger = loggerFactory.CreateLogger("CreateProjectFeature");

            if (createAccountRequestDto == null)
            {
                logger.LogError("Invalid value for {CreateAccountRequestDto}", nameof(createAccountRequestDto));
                return Results.BadRequest($"Invalid value for {nameof(createAccountRequestDto)}");
            }

            var createAccountCommand = createAccountRequestDto.Adapt<CreateAccountCommand>();

            var operationResult = await sender.Send(createAccountCommand, cancellationToken).ConfigureAwait(false);

            return operationResult.ToActionResult();
        }

        
    }
}
