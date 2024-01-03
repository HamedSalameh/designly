using Clients.Domain.Entities;
using Designly.Shared.ValueObjects;

namespace Clients.Tests
{
    [TestFixture]
    public class ClientCreationTests
    {
        private const string FirstName = "John";
        private const string FamilyName = "Doe";
        private const string City = "Utopia";
        private const string newCity = "Urbanoia";
        private const string PrimaryPhoneNumber = "0542123123";
        private readonly Guid TenantId = Guid.NewGuid();

        [Test]
        public void UpdateAddress_ShouldUpdateAddress()
        {
            // Arrange
            var client = new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), TenantId);
            var newAddress = new Address(newCity);

            // Act
            client.UpdateAddress(newAddress);

            // Assert
            Assert.That(client.Address, Is.EqualTo(newAddress));
        }

        [Test]
        public void UpdateContactDetails_ShouldUpdateContactDetails()
        {
            // Arrange
            var client = new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), TenantId);
            var newContactDetails = new ContactDetails(PrimaryPhoneNumber);

            // Act
            client.UpdateContactDetails(newContactDetails);

            // Assert
            Assert.That(client.ContactDetails, Is.EqualTo(newContactDetails));
        }

        [Test]
        public void UpdateClient_ShouldUpdateClientProperties()
        {
            // Arrange
            var client = new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), TenantId);
            var updatedClient = new Client("Jane", "Smith", new Address(newCity), new ContactDetails("1234567890"), TenantId);

            // Act
            client.UpdateClient(updatedClient);

            // Assert
            Assert.That(client.FirstName, Is.EqualTo(updatedClient.FirstName));
            Assert.Multiple(() =>
            {
                Assert.That(client.FamilyName, Is.EqualTo(updatedClient.FamilyName));
                Assert.That(client.Address, Is.EqualTo(updatedClient.Address));
                Assert.That(client.ContactDetails, Is.EqualTo(updatedClient.ContactDetails));
            });
        }

        [Test]
        public void ToString_ShouldReturnFormattedStringRepresentation()
        {
            // Arrange
            var address = new Address(City);
            var contactDetails = new ContactDetails(PrimaryPhoneNumber);
            var client = new Client(FirstName, FamilyName, address, contactDetails, TenantId);

            // Act
            var result = client.ToString();

            // Assert
            var expected = $"{FirstName} {FamilyName}, {City}, {PrimaryPhoneNumber}";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UpdateClient_WithNullClient_ShouldThrowArgumentException()
        {
            // Arrange
            var client = new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), TenantId);

            // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentException>(() => client.UpdateClient(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
