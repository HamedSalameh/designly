namespace Designly.Base.Exceptions
{
    // exception to handler api with 500 status code
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string? message = "An internal server error has occurred.")
            : base(message)
        {
        }

        public InternalServerErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
