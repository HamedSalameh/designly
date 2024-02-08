using System.Net.Mime;
using Designly.Base.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Designly.Shared.Middleware
{
    public class BusinessLogicExceptionHandler(ILogger<BusinessLogicExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<BusinessLogicExceptionHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(exception, "An exception was thrown from the endpoint.");
            }

            _logger.LogError(exception, "An exception was thrown during request processing.");

            if (exception is not BusinessLogicException businessLogicException)
            {
                // This exception was not handled, hence cannot be converted to a ProblemDetails object.
                return false;
            }
            
            var problemDetails = businessLogicException.ToDesignlyProblemDetails();
            if (problemDetails is null)
            {
                _logger.LogError("Could not parse the exception to a problem details object.");
                return false;
            }

            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken).ConfigureAwait(false);

            return true;
        }

    }
}