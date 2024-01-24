namespace Designly.Configuration
{
    public class AccountsApiConfiguration
    {
        public new string Position { get; } = "AccountsApiConfiguration";

        public string? BaseUrl { get; set; }
        public string? ServiceUrl { get; set; }
        public EndpointsConfiguration? Endpoints { get; set; }
    }

    public class EndpointsConfiguration
    {
        public string? Status { get; set; }
    }
}
