namespace Clients.Domain
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

        public static string CorrelationIdHeader = "X-Correlation-Id";
    }
}
