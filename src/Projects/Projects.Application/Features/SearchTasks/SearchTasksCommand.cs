using Projects.Application.Filter;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTasksCommand
    {
        public TenantId tenantId { get; set; }
        public ProjectId projectId { get; set; }
        public List<FilterCondition> filters { get; set; } = [];
    }
}
