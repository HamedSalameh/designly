using Designly.Shared.Exceptions;
using Newtonsoft.Json;
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

            // set the problem detail type as business logic exception
            problemDetails.Type = ProblemDetailTypes.BusinessLogicException.name;

            return problemDetails;
        }

        /// <summary>
        /// Creates a new business logic exception from a problem details in an http reponse
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task ToBusinessLogicException(this HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var validationFailureReason = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var designlyProblemDetails = JsonConvert.DeserializeObject<DesignlyProblemDetails>(validationFailureReason);
            if (designlyProblemDetails is not null)
            {
                // return a failed result
                var businessLogicException = new BusinessLogicException(designlyProblemDetails.Title);
                businessLogicException.DomainErrors = designlyProblemDetails.Errors;
                throw businessLogicException;
            }
            throw new Exception("Could not parse the exception to a problem details object.");
        }

        public sealed record ProblemDetailType(string name, string title);

        // list of problem details types
        public static class ProblemDetailTypes
        {
            public static ProblemDetailType BusinessLogicException = new ProblemDetailType(
                nameof(BusinessLogicException),
                "The request could not be processed");

            public static ProblemDetailType ValidationException = new ProblemDetailType(
                nameof(ValidationException),
                "There are one or more validation errors occured");
        }
    }
}
