using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Domain
{
    public static class Consts
    {
        //https://stackoverflow.com/questions/417142/what-is-the-maximum-length-of-a-url-in-different-browsers
        public const short MaxUrlLength = 2048;

        public static class Account
        {
            public const int NameMaxLength = 100;
        }

        public static class Team
        {
            public const int NameMaxLength = 50;
        }

        public static class User
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int JobTitleMaxLength = 50;
            //https://www.rfc-editor.org/errata_search.php?rfc=3696&eid=1690
            public const short MaxEmailAddressLength = 320;
            //https://en.wikipedia.org/wiki/E.164
            public const short MaxPhoneNumberLength = 15;
        }
    }
}
