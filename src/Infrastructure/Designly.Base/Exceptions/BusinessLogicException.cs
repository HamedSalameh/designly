namespace Designly.Base.Exceptions
{
    /// <summary>
    /// A business logic exception indicates that the request is valid but the business logic has failed.
    /// </summary>
    public class BusinessLogicException : Exception
    {
        private const string OneOrMoreBusinessLogicFailures = "One or more business logic failures have occurred.";

        public BusinessLogicException(string? message = OneOrMoreBusinessLogicFailures)
            : base(message)
        {
            Errors = new Dictionary<string, string>();
            DomainErrors = new List<KeyValuePair<string, string>>();
        }

        public BusinessLogicException(string message, Exception inner)
            : base(message, inner)
        {
            Errors = new Dictionary<string, string>();
            DomainErrors = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="BusinessLogicException"/> with a one or more errors.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="message"></param>
        public BusinessLogicException(IDictionary<string, string> errors, string message = OneOrMoreBusinessLogicFailures) : this(
                       message)
        {
            Errors = errors;
            // convery dictionary to list of key value pairs
            DomainErrors = errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList();
        }

        /// <summary>
        /// Creates a new instance of <see cref="BusinessLogicException"/> with a single error.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        public BusinessLogicException(KeyValuePair<string, string> error, string message = OneOrMoreBusinessLogicFailures) : this(
                                  message)
        {
            Errors = new Dictionary<string, string>
            {
                { error.Key, error.Value }
            };

            DomainErrors = new List<KeyValuePair<string, string>>
            {
                error
            };
        }

        /// <summary>
        /// Creates a new instance of <see cref="BusinessLogicException"/> with a single error.
        /// </summary>
        /// <param name="key">The property that failed the business logic validation</param>
        /// <param name="keyValidationMessage">Description of the business logic failure and reason</param>
        /// <param name="message"></param>
        public BusinessLogicException(string key, string keyValidationMessage, string message = OneOrMoreBusinessLogicFailures) : this(
                                             message)
        {
            Errors = new Dictionary<string, string>
            {
                { key, keyValidationMessage }
            };
        }

        public BusinessLogicException(Error error, string message = OneOrMoreBusinessLogicFailures) : this(
                                                        message)
        {
            DomainErrors = new List<KeyValuePair<string, string>>
            {
                new(error.Code, error.Description)
            };
        }

        public BusinessLogicException(List<Error> errors, string message = OneOrMoreBusinessLogicFailures) : this(
                                                                   message)
        {
            DomainErrors = errors.Select(x => new KeyValuePair<string, string>(x.Code, x.Description)).ToList();
        }

        public IDictionary<string, string> Errors { get; }
        public IList<KeyValuePair<string, string>> DomainErrors { get; set; }
    }
}
