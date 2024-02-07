using System.Net.Mime;
using Designly.Base.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Designly.Shared.Middleware
{
    public class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<ValidationExceptionHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            if (exception is ValidationException validation)
            {
                var response = httpContext.Response;
                response.ContentType = MediaTypeNames.Application.Json;

                response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    Detail = "See the errors property for details.",
                    Instance = httpContext.Request.Path
                };


                if (validation.Errors.Any())
                {
                    problemDetails.Extensions["Errors"] = validation.Errors;
                }

                httpContext.Response.WriteAsJsonAsync(problemDetails);

                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }
    }
}