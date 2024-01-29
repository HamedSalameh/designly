namespace Designly.Auth.Providers
{
    public interface ITokenProvider
    {
        Task<string?> GetAccessTokenAsync(string clientId, string clientSecret);
        Task<string?> GetAccessTokenAsync();
    }
}
