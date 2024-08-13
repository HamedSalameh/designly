using Designly.Filter;

namespace Accounts.Application.Features.SearchUsers
{
    public class SearchUsersRequest
    {
        public List<FilterConditionDto> filters { get; set;  } = [];
    }
}
