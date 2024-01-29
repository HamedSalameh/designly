namespace Designly.Auth.Models
{
    public class OAuth2ServiceProviderConfiguration
    {
        public const string Position = "OAuth2ServiceProviderConfiguration";
        
        public string grant_type = "client_credentials";
        public string? client_id { get; set; }
        public string? client_secret { get; set; }
        public string? authorization_endpoint { get; set; }
        public string? token_endpoint { get; set; }

    }
}
