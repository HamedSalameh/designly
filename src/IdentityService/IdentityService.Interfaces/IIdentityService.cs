namespace IdentityService.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> AddUserToGroup(string email, string groupName, CancellationToken cancellation);
        Task<bool> CreateGroup(string groupName, CancellationToken cancellation);
        Task<bool> CreateUser(string email, string firstName, string lastName, CancellationToken cancellationToken);
        Task<ITokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken);
        Task<ITokenResponse?> LoginJwtAsync(string username, string password, CancellationToken cancellationToken);
        Task<ITokenResponse?> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task<bool> SignoutAsync(string accessToken, CancellationToken cancellation);
    }
}
