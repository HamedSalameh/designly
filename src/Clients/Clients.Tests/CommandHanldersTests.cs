using Clients.Application.Commands;
using Clients.Domain.Entities;
using Clients.Domain.ValueObjects;
using Clients.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clients.Tests
{
    [TestFixture]
    public class CreateClientCommandHandlerTests
    {
        private Mock<ILogger<CreateClientCommandHandler>> _loggerMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CreateClientCommandHandler _handler;

        private string firstName = "John";
        private string familyName = "Doe";
        string primaryPhoneNumber = "123-9222333";
        string secondaryPhoneNumber = "12-9987878";
        string emailAddress = "someaddress@mailserver.com";
        string street = "SomeStreet";
        string city = "cityName";
        string buildingNumber = "bn-05";
        List<string> addressLines = new List<string>() { "address line1", "address line2" };

        private ContactDetails contactDetails;
        private Address address;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CreateClientCommandHandler>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateClientCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            contactDetails = new ContactDetails(primaryPhoneNumber, secondaryPhoneNumber, emailAddress);
            address = new Address(city, street, buildingNumber, addressLines);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsClientId()
        {
            // Arrange
            var draftClient = new Client(firstName, familyName, address, contactDetails);
            var request = new CreateClientCommand(draftClient);

            _unitOfWorkMock.Setup(uow => uow.ClientsRepository.CreateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Handle_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            ILogger<CreateClientCommandHandler> logger = null;
            var unitOfWork = new Mock<IUnitOfWork>().Object;

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => new CreateClientCommandHandler(logger, unitOfWork));
        }

        [Test]
        public void Handle_NullUnitOfWork_ThrowsArgumentNullException()
        {
            // Arrange
            var logger = new Mock<ILogger<CreateClientCommandHandler>>().Object;
            IUnitOfWork unitOfWork = null;

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => new CreateClientCommandHandler(logger, unitOfWork));
        }
    }
}
