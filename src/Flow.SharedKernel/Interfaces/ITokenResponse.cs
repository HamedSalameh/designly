namespace Flow.SharedKernel.Interfaces
{
    public interface ITokenResponse
    {
        string AccessToken { get; init; }
        int ExpiresIn { get; init; }
        string IdToken { get; init; }
        string TokenType { get; init; }
    }
}