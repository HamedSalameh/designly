namespace Accounts.Domain
{
    public static class AccountErrors
    {
        public static readonly Error AccountNotFound = new("account_not_found", "Account not found.");
        public static readonly Error AccountAlreadyExists = new("account_already_exists", "Account already exists.");
        public static readonly Error AccountNameAlreadyExists = new("account_name_already_exists", "Account name already exists.");
        public static readonly Error AccountOwnerAlreadyExists = new("account_owner_already_exists", "Account owner already exists.");
        public static readonly Error AccountOwnerNotFound = new("account_owner_not_found", "Account owner not found.");
        public static readonly Error AccountOwnerEmailAlreadyExists = new("account_owner_email_already_exists", "Account owner email already exists.");
        public static readonly Error AccountOwnerEmailNotFound = new("account_owner_email_not_found", "Account owner email not found.");
        public static readonly Error AccountOwnerEmailNotValid = new("account_owner_email_not_valid", "Account owner email not valid.");
    }
}
