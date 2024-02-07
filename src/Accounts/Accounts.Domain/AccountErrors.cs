using Designly.Base;

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

        // User account errors        
        public static readonly Error UserNotFound = new("user_not_found", "User not found.");
        public static readonly Error UserEmailAlreadyExists = new("user_email_already_exists", "User email already exists.");
        public static readonly Error UserEmailNotFound = new("user_email_not_found", "User email not found.");
        public static readonly Error UserEmailNotValid = new("user_email_not_valid", "User email not valid.");
        public static readonly Error UserEmailIsBlacklisted = new("user_email_is_blacklisted", "User email is blacklisted.");
        public static readonly Error UserIsBlacklisted = new("user_is_blacklisted", "User is blacklisted");

        public static readonly Error UserIsNotActivated = new("user_not_activated", "User is not activated");
        public static readonly Error UserIsSuspended = new("user_is_suspended", "User is suspended");
        public static readonly Error UserIsDisabled = new("user_is_disabled", "User is disabled");
        public static readonly Error UserIsMarkedForDeletion = new("user_marked_for_deletion", "User is marked for deletion");
        public static readonly Error UserIsDeleted = new("user_is_deleted", "User is deleted");

        public static readonly Error UnsupportedUserStatus = new("unsupported_status", "Unsupported status");
    }
}
