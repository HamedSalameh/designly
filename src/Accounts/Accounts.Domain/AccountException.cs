﻿using Designly.Base;

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

        public AccountException(string message, Exception innerException, Error accountError) : base(message, innerException)
        {
            Errors = [accountError];
        }

        public AccountException(string message, Exception innerException, List<Error> accountErrors) : base(message, innerException)
        {
            Errors = new List<Error>(accountErrors);
        }

        public AccountException(string message, List<Error> accountErrors) : base(message)
        {
            Errors = new List<Error>(accountErrors);
        }

        public AccountException(string message, Error accountError) : base(message)
        {
            Errors = [accountError];
        }

        public AccountException(Error accountError)
        {
            this.Errors = [accountError];
        }

        public List<Error> Errors { get; set; } = [];
        
        public override string ToString()
        {
            return $"{base.ToString()} Errors: {Errors}";
        }
    }
}
