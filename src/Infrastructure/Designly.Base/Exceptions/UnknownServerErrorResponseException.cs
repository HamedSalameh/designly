namespace Designly.Base.Exceptions
{
    public class UnknownServerErrorResponseException : Exception
    {
        public UnknownServerErrorResponseException(string? message = "An unknown server error response has occurred.")
            : base(message)
        {
        }

        public UnknownServerErrorResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
