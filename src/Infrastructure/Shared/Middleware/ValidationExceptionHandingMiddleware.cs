using System.Net.Mime;
using Designly.Base.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Designly.Shared.Middleware
{
    public class ValidationExceptionHandler() : IExceptionHandler
    {
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

                httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }
    }
}