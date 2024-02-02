using Designly.Auth.Providers;
using Designly.Configuration;
using Designly.Shared;
using Designly.Shared.Extensions;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Projects.Domain;
using Projects.Infrastructure.Interfaces;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Projects.Application.Features.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger,
            ITokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory,
            IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Validate the customer by Id
                await ValidateClientAsync(request.TenantId, request.ClientId, cancellationToken);

                // Step 2: Validate the project lead by Id
                await ValidateProjectLeadAsync(request.TenantId, request.ProjectLeadId, cancellationToken);

                var basicProject = BasicProject.CreateBasicProject(request.TenantId, request.ProjectLeadId, request.ClientId, request.Name, 
                    request.StartDate, request.Deadline, request.CompletedAt, request.Description);
                
                var project_id = await _unitOfWork.ProjectsRepository.CreateBasicProjectAsync(basicProject, cancellationToken);
                
                _logger.LogDebug($"Created project: {basicProject.Name} ({basicProject.Id}, under account {basicProject.TenantId})");

                return project_id;
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

                if (response.IsSuccessStatusCode) return;
                // only handle business logic related problem details here
                if (!response.IsSuccessStatusCode && response.StatusCode is System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    await response.ToBusinessLogicException().ConfigureAwait(false);
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception($"Could not validate project lead with Id {projectLeadId} : {responseContent}");
            }
        }

        private async Task ValidateClientAsync(Guid tenantId, Guid clientId, CancellationToken cancellationToken)
        {
            if (clientId == Guid.Empty || clientId == default)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            using var httpClient = await CreateHttpClient(ClientsServiceConfiguration.Position);

            var response = await httpClient.GetAsync($"validate/{tenantId}/{clientId}", cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var clientStatus = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (Enum.TryParse<ClientStatusCode>(clientStatus, out var clientStatusCode) && clientStatusCode == ClientStatusCode.Active)
                {
                    return;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                await response.ToBusinessLogicException().ConfigureAwait(false);
                return;
            }

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new Exception($"Could not validate client with Id {clientId}: {responseContent}");

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
