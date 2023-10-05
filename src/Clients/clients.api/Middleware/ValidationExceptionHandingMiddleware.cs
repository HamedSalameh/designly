using System.Net.Mime;
using System.Text.Json;
using Clients.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Middleware
{
    public class ValidationExceptionHandingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionHandingMiddleware> _logger;
        
        public ValidationExceptionHandingMiddleware(RequestDelegate next, ILogger<ValidationExceptionHandingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task Invoke(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Domain.Exceptions.ValidationException validationException)
            {
                _logger.LogError(validationException, "Validation exception occurred");
                await HandleValidationExceptionAsync(context, validationException);
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext httpContext, ValidationException validationException)
        {
            try
            {
                var response = httpContext.Response;
                response.ContentType = MediaTypeNames.Application.Json;

                response.StatusCode = StatusCodes.Status400BadRequest;
                
                var problemDetails = new ProblemDetails()
                {
                    Status =  StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    Detail = "See the errors property for details.",
                    Instance = httpContext.Request.Path
                };

                if (validationException.Errors.Any())
                {
                    problemDetails.Extensions["Errors"] = validationException.Errors;
                }
                
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                
                var errorsJson = JsonSerializer.Serialize(problemDetails, jsonSerializerOptions);
                
                await httpContext.Response.WriteAsJsonAsync(errorsJson).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception occurred while handling validation exception");
                throw;
            }
        }
    }
}