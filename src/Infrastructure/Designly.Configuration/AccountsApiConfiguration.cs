namespace Designly.Configuration
{
    public class AccountsApiConfiguration : IApiServiceConfiguration
    {
        public string Position { get; } = "AccountsApiConfiguration";

        public string? ServiceName { get; set; }
        public string? BaseUrl { get; set; }
        public string? ServiceUrl { get; set; }
        public EndpointsConfiguration? Endpoints { get; set; }
        
    }

    public interface IApiServiceConfiguration
    {
        string Position { get; }

        string? ServiceName { get; set; }
        string? BaseUrl { get; set; }
        string? ServiceUrl { get; set; }
        EndpointsConfiguration? Endpoints { get; set; }
    }

    public class EndpointsConfiguration
    {
        public string? Status { get; set; }
    }
}
