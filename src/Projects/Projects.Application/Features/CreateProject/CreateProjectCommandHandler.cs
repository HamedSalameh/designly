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
using System.Reflection.Metadata.Ecma335;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger,
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
                await ValidateProjectLeadAsync(request.TenantId, request.ProjectLeadId, cancellationToken);

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

        private async Task ValidateProjectLeadAsync(Guid tenantId, Guid projectLeadId, CancellationToken cancellationToken)
        {
            if (projectLeadId == Guid.Empty || projectLeadId == default)
            {
                throw new ArgumentNullException(nameof(projectLeadId));
            }

            using (var httpClient = await CreateHttpClient(AccountsServiceConfiguration.Position))
            {
                var response = await httpClient.GetAsync($"{tenantId}/users/{projectLeadId}/validate", cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    // Since this is a server to server call, we should only handle 422 
                    // and bad request should not be parsed here
                    if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    {
                        // read the error message
                        await ToBusinessLogicException(response).ConfigureAwait(false);
                    }

                    throw new Exception($"Could not validate project lead with Id {projectLeadId}");
                }
            }
        }

        private static async Task ToBusinessLogicException(HttpResponseMessage response)
        {
            var validationFailureReason = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var designlyProblemDetails = JsonConvert.DeserializeObject<DesignlyProblemDetails>(validationFailureReason);
            if (designlyProblemDetails is not null)
            {
                // return a failed result
                var businessLogicException = new BusinessLogicException(designlyProblemDetails.Title);
                businessLogicException.DomainErrors = designlyProblemDetails.Errors;
                throw businessLogicException;
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
}
