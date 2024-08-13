using Accounts.Domain;
using Accounts.Infrastructure.Filter;
using Accounts.Infrastructure.Interfaces;
using Designly.Base.Exceptions;
using Designly.Filter;
using Designly.Shared;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

namespace Accounts.Application.Features.SearchUsers
{
    public class SearchUsersCommandHandler : IRequestHandler<SearchUsersCommand, Result<IEnumerable<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchUsersCommandHandler> _logger;
        private readonly IQueryBuilder _queryBuilder;

        public SearchUsersCommandHandler(IUnitOfWork unitOfWork, ILogger<SearchUsersCommandHandler> logger, IQueryBuilder queryBuilder)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(queryBuilder);

            _unitOfWork = unitOfWork;
            _logger = logger;
            _queryBuilder = queryBuilder;
        }

        public async Task<Result<IEnumerable<UserDto>>> Handle(SearchUsersCommand searchUsersCommand, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {SearchUsersCommand}", nameof(SearchUsersCommand));
            }

            FilterDefinition filterDefinition = GetFilterDefinition(searchUsersCommand);
            var sqlQuery = _queryBuilder.BuildAsync(filterDefinition);
            if (sqlQuery == null)
            {
                var errorMessage = "The query builder failed to build the query.";
                _logger.LogError("{ErrorMessage}", errorMessage);

                return new Result<IEnumerable<UserDto>>(new BusinessLogicException(errorMessage));
            }

            var results = await _unitOfWork.UsersRepository.GetUsersAsync(searchUsersCommand.TenantId, sqlQuery, cancellationToken).ConfigureAwait(false);

            var users = results.Select(u =>
            {
                return new UserDto()
                {
                    Account = u.AccountId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    JobTitle = u.JobTitle,
                    Email = u.Email,
                    userStatus = u.Status
                };
            });

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Request {SearchUsersCommand} was handled and returned {Results}", nameof(SearchUsersCommand), results?.Count());
            }

            return new Result<IEnumerable<UserDto>>(users ?? []);
        }

        private static FilterDefinition GetFilterDefinition(SearchUsersCommand request)
        {
            var filters = request.FilterConditions;
            filters.Add(new FilterCondition(UserFieldToColumnMapping.TenantId, FilterConditionOperator.Equals, [request.TenantId]));
            return new FilterDefinition(UserFieldToColumnMapping.UsersTable, request.FilterConditions);
        }
    }
}
