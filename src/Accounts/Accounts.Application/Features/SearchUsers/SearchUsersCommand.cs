using Accounts.Domain;
using Designly.Filter;
using LanguageExt.Common;
using MediatR;

namespace Accounts.Application.Features.SearchUsers
{
    public record SearchUsersCommand(Guid TenantId) : IRequest<Result<IEnumerable<UserDto>>>
    {
        public List<FilterCondition> FilterConditions { get; set; } = [];
    }
}
