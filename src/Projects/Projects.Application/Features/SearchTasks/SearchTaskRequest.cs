using Designly.Filter;
using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTaskRequest
    {
        required public ProjectId projectId { get; set; }
        required public List<FilterConditionDto> filters { get; set; } = [];
    }
}
