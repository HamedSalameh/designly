namespace Designly.Auth.Models
{
    public class TokenResponse : ITokenResponse
    {
        public string IdToken { get; init; } = "Not Set";
        public string RefreshToken { get; init; } = "Not Set";
        public string AccessToken { get; init; } = "Not Set";
        public int ExpiresIn { get; init; } = 0;
        public string TokenType { get; init; } = "Not Set";
    }
}
