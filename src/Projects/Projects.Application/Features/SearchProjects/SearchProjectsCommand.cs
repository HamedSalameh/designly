using Designly.Filter;
using LanguageExt.Common;
using MediatR;
using Projects.Domain;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.SearchProjects
{
    public class SearchProjectsCommand : IRequest<Result<IEnumerable<BasicProject>>>
    {
        public TenantId TenantId { get; set; }
        public List<FilterCondition> FilterConditions { get; set; } = [];
    }
}