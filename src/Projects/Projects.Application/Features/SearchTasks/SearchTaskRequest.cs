using Projects.Domain.StonglyTyped;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTaskRequest
    {
        public ProjectId projectId { get; set; }
        public List<FilterConditionDto> filters { get; set; } = [];
    }
}
