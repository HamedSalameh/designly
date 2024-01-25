using Designly.Auth.Identity;
using Designly.Auth.Providers;
using Designly.Configuration;
using Designly.Shared;
using Designly.Shared.Exceptions;
using LanguageExt.Common;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;
        private readonly IOptions<AccountsApiConfiguration> _accountApiConfiguration;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger,
            IOptions<AccountsApiConfiguration> accountApiConfiguration,
            ITokenProvider tokenProvider, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountApiConfiguration = accountApiConfiguration ?? throw new ArgumentNullException(nameof(accountApiConfiguration));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Validate the customer by Id
                var validatedClient = await ValidateClientAsync(request.TenantId, request.ClientId, cancellationToken);
                if (!validatedClient)
                {
                    _logger.LogError("Client validation for {clientId} failed", request.ClientId);

                    var businessLogicError = new BusinessLogicException("ClientId", $"Client with Id {request.ClientId} is not a valid client.");
                    return new Result<Guid>(businessLogicError);
                }

                // Step 2: Validate the project lead by Id
                var validatedProjectLead = await ValidateProjectLeadAsync(request.ProjectLeadId, cancellationToken);
                if (!validatedProjectLead)
                {
                    _logger.LogError("Could not find project lead with Id: {projectLeadId}", request.ProjectLeadId);
                    var businessLogicError = new BusinessLogicException("ProjectLeadId", $"Project lead with Id {request.ProjectLeadId} is not a valid project lead.");
                    return new Result<Guid>(businessLogicError);
                }

                var projectId = Guid.NewGuid();
                // var projectId = await _unitOfWork.ClientsRepository.CreateClientAsyncWithDapper(client, cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Created project: {clientId}", projectId);

                return await Task.FromResult(projectId);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                _logger.LogError(ex, $"Could not create project due to error : {ex.Message}");
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
            if (clientId == Guid.Empty || clientId == default)
            {
                return false;
            }

            using (var client = await CreateHttpClient())
            {
                var response = await client.GetAsync($"status/{tenantId}/{client}", cancellationToken).ConfigureAwait(false);

                var clientStatus = await response.Content.ReadAsStringAsync();

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Client status for {clientId} in tenant {tenantId}: {clientStatus}", clientId, tenantId, clientStatus);
                }

                Enum.TryParse<ClientStatusCode>(clientStatus, out var clientStatusCode);
                return response.IsSuccessStatusCode && clientStatusCode == ClientStatusCode.Active;
            }
        }

        private string GetAccountServiceUri(string endpoint)
        {
            var clientServiceAddress = _accountApiConfiguration.Value.BaseUrl;
            var serviceUrl = _accountApiConfiguration.Value.ServiceUrl;
            var clientServiceEndpoint = _accountApiConfiguration.Value.Endpoints?.Status;

            return $"{clientServiceAddress}/{serviceUrl}/{clientServiceEndpoint}";
        }

        private async Task<HttpClient> CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient(nameof(AccountsApiConfiguration));

            await AddAuthentication(client).ConfigureAwait(false);

            return client;
        }

        private async Task AddAuthentication(HttpClient client)
        {
            var accessToken = await _tokenProvider.GetAccessTokenAsync("5jbktc23rqr59etq1kgeq5s6ms", "mmhvko32k47d50ik6hfqlomref62gvegvntp8g0v2qq0j40671v").ConfigureAwait(false);
            var authenticationHeaderValue = new AuthenticationHeaderValue(Designly.Auth.Consts.BearerAuthenicationScheme, accessToken);

            client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            client.DefaultRequestHeaders.Add(Consts.ApiVersionHeaderEntry, "1.0"); // TODO: Get from configuration
        }

    }
}
