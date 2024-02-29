using Amazon.Runtime.Internal.Util;
using Designly.Auth.Providers;
using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using Designly.Configuration;
using Designly.Shared.Polly;
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

        public ClientValidationRequestHandler(IHttpClientProvider httpClientProvider)
        {
            _policy = PollyPolicyFactory.WrappedNetworkRetries();
            _httpClientProvider = httpClientProvider;
        }

        public async Task<Exception?>? ValidateAsync(ClientValidationRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (request.ClientId == ClientId.Empty)
            {
                throw new ArgumentException($"Invalid value for {nameof(request.ClientId)}");
            }
            if (request.TenantId == TenantId.Empty)
            {
                throw new ArgumentException($"Invalid value for {nameof(request.TenantId)}");
            }

            using var httpClient = await _httpClientProvider.CreateHttpClient(ClientsServiceConfiguration.Position); // await CreateHttpClient(ClientsServiceConfiguration.Position);
            var validationResponse = await _policy.ExecuteAsync(async () =>
            {
                return await DoClientValidation(request, httpClient, cancellationToken).ConfigureAwait(false);
            });

            return validationResponse;
        }

        private static async Task<Exception?> DoClientValidation(ClientValidationRequest request, HttpClient httpClient, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"validate/{request.TenantId}/{request.ClientId}", cancellationToken).ConfigureAwait(false);

            Exception? exception = response switch
            {
                // Business logic validation succeeded
                { StatusCode: HttpStatusCode.OK } => await handleSuccessfulValidationResponse(response, cancellationToken).ConfigureAwait(false),

                // Business logic validation failed
                { StatusCode: HttpStatusCode.UnprocessableEntity } => await response.HandleUnprocessableEntityResponse(),

                // Remote API returned an error
                { StatusCode: HttpStatusCode.InternalServerError } => await response.HandleInternalServerErrorResponse(cancellationToken),

                // All others
                _ => await response.HandleUnknownServerErrorResponse(cancellationToken).ConfigureAwait(false)
            };

            return exception;
        }

        private static async Task<BusinessLogicException?> handleSuccessfulValidationResponse(HttpResponseMessage response, CancellationToken cancellationToken)
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
    }
}
