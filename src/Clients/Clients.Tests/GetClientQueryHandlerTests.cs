using Clients.Application.Queries;
using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using Designly.Base.Exceptions;
using Designly.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;


namespace Clients.Tests
{
    [TestFixture]
    public class GetClientQueryHandlerTests
    {
        readonly Guid Tenant = Guid.NewGuid();
        readonly string firstName = "John";
        readonly string familyName = "Doe";
        readonly string primaryPhoneNumber = "123-9222333";
        readonly string secondaryPhoneNumber = "12-9987878";
        readonly string emailAddress = "someaddress@mailserver.com";
        readonly string street = "SomeStreet";
        readonly string city = "cityName";
        readonly string buildingNumber = "bn-05";
        readonly List<string> addressLines = new() { "address line1", "address line2" };

        [Test]
        public async Task Handle_ValidRequest_ReturnsClient()
        {
            // Arrange
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var logger = Substitute.For<ILogger<GetClientQueryHandler>>();

            var handler = new GetClientQueryHandler(unitOfWork, logger);

            var contactDetails = new ContactDetails(primaryPhoneNumber, secondaryPhoneNumber, emailAddress);
            var address = new Address(city, street, buildingNumber, addressLines);
            var demoClient = new Client(firstName, familyName, address, contactDetails, Tenant);
            demoClient.Id = Guid.NewGuid();

            var request = new GetClientQuery(demoClient.Id, Tenant);

            unitOfWork.ClientsRepository.GetClientAsyncWithDapper(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(demoClient);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(demoClient.Id));
        }

        [Test]
        public void Handle_InvalidTenantId_ThrowsArgumentNullException()
        {
            // Arrange
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var logger = Substitute.For<ILogger<GetClientQueryHandler>>();

            var handler = new GetClientQueryHandler(unitOfWork, logger);

            var request = new GetClientQuery(Guid.Empty, Guid.NewGuid());

            unitOfWork.ClientsRepository.GetClientAsyncWithDapper(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ThrowsAsync(new ArgumentNullException());

            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.That(ex, Is.Not.Null);
        }

        [Test]
        public void Handle_InvalidClientId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var logger = Substitute.For<ILogger<GetClientQueryHandler>>();

            var handler = new GetClientQueryHandler(unitOfWork, logger);

            var request = new GetClientQuery(Guid.NewGuid(), Guid.Empty);

            unitOfWork.ClientsRepository.GetClientAsyncWithDapper(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ThrowsAsync(new ArgumentNullException());

            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.That(ex, Is.Not.Null);
        }

        [Test]
        public void Handle_ValidRequest_NoClientFound_ThrowsEntityNotFound()
        {
            // Arrange
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var logger = Substitute.For<ILogger<GetClientQueryHandler>>();

            var handler = new GetClientQueryHandler(unitOfWork, logger);

            var request = new GetClientQuery(Guid.Empty, Guid.Empty);

            unitOfWork.ClientsRepository.GetClientAsyncWithDapper(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

            // Act
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() => handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.That(ex, Is.Not.Null);
        }
    }

}