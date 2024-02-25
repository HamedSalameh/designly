using Designly.Base.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Threading;

namespace Designly.Base.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static DesignlyProblemDetails ToDesignlyProblemDetails(this ValidationException validationException,
            string title = "There are one or more validation errors occured",
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            ArgumentNullException.ThrowIfNull(validationException);

            var problemDetail = "See the error list for more information";

            var errors = validationException.Errors;
            // convert the dictionary of errors to a list of key value pairs
            // this is needed because the problem details extensions only accepts a list of key value pairs
            // and not a dictionary
            var errorList = errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList();

            var problemDetails = new DesignlyProblemDetails(
                title: title,
                statusCode: (int)statusCode,
                errors: errorList,
                detail: errorList.Count == 1 ? errorList[0].Value : problemDetail
                );

            return problemDetails;
        }

        public static DesignlyProblemDetails ToDesignlyProblemDetails(this BusinessLogicException businessLogicException,
            string title = "The request could not be processed",
            HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
        {
            var problemDetail = "See the error list for more information";

            ArgumentNullException.ThrowIfNull(businessLogicException);

            var errors = businessLogicException.DomainErrors;
            var errorList = new List<KeyValuePair<string, string>>(errors);

            var problemDetails = new DesignlyProblemDetails(
                title: title,
                statusCode: (int)statusCode,
                errorList,
                detail: errors.Count == 1 ? errors[0].Key : problemDetail)
            {
                // set the problem detail type as business logic exception
                Type = ProblemDetailTypes.BusinessLogicException.Name
            };

            return problemDetails;
        }

        // handle 422
        public static async Task<BusinessLogicException> HandleUnprocessableEntityResponse(this HttpResponseMessage response)
        {
            var validationFailureReason = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                var designlyProblemDetails = JsonConvert.DeserializeObject<DesignlyProblemDetails>(validationFailureReason);
                var businessLogicException = new BusinessLogicException(designlyProblemDetails?.Title);
                if (designlyProblemDetails is not null)
                {
                    // return a failed result
                    businessLogicException.DomainErrors = designlyProblemDetails.Errors;
                }
                return businessLogicException;
            }
            catch (Exception exception)
            {
                return new BusinessLogicException(exception.Message);
            }
        }

        // handle 500
        public static async Task<InternalServerErrorException> HandleInternalServerErrorResponse(this HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var validationFailureReason = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return new InternalServerErrorException(validationFailureReason);
        }

        public static async Task<UnknownServerErrorResponseException> HandleUnknownServerErrorResponse(this HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return new UnknownServerErrorResponseException($"{response.StatusCode} : {responseContent}");
        }

        public sealed record ProblemDetailType(string Name, string Title);

        // list of problem details types
        public static class ProblemDetailTypes
        {
            public static readonly ProblemDetailType BusinessLogicException = new(
                nameof(BusinessLogicException),
                "The request could not be processed");

            public static readonly ProblemDetailType ValidationException = new(
                nameof(ValidationException),
                "There are one or more validation errors occured");
        }

    }
}
