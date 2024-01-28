using Designly.Shared.Exceptions;
using System.Net;

namespace Designly.Shared.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static DesignlyProblemDetails ToDesignlyProblemDetails(this ValidationException validationException,
            string title = "There are one or more validation errors occured",
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (validationException == null)
            {
                throw new ArgumentNullException(nameof(validationException));
            }
            var problemDetail = "See the error list for more information";

            var errors = validationException.Errors;
            // convert the dictionary of errors to a list of key value pairs
            // this is needed because the problem details extensions only accepts a list of key value pairs
            // and not a dictionary
            var errorList = errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList();

            var problemDetails = new DesignlyProblemDetails(
                title: title,
            statusCode: (int)statusCode,
                detail: errorList.Count == 1 ? errorList[0].Value : problemDetail,
                errorList);

            return problemDetails;
        }

        public static DesignlyProblemDetails ToDesignlyProblemDetails(this BusinessLogicException businessLogicException,
            string title = "The request could not be processed",
            HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
        {
            var problemDetail = "See the error list for more information";

            if (businessLogicException == null)
            {
                throw new ArgumentNullException(nameof(businessLogicException));
            }

            var errors = businessLogicException.DomainErrors;
            var errorList = new List<KeyValuePair<string, string>>(errors);

            var problemDetails = new DesignlyProblemDetails(
                title: title,
                statusCode: (int)statusCode,
                detail: errors.Count == 1 ? errors[0].Key : problemDetail,
                errorList);

            return problemDetails;
        }
    }
}
