using Designly.Filter;

namespace Projects.Application.Features.SearchProjects
{
    public class SearchProjectsRequest
    {
        public List<FilterConditionDto> filters { get; set; } = [];
    }
}
