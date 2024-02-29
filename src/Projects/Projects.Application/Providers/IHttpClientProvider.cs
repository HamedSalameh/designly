
namespace Projects.Application.Providers
{
    public interface IHttpClientProvider
    {
        Task<HttpClient> CreateHttpClient(string configuration);
    }
}