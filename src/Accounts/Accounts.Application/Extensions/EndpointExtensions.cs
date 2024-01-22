using Accounts.Domain;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Application.Extensions
{
    public static class EndpointExtensions
    {
        private const string ProblemTitle = "One or more validation errors occurred.";
        private const string ProblemDetail = "See the errors property for details.";

        public static IActionResult ToActionResult(this Result<Guid> result)
        {
            return result.Match<IActionResult>(
                Succ: id => new OkObjectResult(id),
                Fail: ex =>
                {
                    if (ex is AccountException accountException)
                    {
                        return new BadRequestObjectResult(accountException.ToAccountProblemDetails());
                    }

                    return new BadRequestObjectResult(ex.Message);
                });
        }

        public static ProblemDetails ToAccountProblemDetails(this AccountException accountException)
        {
            if (accountException == null)
            {
                throw new ArgumentNullException(nameof(accountException));
            }

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = ProblemTitle,
                Detail = ProblemDetail,
            };

            var accountErrors = accountException.Errors;
            // add the errors as list of key value pairs to the extensions under 'errors'
            if (accountErrors != null)
            {
                problemDetails.Extensions.Add("Errors", accountErrors);
            }
            return problemDetails;
        }
    }
}
