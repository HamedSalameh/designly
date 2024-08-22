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
        public static IEndpointConventionBuilder MapSearchAccountUsersEndpoint(this IEndpointRouteBuilder builder, string pattern = "{accountId}/users/search")
        {
            var endpoint = builder
                .MapPost(pattern, SearchAccountUsersEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoint;
        }

        private static async Task<IResult> SearchAccountUsersEndpointMethodAsync([FromRoute] Guid accountId, [FromBody] SearchUsersRequest searchUsersRequest,
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

            // TODO: How to maintain security that only admin or account owner search? where does tenant from token goes ?
            var tenantId = accountId;

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
