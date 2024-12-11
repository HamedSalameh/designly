using Designly.Filter;
using LanguageExt.Common;
using MediatR;
using Projects.Domain;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.SearchProperties
{
    public sealed class SearchPropertiesCommand : IRequest<Result<IEnumerable<Property>>>
    {
        public TenantId TenantId { get; set; }
        public List<FilterCondition> FilterConditions { get; set; } = [];

        public SearchPropertiesCommand(TenantId tenantId)
        {
            TenantId = tenantId;
        }
    }
}
