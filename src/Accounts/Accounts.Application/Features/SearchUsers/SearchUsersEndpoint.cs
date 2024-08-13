using Accounts.Application.Extensions;
using Designly.Auth.Identity;
using Designly.Filter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Features.SearchUsers
{
    public static class SearchUsersEndpoint
    {
        public static IEndpointConventionBuilder MapSearchUsersEndpoint(this IEndpointRouteBuilder builder, string pattern)
        {
            var endpoint = builder
                .MapPost(pattern, SearchUsersEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> SearchUsersEndpointMethodAsync([FromBody] SearchUsersRequest searchUsersRequest,
            ITenantProvider tenantProvider,
            ISender sender,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("SearchProjectsFeature");

            if (searchUsersRequest == null)
            {
                logger.LogError("Invalid value for {Request}", nameof(searchUsersRequest));
                return Results.BadRequest("The submitted search user object is not valid or empty");
            }

            var tenantId = tenantProvider.GetTenantId();

            var searchUsersCommand = new SearchUsersCommand(tenantId);

            var filterConditions = new List<FilterCondition>();
            foreach (var filter in searchUsersRequest.filters)
            {
                if (filter == null || filter.Operator is null || filter.Field is null)
                {
                    return Results.BadRequest("One of the filter conditions is not valid.");
                }
                if (!SupportedFilterConditionOperators.FilterConditionOperatorsDictionary.TryGetValue(filter.Operator.ToLower(), out var filterConditionOperator))
                {
                    return Results.BadRequest("We could not parse a filter operator for one of the filter conditions.");
                }
                if (!SupportedUserFieldNames.UserFieldNamesDictionary.TryGetValue(filter.Field, out var filterConditionField))
                {
                    return Results.BadRequest("We could not parse a filter field for one of the filter conditions.");
                }

                // convert the filter.valuies from JsonElement to the correct type
                var valuesList = filter.Value.ToList();


                filterConditions.Add(new FilterCondition(filterConditionField, filterConditionOperator, valuesList));
            }

            searchUsersCommand.FilterConditions = filterConditions;

            var result = await sender.Send(searchUsersCommand, cancellationToken).ConfigureAwait(false);

            return result.ToActionResult();
        }
    }
}
