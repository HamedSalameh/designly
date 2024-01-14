﻿namespace Designly.Shared.Exceptions
{
    /// <summary>
    /// A business logic exception indicates that the request is valid but the business logic has failed.
    /// </summary>
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string>();
        }

        public BusinessLogicException(string message, Exception inner)
            : base(message, inner)
        {
            Errors = new Dictionary<string, string>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="BusinessLogicException"/> with a one or more errors.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="message"></param>
        public BusinessLogicException(IDictionary<string, string> errors, string message = "One or more business logic failures have occurred.") : this(
                       message)
        {
            Errors = errors;
        }

        /// <summary>
        /// Creates a new instance of <see cref="BusinessLogicException"/> with a single error.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        public BusinessLogicException(KeyValuePair<string, string> error, string message = "One or more business logic failures have occurred.") : this(
                                  message)
        {
            Errors = new Dictionary<string, string>
            {
                { error.Key, error.Value }
            };
        }

        /// <summary>
        /// Creates a new instance of <see cref="BusinessLogicException"/> with a single error.
        /// </summary>
        /// <param name="key">The property that failed the business logic validation</param>
        /// <param name="keyValidationMessage">Description of the business logic failure and reason</param>
        /// <param name="message"></param>
        public BusinessLogicException(string key, string keyValidationMessage, string message = "One or more business logic failures have occurred.") : this(
                                             message)
        {
            Errors = new Dictionary<string, string>
            {
                { key, keyValidationMessage }
            };
        }

        public IDictionary<string, string> Errors { get; }
    }
}