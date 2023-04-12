namespace Clients.Domain.ValueObjects
{
    public class ContactDetails
    {
        public string PrimaryPhoneNumber { get; set; }
        public string? SecondaryPhonenumber { get; set; }
        public string? EmailAddress { get; set; }

        public ContactDetails(string primaryPhoneNumber, string secondaryPhonenumber = "", string emailAddress = "")
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
            
            SecondaryPhonenumber = secondaryPhonenumber;
            EmailAddress = emailAddress;
        }
    }
}
