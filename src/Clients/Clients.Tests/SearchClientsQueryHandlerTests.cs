using Clients.Application.Queries;
using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using Designly.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Clients.Tests
{
    [TestFixture]
    public class SearchClientsQueryHandlerTests
    {
        private readonly string firstName = "John";
        private readonly string familyName = "Doe";
        private readonly string city = "Utopia";

        [Test]
        public void Handle_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var loggerMock = Substitute.For<ILogger<SearchClientsQueryHandler>>();
            var handler = new SearchClientsQueryHandler(loggerMock, unitOfWorkMock);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.Handle(null!, CancellationToken.None));
        }

        [Test]
        public async Task Handle_SearchByName_ReturnsSingle()
        {
            // Arrange
            var request = new SearchClientsQuery(Guid.NewGuid(), firstName, "", "");
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var loggerMock = Substitute.For<ILogger<SearchClientsQueryHandler>>();
            var handler = new SearchClientsQueryHandler(loggerMock, unitOfWorkMock);

            var client = new Client(firstName, familyName, new Address(city), new ContactDetails("123-123123"), Guid.NewGuid());
            var clientList = new List<Client> { client };
            unitOfWorkMock.ClientsRepository.SearchClientsAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(clientList);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ToList(), Has.Count.EqualTo(1));
                Assert.That(result.First().FirstName, Is.EqualTo(firstName));
            });
        }

        [Test]
        public async Task Handle_SearchByFamilyName_ReturnsSingle()
        {
            // Arrange
            var request = new SearchClientsQuery(Guid.NewGuid(), "", familyName, "");
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var loggerMock = Substitute.For<ILogger<SearchClientsQueryHandler>>();
            var handler = new SearchClientsQueryHandler(loggerMock, unitOfWorkMock);

            var client = new Client(firstName, familyName, new Address(city), new ContactDetails("123-123123"), Guid.NewGuid());
            var clientList = new List<Client> { client };
            unitOfWorkMock.ClientsRepository.SearchClientsAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(clientList);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ToList(), Has.Count.EqualTo(1));
                Assert.That(result.First().FamilyName, Is.EqualTo(familyName));
            });
        }

        [Test]
        public async Task Handle_SearchByCity_ReturnsSingle()
        {
            // Arrange
            var request = new SearchClientsQuery(Guid.NewGuid(), "", "", city);
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var loggerMock = Substitute.For<ILogger<SearchClientsQueryHandler>>();
            var handler = new SearchClientsQueryHandler(loggerMock, unitOfWorkMock);

            var client = new Client(firstName, familyName, new Address(city), new ContactDetails("123-123123"), Guid.NewGuid());
            var clientList = new List<Client> { client };
            unitOfWorkMock.ClientsRepository.SearchClientsAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(clientList);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ToList(), Has.Count.EqualTo(1));
                Assert.That(result.First().Address.City, Is.EqualTo(city));
            });
        }

        [Test]
        public async Task Handle_SearchByAll_ReturnsSingle()
        {
            // Arrange
            var request = new SearchClientsQuery(Guid.NewGuid(), firstName, familyName, city);
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var loggerMock = Substitute.For<ILogger<SearchClientsQueryHandler>>();
            var handler = new SearchClientsQueryHandler(loggerMock, unitOfWorkMock);

            var client = new Client(firstName, familyName, new Address(city), new ContactDetails("123-123123"), Guid.NewGuid());
            var clientList = new List<Client> { client };
            unitOfWorkMock.ClientsRepository.SearchClientsAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(clientList);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ToList(), Has.Count.EqualTo(1));
                Assert.That(result.First().FirstName, Is.EqualTo(firstName));
                Assert.That(result.First().FamilyName, Is.EqualTo(familyName));
                Assert.That(result.First().Address.City, Is.EqualTo(city));
            });
        }

        [Test]
        public async Task Handle_SearchByAll_ReturnsMultiple()
        {
            // Arrange
            var request = new SearchClientsQuery(Guid.NewGuid(), firstName, familyName, city);
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var loggerMock = Substitute.For<ILogger<SearchClientsQueryHandler>>();
            var handler = new SearchClientsQueryHandler(loggerMock, unitOfWorkMock);

            var client1 = new Client(firstName, familyName, new Address(city), new ContactDetails("123-123123"), Guid.NewGuid());
            var client2 = new Client(firstName, familyName, new Address(city), new ContactDetails("123-123123"), Guid.NewGuid());
            var clientList = new List<Client> { client1, client2 };
            unitOfWorkMock.ClientsRepository.SearchClientsAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(clientList);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToList(), Has.Count.EqualTo(2));
        }
    }
}
