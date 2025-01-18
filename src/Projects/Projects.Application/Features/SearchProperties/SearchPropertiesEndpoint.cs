using Amazon.Runtime.SharedInterfaces;
using Designly.Auth.Identity;
using Designly.Filter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Projects.Application.Extentions;

namespace Projects.Application.Features.SearchProperties
{
    public static class SearchPropertiesEndpoint
    {
        public static IEndpointConventionBuilder MapSearchPropertiesEndpoint(this IEndpointRouteBuilder endpoints, string pattern)
        {
            var endPoint = endpoints
                .MapPost(pattern, SearchPropertiesEndpointMethodAsync)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status400BadRequest);

            return endPoint;
        }

        public static async Task<IResult> SearchPropertiesEndpointMethodAsync([FromBody] SearchPropertiesRequest searchPropertiesRequest,
                       ITenantProvider tenantProvider,
                       ISender sender,
                       ILoggerFactory loggerFactory,
                       CancellationToken cancellationToken)
        {
            ILogger logger = loggerFactory.CreateLogger("SearchPropertiesFeature");

            if (searchPropertiesRequest == null)
            {
                logger.LogError("Invalid value for {Request}", nameof(searchPropertiesRequest));
                return Results.BadRequest("The submitted search property object is not valid or empty");
            }
            if (string.IsNullOrWhiteSpace(searchPropertiesRequest.City) &&
                string.IsNullOrWhiteSpace(searchPropertiesRequest.Street))
            {
                logger.LogWarning("Request object is not valid. At least one of the following properties must be provided: Id, City, Street");
                return Results.BadRequest("At least one of the following properties must be provided: Id, City, Street");
            }

            var tenantId = tenantProvider.GetTenantId();

            var searchPropertiesCommand = new SearchPropertiesCommand(tenantId);

            var filterConditions = new List<FilterCondition>();
            // Check if 'search all' is requested (indicated by '*' in City or Street)
            bool isSearchAll = searchPropertiesRequest.City == "*" || searchPropertiesRequest.Street == "*";

            if (!isSearchAll)
            {
                if (!string.IsNullOrWhiteSpace(searchPropertiesRequest.City))
                {
                    filterConditions.Add(new FilterCondition("City", FilterConditionOperator.Contains, [searchPropertiesRequest.City]));
                }
                if (!string.IsNullOrWhiteSpace(searchPropertiesRequest.Street))
                {
                    filterConditions.Add(new FilterCondition("Street", FilterConditionOperator.Contains, [searchPropertiesRequest.Street]));
                }
            }

            searchPropertiesCommand.FilterConditions = filterConditions;

            var properties = await sender.Send(searchPropertiesCommand, cancellationToken);

            return properties.ToActionResult(res => Results.Ok(res));
        }
    }
}
