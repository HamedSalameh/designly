using Designly.Auth.Providers;
using Designly.Base;
using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using Designly.Configuration;
using Designly.Shared;
using Designly.Shared.Polly;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly.Wrap;
using Projects.Application.Builders;
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
        private readonly IProjectBuilder _projectBuilder;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AsyncPolicyWrap _policy;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger,
            ITokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory,
            IProjectBuilder projectBuilder,
            IUnitOfWork unitOfWork,
            AsyncPolicyWrap policy)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _projectBuilder = projectBuilder ?? throw new ArgumentNullException(nameof(projectBuilder));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _policy = PollyPolicyFactory.WrappedNetworkRetries();
        }

        public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling request {CreateProjectCommandHandler} for {request.Name}", nameof(CreateProjectCommandHandler), request.Name);
            }

            try
            {
                // Step 1: Validate the customer by Id
                var clientValidationResult = await ValidateClientAsync(request.TenantId, request.ClientId, cancellationToken);
                if (clientValidationResult != null)
                {
                    return new Result<Guid>(clientValidationResult);
                }

                // Step 2: Validate the project lead by Id
                var projectLeadValidationResult = await ValidateProjectLeadAsync(request.TenantId, request.ProjectLeadId, cancellationToken);
                if (projectLeadValidationResult != null)
                {
                    return new Result<Guid>(projectLeadValidationResult);
                }

                var projectBuilder = _projectBuilder
                    .WithProjectLead(request.ProjectLeadId)
                    .WithClient(request.ClientId)
                    .WithName(request.Name)
                    .WithDescription(request.Description)
                    .WithStartDate(request.StartDate)
                    .WithDeadline(request.Deadline)
                    .WithCompletedAt(request.CompletedAt);
                    
                var basicProject = projectBuilder.BuildBasicProject();

                var project_id = await _unitOfWork.ProjectsRepository.CreateBasicProjectAsync(basicProject, cancellationToken);
                
                _logger.LogDebug("Created project: {basicProject.Name} ({basicProject.Id}, under account {basicProject.TenantId})", 
                    basicProject.Name, basicProject.Id, basicProject.TenantId);

                return project_id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create project due to error : {ex.Message}", ex.Message);
                throw;
            }
        }

        private async Task<Exception?> ValidateProjectLeadAsync(Guid tenantId, Guid projectLeadId, CancellationToken cancellationToken)
        {
            if (projectLeadId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(projectLeadId));
            }

            using var httpClient = await CreateHttpClient(AccountsServiceConfiguration.Position);
            var validationResponse = _policy.ExecuteAsync(async () =>
            {
                var response = await httpClient.GetAsync($"{tenantId}/users/{projectLeadId}/validate", cancellationToken).ConfigureAwait(false);

                if (response.IsSuccessStatusCode) return null;
                // only handle business logic related problem details here
                if (!response.IsSuccessStatusCode && response.StatusCode is System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    return await response.HandleUnprocessableEntityResponse();
                }

                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return new Exception($"Could not validate project lead with Id {projectLeadId} : {responseContent}");
            });
            
            return await validationResponse;
        }

        private async Task<Exception?> ValidateClientAsync(Guid tenantId, Guid clientId, CancellationToken cancellationToken)
        {
            if (clientId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            using var httpClient = await CreateHttpClient(ClientsServiceConfiguration.Position);
            var validationReponse = _policy.ExecuteAsync(async () =>
            {
                HttpResponseMessage response = await httpClient.GetAsync($"validate/{tenantId}/{clientId}", cancellationToken).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return await handleSuccessfulValidationResponse(response, cancellationToken).ConfigureAwait(false);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    return await response.HandleUnprocessableEntityResponse();
                }

                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return new Exception($"Could not validate client with Id {clientId}: {responseContent}");
            });
            
            return await validationReponse;
        }

        private static async Task<Exception?> handleSuccessfulValidationResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var clientStatusContentResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var clientStatus = JsonConvert.DeserializeAnonymousType(clientStatusContentResponse, new { Code = 0, Description = "" });

            if (clientStatus != null && !clientStatus.Description.Equals("Active", StringComparison.OrdinalIgnoreCase))
            {
                Designly.Base.Error clientStatusError = new(clientStatus.Code.ToString(), clientStatus.Description);
                return new BusinessLogicException(clientStatusError);
            }
            return null;
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
