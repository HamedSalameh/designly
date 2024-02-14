namespace Designly.Base.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        // Represent business exception
        // The exception data can passed as dictionary<string,string> or as a list of ValidationFailure

        public ValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string>();
        }

        public ValidationException(string message, Exception inner)
            : base(message, inner)
        {
            Errors = new Dictionary<string, string>();
        }

        public ValidationException(string message, IDictionary<string, string> errors)
            : base(message)
        {
            Errors = errors;
        }

        public ValidationException(string message, IList<KeyValuePair<string, string>> errors) : base(message)
        {
            Errors ??= new Dictionary<string, string> ();
            foreach(var error in errors)
            {
                Errors.Add(error.Key, error.Value);
            }
        }

        public ValidationException(IDictionary<string, string> errors) : this(
            "One or more validation failures have occurred.")
        {
            Errors = errors;
        }

        public IDictionary<string, string> Errors { get; }
    }

    
}