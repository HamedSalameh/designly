namespace Designly.Shared
{
    public static class Consts
    {
        //https://stackoverflow.com/questions/417142/what-is-the-maximum-length-of-a-url-in-different-browsers
        public static readonly short MaxUrlLength = 2048;

        //https://en.wikipedia.org/wiki/E.164
        public static readonly short MaxPhoneNumberLength = 15;

        //https://www.rfc-editor.org/errata_search.php?rfc=3696&eid=1690
        public static readonly short MaxEmailAddressLength = 320;

        public static readonly byte MaxClientNameLength = 100;

        public static readonly byte FirstNameMaxLength = 50;
        public static readonly byte LastNameMaxLength = 50;

        public static readonly byte DefaultMaxLimitedStringLength = 255;

        public static class Strings
        {
            public static readonly string ValueNotSet = "Value not set";

            public static readonly string NotAvailable = "N/A";
        }

        // Message headers
        public static readonly string CorrelationIdHeader = "X-Correlation-Id";
        public static readonly string ApiVersionHeaderEntry = "api-version";
        public static readonly string ApiVersionQueryStringEntry = "api-version";
        
    }

    
}
