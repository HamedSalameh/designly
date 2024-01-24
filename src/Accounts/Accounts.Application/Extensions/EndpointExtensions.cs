using Accounts.Domain;
using Designly.Shared.Exceptions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Application.Extensions
{
    public static class EndpointExtensions
    {
        private const string ValidationProblemTitle = "One or more validation errors occurred.";
        private const string AccountExceptionProblemTitle = "One or more account validation errors occurred.";
        private const string ProblemDetail = "See the errors property for details.";

        public static IResult ToActionResult<T>(this Result<T> result)
        {
            return result.Match(
                Succ: response => Results.Ok(result),
                Fail: ex =>
                {
                    IResult result = ex switch
                    {
                        ValidationException validationException => Results.Ok(validationException.ToValidationProblemDetails()),
                        AccountException accountException => Results.Ok(accountException.ToAccountProblemDetails()),
                        _ => Results.BadRequest(ex.Message)
                    };

                    return result;
                });
        }

        public static ProblemDetails ToAccountProblemDetails(this AccountException accountException)
        {
            if (accountException == null)
            {
                throw new ArgumentNullException(nameof(accountException));
            }

            var problemDetails = CreateBasicProlemDetails(AccountExceptionProblemTitle, StatusCodes.Status400BadRequest);

            var accountErrors = accountException.Errors;
            // add the errors as list of key value pairs to the extensions under 'errors'
            if (accountErrors != null)
            {
                problemDetails.Extensions.Add("Errors", accountErrors);
            }
            return problemDetails;
        }

        public static ProblemDetails ToValidationProblemDetails(this ValidationException validationException)
        {
            if (validationException == null)
            {
                throw new ArgumentNullException(nameof(validationException));
            }

            var problemDetails = CreateBasicProlemDetails(ValidationProblemTitle, StatusCodes.Status400BadRequest);
            var accountErrors = validationException.Errors;
            // add the errors as list of key value pairs to the extensions under 'errors'
            if (accountErrors != null)
            {
                problemDetails.Extensions.Add("Errors", accountErrors);
            }
            return problemDetails;
        }

        private static ProblemDetails CreateBasicProlemDetails(string title, int statusCode)
        {
            return new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = ProblemDetail
            };
        }
    }
}
