namespace Designly.Auth.Providers
{
    public interface ITokenProvider
    {
        Task<string?> GetAccessTokenAsync(string clientId, string clientSecret);
        Task<string?> GetAccessTokenAsync();
        Task<string?> GetTokenAsync(string clientId, string clientSecret);
    }
}
