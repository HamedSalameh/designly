using Accounts.Domain;
using Designly.Base;
using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Accounts.Application.Extensions
{

    public static class EndpointExtensions
    {
        public static IResult ToActionResult<T>(this Result<T> result)
        {
            return result.Match(
                Succ: response => Results.Ok(response),
                Fail: ex =>
                {
                    IResult result = ex switch
                    {
                        ValidationException validationException => Results.BadRequest(validationException.ToDesignlyProblemDetails()),
                        AccountException accountException => Results.UnprocessableEntity(accountException.ToDesignlyProblemDetails()),
                        BusinessLogicException businessLogicException => Results.UnprocessableEntity(businessLogicException.ToDesignlyProblemDetails()),
                        _ => Results.BadRequest(ex.Message)
                    };

                    return result;
                });
        }

        public static DesignlyProblemDetails ToDesignlyProblemDetails(this AccountException accountException,
            string title = "The request could not be processed",
            HttpStatusCode? statusCode = HttpStatusCode.UnprocessableEntity)
        {
            var problemDetail = "See the error list for more information";

            ArgumentNullException.ThrowIfNull(accountException);

            var errors = accountException.Errors;

            var problemDetails = new DesignlyProblemDetails(
                title: title,
                statusCode: statusCode.HasValue ? (int)statusCode.Value :  (int)HttpStatusCode.UnprocessableEntity,
                errors,
                detail: errors.Count == 1 ? errors[0].Description : problemDetail
                );

            return problemDetails;
        }
    }
}
