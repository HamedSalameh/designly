using Microsoft.Extensions.Logging;
using NSubstitute;
using Projects.Application.Features.CreateProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Tests
{
    [TestFixture]
    public class CreateProjectCommandHandlerTests
    {
        private CreateProjectCommandHandler _handler;
        // NSubstitute for ILogger as Mock
        private readonly ILogger<CreateProjectCommandHandler> _logger = Substitute.For<ILogger<CreateProjectCommandHandler>>();

        [SetUp]
        public void Setup()
        {

            _handler = new CreateProjectCommandHandler(_logger);
        }

        [Test]
        public async Task Should_create_project()
        {
            var command = new CreateProjectCommand
            {
                TenantId = Guid.NewGuid(),
                Name = "Test Project",
                ProjectLeadId = Guid.NewGuid(),
                ClientId = Guid.NewGuid()
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        }
    }
}
