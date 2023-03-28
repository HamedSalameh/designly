using Flow.SharedKernel.Interfaces;

namespace Flow.SharedKernel.Models
{
    public class TokenResponse : ITokenResponse
    {
        public string IdToken { get; init; }
        public string RefreshToken { get; init; }
        public string AccessToken { get; init; }
        public int ExpiresIn { get; init; }
        public string TokenType { get; init; }
    }
}
