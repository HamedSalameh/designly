namespace Designly.Auth.Models
{
    /// <summary>
    /// Holds the configuration for OAuth2 authentication flow with the IDP
    /// </summary>
    public sealed class OAuth2ServiceProviderConfiguration
    {
        public static readonly string Position = "OAuth2ServiceProviderConfiguration";
        
        public readonly string grant_type = "client_credentials";
        public string? client_id { get; set; }
        public string? client_secret { get; set; }
        public string? authorization_endpoint { get; set; }
        public string? token_endpoint { get; set; }
    }
}
