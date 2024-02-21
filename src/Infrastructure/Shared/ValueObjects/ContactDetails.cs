using System.Text;

namespace Designly.Shared.ValueObjects
{
    public class ContactDetails
    {
        public string PrimaryPhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; } = "";
        public string EmailAddress { get; set; } = "";

        public ContactDetails(string primaryPhoneNumber, string secondaryPhoneNumber = "", string emailAddress = "")
        {
            if (string.IsNullOrEmpty(primaryPhoneNumber))
                throw new ArgumentNullException(primaryPhoneNumber);

            if (primaryPhoneNumber.Length > Consts.MaxPhoneNumberLength)
                throw new ArgumentException($"Phone number cannot be longer than {Consts.MaxPhoneNumberLength} characters");

            if (EmailAddress?.Length > Consts.MaxEmailAddressLength)
            {
                throw new ArgumentException($"Email address length cannot be longer than {Consts.MaxEmailAddressLength} characters");
            }

            PrimaryPhoneNumber = primaryPhoneNumber;
            SecondaryPhoneNumber = secondaryPhoneNumber;
            EmailAddress = emailAddress;
        }

        private ContactDetails()
        {
            PrimaryPhoneNumber = Consts.Strings.ValueNotSet;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            if (!string.IsNullOrEmpty(PrimaryPhoneNumber))
            {
                sb.Append($"{PrimaryPhoneNumber}");
            }

            if (!string.IsNullOrEmpty(SecondaryPhoneNumber))
            {
                sb.Append($", {SecondaryPhoneNumber}");
            }

            if (!string.IsNullOrEmpty(EmailAddress))
            {
                sb.Append($", {EmailAddress}");
            }

            return sb.ToString();
        }

    }
}
