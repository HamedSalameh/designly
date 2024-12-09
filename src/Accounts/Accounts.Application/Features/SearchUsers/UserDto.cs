using static Accounts.Domain.Consts;

namespace Accounts.Application.Features.SearchUsers
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public Guid Account { get; set; }

        public string FirstName { get; set; } = Designly.Shared.Consts.Strings.ValueNotSet;
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public UserStatus userStatus { get; set; }
    }
}
