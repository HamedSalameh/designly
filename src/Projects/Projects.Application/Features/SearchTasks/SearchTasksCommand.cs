using LanguageExt.Common;
using MediatR;
using Projects.Application.Filter;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTasksCommand : IRequest<Result<IEnumerable<TaskItem>>>
    {
        public TenantId tenantId { get; set; }
        public ProjectId projectId { get; set; }
        public List<FilterCondition> filters { get; set; } = [];
    }
}
