﻿using Accounts.Application.Extensions;
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
                //.RequireAuthorization(policyBuilder => policyBuilder.AddRequirements(new MustBeAdminRequirement()))
                .AllowAnonymous()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endPoint;
        }

        private static async Task<IActionResult> CreateAccountEndpointMethodAsync([FromBody] CreateAccountRequestDto createAccountRequestDto,
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
                return new BadRequestObjectResult($"Invalid value for {nameof(createAccountRequestDto)}");
            }

            var createAccountCommand = createAccountRequestDto.Adapt<CreateAccountCommand>();

            var operationResult = await sender.Send(createAccountCommand, cancellationToken).ConfigureAwait(false);

            return operationResult.ToActionResult();
        }

        
    }
}
