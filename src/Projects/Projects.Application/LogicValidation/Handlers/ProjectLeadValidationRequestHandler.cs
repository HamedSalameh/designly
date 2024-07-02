using Designly.Base;
using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using Designly.Configuration;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Polly.Wrap;
using Projects.Application.LogicValidation.Requests;
using Projects.Application.Providers;
using Projects.Domain.StonglyTyped;
using System.Net;

namespace Projects.Application.LogicValidation.Handlers
{
    public class ProjectLeadValidationRequestHandler : IBusinessLogicValidationHandler<ProjectLeadValidationRequest>
    {
        private readonly AsyncPolicyWrap _policy;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ILogger<ProjectLeadValidationRequestHandler> _logger;
        private static readonly Error _validationFailed = new("Business Logic Validation Failed", "Project Lead");

        public ProjectLeadValidationRequestHandler(ILogger<ProjectLeadValidationRequestHandler> logger, IHttpClientProvider httpClientProvider)
        {
            _policy = PollyPolicyFactory.WrappedNetworkRetries(logger);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientProvider = httpClientProvider ?? throw new ArgumentNullException(nameof(httpClientProvider));
        }

        public async Task<Exception?> ValidateAsync(ProjectLeadValidationRequest request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Validating project lead {ProjectLeadId} for tenant {TenantId}", request.ProjectLeadId, request.TenantId);
            }
            
            if (request.ProjectLeadId == ProjectLeadId.Empty)
            {
                _logger.LogError("Invalid value for {ProjectLeadId}", nameof(request.ProjectLeadId));
                throw new ArgumentNullException(nameof(request.ProjectLeadId));
            }
            
            if (request.TenantId == TenantId.Empty)
            {
                _logger.LogError("Invalid value for {TenantId}", nameof(request.TenantId));
                throw new ArgumentNullException(nameof(request.TenantId));
            }

            using var httpClient = await _httpClientProvider.CreateHttpClient(AccountsServiceConfiguration.Position);
            var validationResponse = await _policy.ExecuteAsync(async () =>
            {
                return await DoProjectLeadValidation(request, httpClient, cancellationToken).ConfigureAwait(false);
            });

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Project lead {ProjectLeadId} for tenant {TenantId} validation response: {ValidationResponse}", request.ProjectLeadId, request.TenantId, validationResponse);
            }

            return validationResponse;
        }

        private static async Task<Exception?> DoProjectLeadValidation(ProjectLeadValidationRequest request, HttpClient httpClient, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync($"{request.TenantId}/users/{request.ProjectLeadId}/validate", cancellationToken).ConfigureAwait(false);

            Exception? exception = response switch
            {
                // Business logic validation succeeded
                { StatusCode: HttpStatusCode.OK } => null,

                // Business logic validation failed
                { StatusCode: HttpStatusCode.UnprocessableEntity } => await HandleUnprocessableEntityResponse(response, cancellationToken),

                // Remote API returned an error
                { StatusCode: HttpStatusCode.InternalServerError } => await response.HandleInternalServerErrorResponse(cancellationToken),

                // all other status codes
                _ => await response.HandleUnknownServerErrorResponse(cancellationToken)
            };

            return exception;
        }
        private static async Task<BusinessLogicException> HandleUnprocessableEntityResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var exception = await response.HandleUnprocessableEntityResponse(cancellationToken);
            exception.DomainErrors.Add(new KeyValuePair<string, string>(_validationFailed.Code, _validationFailed.Description));
            return exception;
        }
    }
}
