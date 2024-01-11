using Microsoft.AspNetCore.Http;

namespace Designly.Shared
{
    public static class Consts
    {
        //https://stackoverflow.com/questions/417142/what-is-the-maximum-length-of-a-url-in-different-browsers
        public const short MaxUrlLength = 2048;

        //https://en.wikipedia.org/wiki/E.164
        public const short MaxPhoneNumberLength = 15;

        //https://www.rfc-editor.org/errata_search.php?rfc=3696&eid=1690
        public const short MaxEmailAddressLength = 320;

        public const byte MaxClientNameLength = 100;

        public const byte DefaultMaxLimitedStringLength = 255;

        public static class Strings
        {
            public const string ValueNotSet = "Value not set";

            public const string NotAvailable = "N/A";
        }

        // Message headers
        public const string CorrelationIdHeader = "X-Correlation-Id";
        public const string ApiVersionHeaderEntry = "api-version";
        public const string ApiVersionQueryStringEntry = "api-version";
        
    }

    public enum ClientStatusCode
    {
        // Default value
        NonExistent = 0,
        // Regular client statuses
        Active,
        // Below are the statuses that are not considered 'Green' and safe to transact with
        Inactive,
        Suspended,        
        HighRisk,
        Blacklisted
    }
}
