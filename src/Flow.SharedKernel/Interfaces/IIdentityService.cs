namespace Flow.SharedKernel.Interfaces
{
    public interface IIdentityService
    {
        Task<ITokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken);
    }
}
