using Designly.Auth.Identity;
using Designly.Auth.Providers;
using Designly.Shared;
using Designly.Shared.Exceptions;
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
        private readonly ITenantProvider _authorizationProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger, ITokenProvider tokenProvider, IHttpClientFactory httpClientFactory, ITenantProvider authorizationProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _authorizationProvider = authorizationProvider ?? throw new ArgumentNullException(nameof(authorizationProvider));
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Validate the customer by Id
                var validatedClient = await ValidateClientAsync(request.TenantId, request.ClientId, cancellationToken);
                if (!validatedClient)
                {
                    _logger.LogError("Client validation for {clientId} failed", request.ClientId);
                   
                    throw new BusinessLogicException("ClientId", $"Client with Id {request.ClientId} is not a valid client.");
                }

                // Step 2: Validate the project lead by Id
                var validatedProjectLead = await ValidateProjectLeadAsync(request.ProjectLeadId, cancellationToken);
                if (!validatedProjectLead )
                {
                    _logger.LogError("Could not find project lead with Id: {projectLeadId}", request.ProjectLeadId);
                    throw new BusinessLogicException("ProjectLeadId", $"Project lead with Id {request.ProjectLeadId} is not a valid project lead.");
                }

                var projectId = Guid.NewGuid();
                // var projectId = await _unitOfWork.ClientsRepository.CreateClientAsyncWithDapper(client, cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Created project: {clientId}", projectId);

                return await Task.FromResult(projectId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create new project due to error: {exceptionType}: {exception.Message}", exception.GetType().Name, exception.Message);
                throw;
            }
        }

        private async Task<bool> ValidateProjectLeadAsync(Guid projectLeadId, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Project lead validation is not implemented yet");
            
            return await Task.FromResult(true);
        }

        private async Task<bool> ValidateClientAsync(Guid tenantId, Guid clientId, CancellationToken cancellationToken)
        {
            if (clientId != Guid.Empty && clientId != default)
            {
                // Use httpclientfactory to create a client
                var client = _httpClientFactory.CreateClient();
                var clientServiceAddress = "https://localhost:7246";
                var clientServiceEndpoint = $"api/v1/clients/status/{tenantId}/{clientId}";
                var clientServiceUrl = $"{clientServiceAddress}/{clientServiceEndpoint}";

                await AddAuthentication(client).ConfigureAwait(false);

                var response = await client.GetAsync(clientServiceUrl, cancellationToken).ConfigureAwait(false);

                var clientStatus = await response.Content.ReadAsStringAsync();
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Client status for {clientId} in tenant {tenantId}: {clientStatus}", clientId, tenantId, clientStatus);
                }
                // try parse client status as enum
                Enum.TryParse<ClientStatusCode>(clientStatus, out var clientStatusCode);

                var clientValidationResult = response.IsSuccessStatusCode && clientStatusCode == ClientStatusCode.Active;
                
                return clientValidationResult;
            }
            return await Task.FromResult(false);
        }

        private async Task AddAuthentication(HttpClient client)
        {
            // TODO: Get clientId and clientSecret from configuration
            var accessToken = await _tokenProvider.GetAccessTokenAsync("5jbktc23rqr59etq1kgeq5s6ms", "mmhvko32k47d50ik6hfqlomref62gvegvntp8g0v2qq0j40671v").ConfigureAwait(false);
            var authenticationHeaderValue = new AuthenticationHeaderValue(
                Designly.Auth.Consts.BearerAuthenicationScheme, accessToken);
            client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            client.DefaultRequestHeaders.Add(Consts.ApiVersionHeaderEntry, "1.0"); // TODO: Get from configuration
        }
    }
}
