using Microsoft.Extensions.Logging;
using NSubstitute;
using Projects.Application.Features.DeleteProject;

namespace Projects.Tests
{
    public class DeleteProjectCommandHandlerTests
    {
        private DeleteProjectCommandHandler _handler;
        // NSubstitute for ILogger as Mock
        private readonly ILogger<DeleteProjectCommandHandler> _logger = Substitute.For<ILogger<DeleteProjectCommandHandler>>();

        [SetUp]
        public void Setup()
        {

            _handler = new DeleteProjectCommandHandler(_logger);
        }

        [Test]
        public async Task Should_delete_project()
        {
            var command = new DeleteProjectCommand
            {
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid()
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.True);
        }
    }
}
