using Accounts.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Extensions
{
    public class AccountExceptionsHandler(ILogger<AccountExceptionsHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<AccountExceptionsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(exception, "An exception was thrown from the endpoint.");
            }

            _logger.LogError(exception, "An exception was thrown during request processing.");

            if (exception is not AccountException accountException)
            {
                // This exception was not handled, hence cannot be converted to a ProblemDetails object.
                return false;
            }

            var problemDetails = new ProblemDetails
            {
                Title = accountException.Message,
                Status = StatusCodes.Status400BadRequest,
                Detail = "TODO",
                Type = $"https://httpstatuses.com/{StatusCodes.Status400BadRequest}",
            };

            // parse the account error object
            var accountErrors = accountException.Errors;
            if (accountErrors != null)
            {
                // iterate through the errors and add them to the problem details
                foreach (var error in accountErrors)
                {
                    problemDetails.Extensions.Add(error.Code, error.Description);
                }
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            return true;
        }
    }

}
