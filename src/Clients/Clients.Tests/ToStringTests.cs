using AutoMapper;
using Clients.API.DTO;
using Clients.API.Mappers;
using Clients.Domain.Entities;
using Clients.Domain.ValueObjects;
using NuGet.Frameworks;

namespace Clients.Tests
{
    [TestFixture]
    public class ToStringOverridesTests
    {
        private static IMapper _mapper;

        string street = "SomeStreet";
        string city = "cityName";
        string buildingNumber = "bn-05";
        List<string> addressLines = new List<string>() { "address line1", "address line2" };
        string primaryPhoneNumer = "123-9222333";
        string secondaryNumberNumber = "12-9987878";
        string emailAddress = "someaddress@mailserver.com";

        [SetUp]
        public void Setup()
        {
            var mapperConfig = new MapperConfiguration(mapperConfiguration =>
            {
                mapperConfiguration.AddProfile(new DefaultMappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public void Address_ToString_Full()
        {
            var address = new Address(city, street, buildingNumber, addressLines);
            var addressToString = address.ToString();

            Assert.That(addressToString, Is.Not.Empty);
        }

        [Test]
        public void Address_ToString_NoBuilding_NoAddresslines()
        {
            var address = new Address(city, street);
            var addressToString = address.ToString();

            Assert.That(addressToString, Is.Not.Empty);
        }

        [Test]
        public void Address_ToString_NoStreet_WithAddressLines()
        {
            var address = new Address(city, addressLines: addressLines);
            var addressToString = address.ToString();

            Assert.That(addressToString, Is.Not.Empty);
        }

        [Test]
        public void ToString_ReturnsAllFields()
        {
            // Arrange
            var contactDetails = new ContactDetails("1234567890", "0987654321", "test@example.com");

            // Act
            var result = contactDetails.ToString();

            // Assert
            Assert.That(result, Is.EqualTo("Primary phone number: 1234567890, Secondary phone number: 0987654321, Email address: test@example.com"));
        }

        [Test]
        public void ToString_ReturnsOnlyPrimaryPhone()
        {
            // Arrange
            var contactDetails = new ContactDetails("1234567890");

            // Act
            var result = contactDetails.ToString();

            // Assert
            Assert.That(result, Is.EqualTo("Primary phone number: 1234567890"));
        }
    }
}