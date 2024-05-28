using Designly.Base;
using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Projects.Application.Extentions
{
    public static class EndpointExtensions
    {
        public static IResult ToActionResult<T>(this Result<T> result, Func<T, IResult>? SuccessHandler)
        {
            return result.Match(
                Succ: SuccessHandler,
                Fail: ex =>
                {
                    IResult result = ex switch
                    {
                        ValidationException validationException => Results.BadRequest(validationException.ToDesignlyProblemDetails()),
                        BusinessLogicException businessLogicException => Results.UnprocessableEntity(businessLogicException.ToDesignlyProblemDetails()),
                        Exception exception => CreateInternalServerError(exception),
                        _ => Results.BadRequest(ex.Message)
                    };

                    return result;
                });
        }

        private static IResult CreateInternalServerError(Exception exception)
        {
            var problemDetail = "See the error list for more information";

            ArgumentNullException.ThrowIfNull(exception);

            var problemDetails = new DesignlyProblemDetails(
                title: problemDetail,
                statusCode: (int)HttpStatusCode.InternalServerError,
                detail: exception.Message
                );

            // return the respone
            return Results.Problem(problemDetails);

        }
    }
}
