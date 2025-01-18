using Designly.Filter;
using LanguageExt.Common;
using MediatR;
using Projects.Domain;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.GetProperty
{
    public class GetRealestatePropertyCommand : IRequest<Result<Property?>>
    {
        public TenantId TenantId { get; set; }
        public List<FilterCondition> FilterConditions { get; set; } = [];

        public GetRealestatePropertyCommand(TenantId tenantId)
        {
            TenantId = tenantId;
        }
    }
}
