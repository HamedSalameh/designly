using Clients.Application.Commands;
using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using Designly.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clients.Tests
{
    [TestFixture]
    public class UpdateClientCommandHandlerTests
    {
        private Mock<ILogger<UpdateClientCommandHandler>> _loggerMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private UpdateClientCommandHandler _handler;

        readonly Guid Tenant = Guid.NewGuid();
        readonly string primaryPhoneNumber = "123-9222333";
        readonly string secondaryPhoneNumber = "12-9987878";
        readonly string emailAddress = "someaddress@mailserver.com";
        readonly string street = "SomeStreet";
        readonly string city = "cityName";
        readonly string buildingNumber = "bn-05";
        readonly List<string> addressLines = ["address line1", "address line2"];

        private ContactDetails contactDetails;
        private Address address;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<UpdateClientCommandHandler>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateClientCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

            contactDetails = new ContactDetails(primaryPhoneNumber, secondaryPhoneNumber, emailAddress);
            address = new Address(city, street, buildingNumber, addressLines);
        }

        // Test: should update client when name is changed
        [Test]
        public async Task Handle_ShouldUpdateClient_WhenNameIsChanged()
        {
            // Arrange
            var demoClientId = Guid.NewGuid();
            var updatedClient = new Client("Jane", "Smith", address, contactDetails, Tenant);
            var updateClientRequest = new UpdateClientCommand(updatedClient);

            _unitOfWorkMock.Setup(unit => unit.ClientsRepository.UpdateClientAsync(updatedClient, CancellationToken.None))
                .ReturnsAsync(updatedClient);

            // Act
            var result = await _handler.Handle(updateClientRequest, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(updatedClient));
        }

        // Test: should throw exception when client is null
        [Test]
        public void Handle_ShouldThrowException_WhenClientIsNull()
        {
            // Arrange
            var updateClientRequest = new UpdateClientCommand(null!);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(updateClientRequest, CancellationToken.None));
        }

        // Test: should throw exception when request is null
        [Test]
        public void Handle_ShouldThrowException_WhenRequestIsNull()
        {
            // Arrange
            UpdateClientCommand updateClientCommand = null!;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(updateClientCommand, CancellationToken.None));
        }
    }
}
