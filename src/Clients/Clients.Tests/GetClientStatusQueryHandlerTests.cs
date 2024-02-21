using Clients.Application.Queries;
using Clients.Domain;
using Clients.Domain.Entities;
using Clients.Infrastructure.Interfaces;
using Designly.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections;

namespace Clients.Tests
{
    [TestFixture]
    public class GetClientStatusQueryHandlerTests
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

        public class Handle_ValidRequest_ReturnsClientStatusData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(ClientStatusCode.Active, ClientStatus.Active);
                    yield return new TestCaseData(ClientStatusCode.Inactive, ClientStatus.Inactive);
                    yield return new TestCaseData(ClientStatusCode.Suspended, ClientStatus.Suspended);
                    yield return new TestCaseData(ClientStatusCode.HighRisk, ClientStatus.HighRisk);
                    yield return new TestCaseData(ClientStatusCode.Blacklisted, ClientStatus.Blacklisted);
                    yield return new TestCaseData(ClientStatusCode.Unsupported, ClientStatus.Unsupported);
                }
            }
        }

        [Test]
        [TestCaseSource(typeof(Handle_ValidRequest_ReturnsClientStatusData), nameof(Handle_ValidRequest_ReturnsClientStatusData.TestCases))]
        public async Task Handle_ValidRequest_ReturnsClientStatus(ClientStatusCode clientStatusCode, ClientStatus expectedStatus)
        {
            // Arrange
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var logger = Substitute.For<ILogger<GetClientStatusQueryHandler>>();

            var handler = new GetClientStatusQueryHandler(unitOfWork, logger);
            var contactDetails = new ContactDetails(primaryPhoneNumber, secondaryPhoneNumber, emailAddress);
            var address = new Address(city, street, buildingNumber, addressLines);
            var demoClient = new Client(firstName, familyName, address, contactDetails, Tenant);
            demoClient.Status = clientStatusCode;

            unitOfWork.ClientsRepository.GetClientAsyncWithDapper(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(demoClient);

            var result = await handler.Handle(new GetClientStatusQuery(Tenant, demoClient.Id), CancellationToken.None);

            Assert.That(result, Is.EqualTo(expectedStatus));
        }
    }
}
