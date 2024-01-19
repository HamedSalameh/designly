namespace Accounts.Domain
{
    public class AccountException : Exception
    {
        public AccountException(string message) : base(message)
        {
        }

        public AccountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountException()
        {
        }

        public AccountException(string message, Exception innerException, string errorCode) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{base.ToString()} ErrorCode: {ErrorCode}";
        }
    }
}
