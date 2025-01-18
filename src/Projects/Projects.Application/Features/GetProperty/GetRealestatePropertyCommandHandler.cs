using Designly.Base.Exceptions;
using Designly.Filter;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Application.Features.SearchProperties;
using Projects.Domain;
using Projects.Infrastructure.Filter;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.GetProperty
{
    public class GetRealestatePropertyCommandHandler : IRequestHandler<GetRealestatePropertyCommand, Result<Property?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetRealestatePropertyCommandHandler> _logger;
        private readonly IQueryBuilder _queryBuilder;

        public GetRealestatePropertyCommandHandler(IUnitOfWork unitOfWork, ILogger<GetRealestatePropertyCommandHandler> logger, IQueryBuilder queryBuilder)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(queryBuilder);

            _unitOfWork = unitOfWork;
            _logger = logger;
            _queryBuilder = queryBuilder;
        }

        public async Task<Result<Property?>> Handle(GetRealestatePropertyCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {GetRealestatePropertyCommand}", nameof(GetRealestatePropertyCommand));
            }

            FilterDefinition filterDefinition = GetFilterDefinition(request);
            // We will not use SqlKata or dynamic SQL generating here, due to the usage of JSONB for the address

            var results = await _unitOfWork.PropertiesRepository.SearchPropertiesAsync(request.TenantId, filterDefinition, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Request {SearchPropertiesCommand} was handled and returned {Results}", nameof(SearchPropertiesCommand), results?.Count());
            }

            return new Result<Property?>(results?.FirstOrDefault());
        }

        private static FilterDefinition GetFilterDefinition(GetRealestatePropertyCommand request)
        {
            var filters = request.FilterConditions;
            filters.Add(new FilterCondition(PropertyFieldToColumnMapping.TenantId, FilterConditionOperator.Equals, [request.TenantId.Id]));
            return new FilterDefinition(PropertyFieldToColumnMapping.PropertiesTable, request.FilterConditions);
        }
    }
}
