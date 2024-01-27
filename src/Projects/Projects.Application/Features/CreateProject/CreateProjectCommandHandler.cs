using Designly.Auth.Identity;
using Designly.Auth.Providers;
using Designly.Configuration;
using Designly.Shared;
using Designly.Shared.Exceptions;
using LanguageExt.Common;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger,
            IOptions<AccountsServiceConfiguration> accountsApiConfig,
            ITokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                var validatedProjectLead = await ValidateProjectLeadAsync(request.TenantId, request.ProjectLeadId, cancellationToken);
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

        private async Task<bool> ValidateProjectLeadAsync(Guid tenantId, Guid projectLeadId, CancellationToken cancellationToken)
        {
            if (projectLeadId == Guid.Empty || projectLeadId == default)
            {
                return false;
            }

            using (var httpClient = await CreateHttpClient(AccountsServiceConfiguration.Position))
            {
                var response = await httpClient.GetAsync($"{tenantId}/users/{projectLeadId}/validate", cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        // read the error message
                        var validationFailureReason = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return false;
                    }

                    throw new Exception($"Could not validate project lead with Id {projectLeadId}");
                }

                var projectLeadStatus = await response.Content.ReadAsStringAsync();

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Project lead status for {projectLeadId}: {projectLeadStatus}", projectLeadId, projectLeadStatus);
                }

                return response.IsSuccessStatusCode;
            }
        }

        private async Task<bool> ValidateClientAsync(Guid tenantId, Guid clientId, CancellationToken cancellationToken)
        {
            if (clientId == Guid.Empty || clientId == default)
            {
                return false;
            }

            using (var httpClient = await CreateHttpClient(ClientsServiceConfiguration.Position))
            {
                var response = await httpClient.GetAsync($"validate/{tenantId}/{clientId}", cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Could not validate client with Id {clientId} in tenant {tenantId}");
                }

                var clientStatus = await response.Content.ReadAsStringAsync();


                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Client status for {clientId} in tenant {tenantId}: {clientStatus}", clientId, tenantId, clientStatus);
                }

                Enum.TryParse<ClientStatusCode>(clientStatus, out var clientStatusCode);
                return response.IsSuccessStatusCode && clientStatusCode == ClientStatusCode.Active;
            }
        }

        private async Task<HttpClient> CreateHttpClient(string configuration)
        {
            var client = _httpClientFactory.CreateClient(configuration);

            await AddAuthentication(client).ConfigureAwait(false);

            return client;
        }

        private async Task AddAuthentication(HttpClient client)
        {
            var accessToken = await _tokenProvider.GetAccessTokenAsync().ConfigureAwait(false);
            var authenticationHeaderValue = new AuthenticationHeaderValue(Designly.Auth.Consts.BearerAuthenicationScheme, accessToken);

            client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            client.DefaultRequestHeaders.Add(Consts.ApiVersionHeaderEntry, "1.0"); // TODO: Get from configuration
        }

    }


    public class ProblemDetailsWithErrors
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }

}
