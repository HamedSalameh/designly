using Designly.Auth.Models;

namespace Designly.Auth.Providers
{
    public interface IIdentityService
    {
        Task<bool> AddUserToGroupAsync(string email, string groupName, CancellationToken cancellation);
        Task<bool> CreateGroupAsync(string groupName, string groupDescription, CancellationToken cancellation);
        Task<bool> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken);
        Task<ITokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken);
        Task<ITokenResponse?> LoginJwtAsync(string username, string password, CancellationToken cancellationToken);
        Task<ITokenResponse?> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task<bool> SetUserPasswordAsync(string email, string password, CancellationToken cancellationToken);
        Task<bool> SignoutAsync(string accessToken, CancellationToken cancellation);
    }
}
