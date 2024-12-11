using Amazon.Runtime.Internal.Util;
using Designly.Base.Exceptions;
using Designly.Filter;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Features.SearchProjects;
using Projects.Domain;
using Projects.Infrastructure.Filter;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.SearchProperties
{
    public class SearchPropertiesCommandHandler : IRequestHandler<SearchPropertiesCommand, Result<IEnumerable<Property>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchPropertiesCommandHandler> _logger;

        public SearchPropertiesCommandHandler(IUnitOfWork unitOfWork, ILogger<SearchPropertiesCommandHandler> logger)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(logger);

            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<Property>>> Handle(SearchPropertiesCommand searchPropertiesCommand, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {SearchPropertiesCommand}", nameof(SearchPropertiesCommand));
            }

            FilterDefinition filterDefinition = GetFilterDefinition(searchPropertiesCommand);
            // We will not use SqlKata or dynamic SQL generating here, due to the usage of JSONB for the address

            var results = await _unitOfWork.PropertiesRepository.SearchPropertiesAsync(searchPropertiesCommand.TenantId, filterDefinition, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Request {SearchPropertiesCommand} was handled and returned {Results}", nameof(SearchPropertiesCommand), results?.Count());
            }

            return new Result<IEnumerable<Property>>(results ?? []);
        }

        private static FilterDefinition GetFilterDefinition(SearchPropertiesCommand request)
        {
            var filters = request.FilterConditions;
            filters.Add(new FilterCondition(PropertyFieldToColumnMapping.TenantId, FilterConditionOperator.Equals, [request.TenantId.Id]));
            return new FilterDefinition(PropertyFieldToColumnMapping.PropertiesTable, request.FilterConditions);
        }
    }
}
