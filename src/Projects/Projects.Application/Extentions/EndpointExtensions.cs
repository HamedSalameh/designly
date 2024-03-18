using Designly.Base.Exceptions;
using Designly.Base.Extensions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;

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
                        _ => Results.BadRequest(ex.Message)
                    };

                    return result;
                });
        }
    }
}
