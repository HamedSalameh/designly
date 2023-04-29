using System.Text;

namespace Clients.Domain.ValueObjects
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

            PrimaryPhoneNumber = primaryPhoneNumber ?? throw new ArgumentNullException(nameof(PrimaryPhoneNumber));
            SecondaryPhoneNumber = secondaryPhoneNumber;
            EmailAddress = emailAddress;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(PrimaryPhoneNumber))
            {
                sb.Append($"Primary phone number: {PrimaryPhoneNumber}");
            }

            if (!string.IsNullOrEmpty(SecondaryPhoneNumber))
            {
                sb.Append($", Secondary phone number: {SecondaryPhoneNumber}");
            }

            if (!string.IsNullOrEmpty(EmailAddress))
            {
                sb.Append($", Email address: {EmailAddress}");
            }

            return sb.ToString();
        }

    }
}
