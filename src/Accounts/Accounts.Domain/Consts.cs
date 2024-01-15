namespace Accounts.Domain
{
    public static class Consts
    {
        //https://stackoverflow.com/questions/417142/what-is-the-maximum-length-of-a-url-in-different-browsers
        public const short MaxUrlLength = 2048;
        public const int AccountNameMaxLength = 100;
        public const int TeamNameMaxLength = 50;

        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
        public const int JobTitleMaxLength = 50;

        //https://www.rfc-editor.org/errata_search.php?rfc=3696&eid=1690
        public const short MaxEmailAddressLength = 320;
        //https://en.wikipedia.org/wiki/E.164
        public const short MaxPhoneNumberLength = 15;

        public enum UserStatus
        {
            /// <summary>
            /// Registered users that are not activated yet or confirmed their email address.
            /// </summary>
            BeforeActivation,

            /// <summary>
            /// Active users are the ones that are in use.
            /// </summary>
            Active,

            /// <summary>
            /// Suspended users are the ones that are not in use but can be reactivated.
            /// <br>User can be suspended when: </br>
            /// <list type="bullet">
            ///     <item>The payment is not made for a long time</item>
            ///     <item>Requested by the account owner</item>
            ///     <item>Requested by the user</item>
            ///     <item>Manually set by the admin</item>
            /// </list>
            /// </summary>
            Suspended,

            /// <summary>
            /// Disabled users are the ones that are not in use and cannot be reactivated.
            /// <br>User can be disabled when: </br>
            /// <list type="bullet">
            ///     <item>The payment is not made for a long time</item>
            ///     <item>Requested by the account owner</item>
            ///     <item>Requested by the user</item>
            ///     <item>Manually set by the admin</item>
            /// </list>
            /// </summary>
            Disabled,

            /// <summary>
            /// Marked for deletion users - this property is set by the application only.
            /// <br>MarkedForDeletion users are not deleted immediately, they are deleted after a certain period of time.</br>
            /// </summary>
            MarkedForDeletion,

            Deleted
        }

        public enum TeamStatus
        {
            Active,
            Suspended,
            Disabled,
            MarkedForDeletion,
            Deleted
        }

        public enum AccountStatus
        {
            /// <summary>
            /// Active accounts are the ones that are in use.
            /// </summary>
            Active,

            /// <summary>
            /// Suspended accounts are the ones that are not in use but can be reactivated.
            /// <br>Account can be suspended when: </br>
            /// <list type="bullet">
            ///     <item>The payment is not made for a long time</item>
            ///     <item>Requested by the account owner</item>
            ///     <item>Manually set by the admin</item>
            /// </list>
            /// </summary>
            Suspended,

            /// <summary>
            /// Disabled accounts are the ones that are not in use and cannot be reactivated.
            /// <br>Account can be disabled when: </br>
            /// <list type="bullet">
            ///     <item>The payment is not made for a long time, past the grace period</item>
            ///     <item>Requested by the account owner</item>
            ///     <item>Manually set by the admin</item>
            /// </list>
            /// </summary>
            Disabled,

            /// <summary>
            /// Marked for deletion accounts - this property is set by the application only.
            /// <br>MarkedForDeletion accounts are not deleted immediately, they are deleted after a certain period of time.</br>
            /// </summary>
            MarkedForDeletion,

            /// <summary>
            /// Deleted accounts are the ones that are deleted
            /// </summary>
            Deleted
        }

    }
}
