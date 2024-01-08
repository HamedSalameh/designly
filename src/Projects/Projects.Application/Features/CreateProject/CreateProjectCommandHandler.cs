using Designly.Auth.Providers;
using Designly.Shared;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger, ITokenProvider tokenProvider, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory)) ;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Validate the customer by Id
                var validatedClient = await ValidateClientAsync(request.ClientId, cancellationToken);
                if (!validatedClient)
                {
                    _logger.LogError("Could not find client with Id: {clientId}", request.ClientId);
                    throw new ArgumentException($"Could not find client with Id: {request.ClientId}");
                }

                // Step 2: Validate the project lead by Id
                var validatedProjectLead = await ValidateProjectLeadAsync(request.ProjectLeadId, cancellationToken);
                if (!validatedProjectLead )
                {
                    _logger.LogError("Could not find project lead with Id: {projectLeadId}", request.ProjectLeadId);
                    throw new ArgumentException($"Could not find project lead with Id: {request.ProjectLeadId}");
                }

                var projectId = Guid.NewGuid();
                // var projectId = await _unitOfWork.ClientsRepository.CreateClientAsyncWithDapper(client, cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Created project: {clientId}", projectId);

                return await Task.FromResult(projectId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exception.Message}", exception.Message);
                throw;
            }
        }

        private async Task<bool> ValidateProjectLeadAsync(Guid projectLeadId, CancellationToken cancellationToken)
        {
            if (projectLeadId != Guid.Empty && projectLeadId != default)
            {
                
            }
            return await Task.FromResult(false);
        }

        private async Task<bool> ValidateClientAsync(Guid clientId, CancellationToken cancellationToken)
        {
            if (clientId != Guid.Empty && clientId != default)
            {
                // Use httpclientfactory to create a client
                var client = _httpClientFactory.CreateClient();
                var clientServiceAddress = "https://localhost:7246";
                var clientServiceEndpoint = $"api/v1/clients/{clientId}";
                var clientServiceUrl = $"{clientServiceAddress}/{clientServiceEndpoint}";

                await AddAuthentication(client).ConfigureAwait(false);

                var response = await client.GetAsync(clientServiceUrl, cancellationToken).ConfigureAwait(false);

                return response.IsSuccessStatusCode;
            }
            return await Task.FromResult(false);
        }

        private async Task AddAuthentication(HttpClient client)
        {
            // TODO: Get clientId and clientSecret from configuration
            var accessToken = await _tokenProvider.GetAccessTokenAsync("35pjbh9a429lu7uepb8b0cv4br", "1m6t5aqj45jf6fcluce9pkrgk8nbnl1inien8dqjdsb6dvhltd98").ConfigureAwait(false);
             var authenticationHeaderValue = new AuthenticationHeaderValue(
                Designly.Auth.Consts.BearerAuthenicationScheme, accessToken);
            client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            client.DefaultRequestHeaders.Add(Consts.ApiVersionHeaderEntry, "1.0"); // TODO: Get from configuration
        }
    }
}
