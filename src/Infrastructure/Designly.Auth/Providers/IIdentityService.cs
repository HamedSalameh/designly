using Designly.Auth.Models;
using LanguageExt.Common;

namespace Designly.Auth.Providers
{
    public interface IIdentityService
    {
        Task<bool> AddUserToGroupAsync(string email, string groupName, CancellationToken cancellationToken);
        Task<bool> CreateGroupAsync(string groupName, string groupDescription, CancellationToken cancellationToken);
        Task<bool> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken);
        Task<ITokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken);
        Task<Result<ITokenResponse?>> LoginJwtAsync(string username, string password, CancellationToken cancellationToken);
        Task<ITokenResponse?> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task<bool> SetUserPasswordAsync(string email, string password, CancellationToken cancellationToken);
        Task<bool> SignoutAsync(string accessToken, CancellationToken cancellationToken);
    }
}
