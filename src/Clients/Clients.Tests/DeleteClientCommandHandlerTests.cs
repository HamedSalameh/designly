using Clients.Application.Commands;
using Clients.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clients.Tests
{
    public class DeleteClientCommandHandlerTests
    {
        private Mock<ILogger<DeleteClientCommandHandler>> _loggerMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private DeleteClientCommandHandler _handler;

        readonly Guid Tenant = Guid.NewGuid();


        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<DeleteClientCommandHandler>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteClientCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);

        }

        [Test]
        public async Task Handle_ShouldDeleteClient()
        {
            // arrange
            var demoClientId = Guid.NewGuid();
            var deleteClientRequest = new DeleteClientCommand(Tenant, demoClientId);

            _unitOfWorkMock.Setup(unit => unit.ClientsRepository.DeleteClientAsync(Tenant, demoClientId, CancellationToken.None))
                .Returns(Task.CompletedTask);

            // Assert
            await _handler.Handle(deleteClientRequest, CancellationToken.None);

            // assert called once
            _unitOfWorkMock.Verify(unit => unit.ClientsRepository.DeleteClientAsync(Tenant, demoClientId, CancellationToken.None), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenClientIdIsEmpty()
        {
            // arrange
            var deleteClientRequest = new DeleteClientCommand(Tenant, Guid.Empty);

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(deleteClientRequest, CancellationToken.None));
            Assert.That(exception.Message, Is.EqualTo(nameof(deleteClientRequest.ClientId)));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenRequestIsNull()
        {
            // arrange
            DeleteClientCommand deleteClientRequest = null!;
            
            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(deleteClientRequest, CancellationToken.None));
        }

    }
}
