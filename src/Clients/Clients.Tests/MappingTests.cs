using AutoMapper;
using Clients.API.DTO;
using Clients.API.Mappers;
using Clients.Domain.ValueObjects;
using NuGet.Frameworks;

namespace Clients.Tests
{
    public class MappingTests
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
        public void MapAddressDto_To_Address()
        {
            var addressDto = new AddressDto(city, street, buildingNumber, addressLines);
            var address = _mapper.Map<Address>(addressDto);

            Assert.That(address, Is.Not.Null);
            Assert.That(address.City, Is.EqualTo(addressDto.City));
            Assert.That(address.Street, Is.EqualTo(addressDto.Street));
            Assert.That(address.BuildingNumber, Is.EqualTo(addressDto.BuildingNumber));
            Assert.That(address.AddressLines, Is.EquivalentTo(addressDto.AddressLines));
        }

        [Test]
        public void MapAddressToAddressDto()
        {
            var address = new Address(city, street, buildingNumber, addressLines);
            var addressDto = _mapper.Map<AddressDto>(address);

            Assert.That(addressDto, Is.Not.Null);
            Assert.That(addressDto.City, Is.EqualTo(address.City));
            Assert.That(addressDto.Street, Is.EqualTo(address.Street));
            Assert.That(addressDto.BuildingNumber, Is.EqualTo(address.BuildingNumber));
            Assert.True(addressDto.AddressLines?.Any());
            Assert.That(addressDto.AddressLines, Is.EquivalentTo(address.AddressLines));
        }

        [Test]
        public void MapContactDetailstoContainDetailsDto()
        {
            var contactDetails = new ContactDetails(primaryPhoneNumer, secondaryNumberNumber, emailAddress);
            var contactDetailsDto = _mapper.Map<ContactDetailsDto>(contactDetails);

            Assert.That(contactDetailsDto, Is.Not.Null);
            Assert.That(contactDetailsDto.PrimaryPhoneNumber, Is.EqualTo(contactDetails.PrimaryPhoneNumber));
            Assert.That(contactDetailsDto.SecondaryPhoneNumber, Is.EqualTo(contactDetails.SecondaryPhoneNumber));
            Assert.That(contactDetailsDto.EmailAddress, Is.EqualTo(contactDetails.EmailAddress));
        }

        [Test]
        public void MapContactDetailsDtotoContainDetails()
        {
            var contactDetailsDto = new ContactDetailsDto(primaryPhoneNumer, secondaryNumberNumber, emailAddress);
            var contactDetails = _mapper.Map<ContactDetails>(contactDetailsDto);

            Assert.That(contactDetails, Is.Not.Null);
            Assert.That(contactDetails.PrimaryPhoneNumber, Is.EqualTo(contactDetailsDto.PrimaryPhoneNumber));
            Assert.That(contactDetails.SecondaryPhoneNumber, Is.EqualTo(contactDetailsDto.SecondaryPhoneNumber));
            Assert.That(contactDetails.EmailAddress, Is.EqualTo(contactDetailsDto.EmailAddress));
        }
    }
}