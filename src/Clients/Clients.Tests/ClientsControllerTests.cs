using AutoMapper;
using Clients.API.Controllers;
using Clients.API.DTO;
using Clients.API.Mappers;
using Clients.Application.Commands;
using Clients.Application.Queries;
using Clients.Domain.Entities;
using Designly.Auth.Identity;
using Designly.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Clients.Tests
{
    public class MockTenantProvider : ITenantProvider
    {
        public Guid mockTenantId { get; set; }
        public bool mockIsServiceAccount { get; set; }
        

        // Implement the interface
        public Guid GetTenantId()
        {
            return mockTenantId;
        }

        public Guid? GetTenantIdFromRequest(HttpContext context)
        {
            return mockTenantId;
        }

        public bool IsServiceAccount(HttpContext context)
        {
            return mockIsServiceAccount;
        }

        public void SetTenantId(Guid tenantId)
        {
            this.mockTenantId = tenantId;
        }
    }

    [TestFixture]
    public class ClientsControllerTests
    {
        private const string FirstName = "John";
        private const string FamilyName = "Doe";
        private const string City = "Utopia";
        private const string newCity = "Urbanoia";
        private const string PrimaryPhoneNumber = "0542123123";

        private readonly ILogger<ClientsController> loggerMock = Substitute.For<ILogger<ClientsController>>();
        private readonly ITenantProvider tenantProviderMock = new MockTenantProvider();
        private static IMapper _mapper;

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
        public async Task CreateClient_NullDto_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            tenantProviderMock.SetTenantId(Guid.NewGuid());

            // Act
            
            var result = await controller.Create(null!, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateClient_InvalidTenantId_ReturnsBadRequest()
        {
            // Arrange
            var tenantId = Guid.Empty;
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            var createClientDto = new ClientDto(Guid.Empty, FirstName, FamilyName, new AddressDto(City), new ContactDetailsDto(PrimaryPhoneNumber), tenantId);
            tenantProviderMock.SetTenantId(Guid.Empty);

            // Act
            var result = await controller.Create(createClientDto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateClient_ValidDto_ReturnsOk()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            var createClientDto = new ClientDto(Guid.Empty, FirstName, FamilyName, new AddressDto(City), new ContactDetailsDto(PrimaryPhoneNumber), tenantId);
            var clientNewId = Guid.NewGuid();
            tenantProviderMock.SetTenantId(tenantId);

            // mock the controller http context
            MockHttpContext(controller);

            mediatorMock.Send(Arg.Any<CreateClientCommand>(), Arg.Any<CancellationToken>()).Returns(clientNewId);

            // Act
            var result = await controller.Create(createClientDto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task SearchClients_NullDto_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            tenantProviderMock.SetTenantId(Guid.NewGuid());

            // Act
            var result = await controller.Search(null!, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task SearchClients_InvalidTenantId_ReturnsBadRequest()
        {
            // Arrange
            var tenantId = Guid.Empty;
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            var clientSearchDto = new ClientSearchDto(FirstName, FamilyName, City);
            tenantProviderMock.SetTenantId(Guid.Empty);

            // Act
            var result = await controller.Search(clientSearchDto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task SearchClients_ValidDto_ReturnsOk()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            var clientSearchDto = new ClientSearchDto(FirstName, FamilyName, City);
            var client = new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), tenantId);
            var clientList = new List<Client> { client };
            tenantProviderMock.SetTenantId(tenantId);

            mediatorMock.Send(Arg.Any<SearchClientsQuery>(), Arg.Any<CancellationToken>()).Returns(clientList);

            // Act
            var result = await controller.Search(clientSearchDto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteClient_EmptyId_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            tenantProviderMock.SetTenantId(Guid.NewGuid());

            // Act
            var result = await controller.Delete(Guid.Empty, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteClient_InvalidTenantId_ReturnsBadRequest()
        {
            // Arrange
            var tenantId = Guid.Empty;
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            tenantProviderMock.SetTenantId(Guid.Empty);

            // Act
            var result = await controller.Delete(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteClient_ValidId_ReturnsOk()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            var clientId = Guid.NewGuid();
            tenantProviderMock.SetTenantId(tenantId);

            // mock the controller http context
            MockHttpContext(controller);

            // Act
            var result = await controller.Delete(clientId, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task UpdateClient_NullDto_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            tenantProviderMock.SetTenantId(Guid.NewGuid());

            // Act
            var result = await controller.UpdateClient(Guid.Empty, null!, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateClient_EmptyId_ReturnsBadRequest()
        {
            // Arrange
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            tenantProviderMock.SetTenantId(Guid.NewGuid());

            // Act
            var result = await controller.UpdateClient(Guid.Empty, new ClientDto(Guid.Empty, FirstName, FamilyName, new AddressDto(City), new ContactDetailsDto(PrimaryPhoneNumber), Guid.NewGuid()), CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateClient_InvalidTenantId_ReturnsBadRequest()
        {
            // Arrange
            var tenantId = Guid.Empty;
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            var clientDto = new ClientDto(Guid.Empty, FirstName, FamilyName, new AddressDto(City), new ContactDetailsDto(PrimaryPhoneNumber), Guid.NewGuid());
            tenantProviderMock.SetTenantId(Guid.Empty);

            // Act
            var result = await controller.UpdateClient(Guid.NewGuid(), clientDto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateClient_ValidDto_ReturnsOk()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var mediatorMock = Substitute.For<IMediator>();
            var controller = new ClientsController(loggerMock, _mapper, mediatorMock, tenantProviderMock);
            var clientDto = new ClientDto(Guid.NewGuid(), FirstName, FamilyName, new AddressDto(City), new ContactDetailsDto(PrimaryPhoneNumber), tenantId);
            var client = new Client(FirstName, FamilyName, new Address(City), new ContactDetails(PrimaryPhoneNumber), tenantId);
            clientDto.Id = Guid.NewGuid();
            client.Id = clientDto.Id;
            tenantProviderMock.SetTenantId(tenantId);

            // mock the controller http context
            MockHttpContext(controller);
            mediatorMock.Send(Arg.Any<UpdateClientCommand>(), Arg.Any<CancellationToken>()).Returns(client);
            // Act
            var result = await controller.UpdateClient(Guid.NewGuid(), clientDto, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        private static void MockHttpContext(ClientsController controller)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer " + Guid.NewGuid().ToString();
            httpContext.Request.Scheme = "http";
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
        }
    }
}
