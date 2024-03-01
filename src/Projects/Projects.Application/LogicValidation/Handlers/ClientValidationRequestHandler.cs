using Designly.Base;
using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using Designly.Configuration;
using Designly.Shared.Polly;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly.Wrap;
using Projects.Application.LogicValidation.Requests;
using Projects.Application.Providers;
using Projects.Domain.StonglyTyped;
using System.Net;

namespace Projects.Application.LogicValidation.Handlers
{
    public class ClientValidationRequestHandler : IBusinessLogicValidationHandler<ClientValidationRequest>
    {
        private readonly AsyncPolicyWrap _policy;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ILogger<ClientValidationRequestHandler> _logger;
        private static readonly Error _validationFailed = new("Client Validation", "Client Validation Failed");

        public ClientValidationRequestHandler(IHttpClientProvider httpClientProvider, ILogger<ClientValidationRequestHandler> logger)
        {
            _policy = PollyPolicyFactory.WrappedNetworkRetries();
            _httpClientProvider = httpClientProvider ?? throw new ArgumentNullException(nameof(httpClientProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Exception?>? ValidateAsync(ClientValidationRequest request, CancellationToken cancellationToken)
        {
            DoValidateRequest(request);

            using var httpClient = await _httpClientProvider.CreateHttpClient(ClientsServiceConfiguration.Position); // await CreateHttpClient(ClientsServiceConfiguration.Position);
            var validationResponse = await _policy.ExecuteAsync(async () =>
            {
                return await DoClientValidation(request, httpClient, cancellationToken).ConfigureAwait(false);
            });

            return validationResponse;
        }

        private void DoValidateRequest(ClientValidationRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (request.ClientId == ClientId.Empty)
            {
                _logger.LogInformation("Invalid value for {ClientId}", nameof(request.ClientId));
                throw new ArgumentException($"Invalid value for {nameof(request.ClientId)}");
            }
            if (request.TenantId == TenantId.Empty)
            {
                _logger.LogInformation("Invalid value for {TenantId}", nameof(request.TenantId));
                throw new ArgumentException($"Invalid value for {nameof(request.TenantId)}");
            }
        }

        private static async Task<Exception?> DoClientValidation(ClientValidationRequest request, HttpClient httpClient, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"validate/{request.TenantId}/{request.ClientId}", cancellationToken).ConfigureAwait(false);

            Exception? exception = response switch
            {
                // Business logic validation succeeded
                { StatusCode: HttpStatusCode.OK } => await HandleSuccessfulValidationResponse(response, cancellationToken).ConfigureAwait(false),

                // Business logic validation failed
                { StatusCode: HttpStatusCode.UnprocessableEntity } => await HandleUnprocessableEntityResponse(response, cancellationToken),

                // Remote API returned an error
                { StatusCode: HttpStatusCode.InternalServerError } => await response.HandleInternalServerErrorResponse(cancellationToken),

                // All others
                _ => await response.HandleUnknownServerErrorResponse(cancellationToken).ConfigureAwait(false)
            };

            return exception;
        }

        private static async Task<BusinessLogicException> HandleUnprocessableEntityResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var exception = await response.HandleUnprocessableEntityResponse(cancellationToken);
            exception.DomainErrors.Add(new KeyValuePair<string, string>(_validationFailed.Code, _validationFailed.Description));
            return exception;
        }

        private static async Task<BusinessLogicException?> HandleSuccessfulValidationResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var clientStatusContentResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var clientStatus = JsonConvert.DeserializeAnonymousType(clientStatusContentResponse, new { Code = 0, Description = "" });

            if (clientStatus != null && !clientStatus.Description.Equals("Active", StringComparison.OrdinalIgnoreCase))
            {
                List<Error> errors = new()
                {
                    _validationFailed,
                    new(clientStatus.Code.ToString(), clientStatus.Description)
                }; 
                
                return new BusinessLogicException(errors);
            }
            return null;
        }
    }
}
