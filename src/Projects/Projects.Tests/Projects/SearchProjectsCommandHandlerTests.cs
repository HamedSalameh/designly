

using Designly.Auth.Identity;
using Designly.Filter;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Builders;
using Projects.Application.Features.SearchProjects;
using Projects.Domain;
using Projects.Domain.StonglyTyped;
using Projects.Infrastructure.Filter;
using Projects.Infrastructure.Interfaces;
using SqlKata;

namespace Projects.Tests.Projects
{
    [TestFixture]
    public class SearchProjectsCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<SearchProjectsCommandHandler>> _loggerMock;
        private Mock<IQueryBuilder> _queryBuilderMock;
        private Mock<ITenantProvider> _tenantProviderMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<SearchProjectsCommandHandler>>();
            _queryBuilderMock = new Mock<IQueryBuilder>();
            _tenantProviderMock = new Mock<ITenantProvider>();
        }

        [Test]
        public async Task Should_Return_Empty_List_When_No_Projects_Found()
        {
            // Arrange
            var sut = new SearchProjectsCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _queryBuilderMock.Object);
            var mockTenantId = TenantId.New;
            var mockFilterConditions = new List<FilterCondition>();
            var mockFilterDefinition = new FilterDefinition(ProjectFieldToColumnMapping.ProjectsTable, mockFilterConditions);
            var mockSqlResult = new Mock<SqlResult>();

            var request = new SearchProjectsCommand
            {
                TenantId = mockTenantId,
                FilterConditions = mockFilterConditions
            };

            _queryBuilderMock.Setup(x => x.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns(new SqlResult());

            _unitOfWorkMock.Setup(x => x.ProjectsRepository.SearchProjectsAsync(mockTenantId, mockSqlResult.Object, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<BasicProject>)[]);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);

            // extract the value from the IResult
            var projects = result.Match(
                               success => success,
                               failure => throw new Exception("Expected success but got failure"));

            Assert.That(projects, Is.Empty);;
        }

        [Test]
        public async Task Should_Return_BasicProjects_When_Found()
        {
            var sut = new SearchProjectsCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _queryBuilderMock.Object);
            var mockTenantId = TenantId.New;
            var mockFilterConditions = new List<FilterCondition>();
            var mockFilterDefinition = new FilterDefinition(ProjectFieldToColumnMapping.ProjectsTable, mockFilterConditions);
            var mockSqlResult = new Mock<SqlResult>();
            var basicProjectBuilder = new ProjectBuilder(_tenantProviderMock.Object);

            _tenantProviderMock.Setup(x => x.GetTenantId())
                .Returns(mockTenantId);

            var mockProject = basicProjectBuilder
                .WithProjectLead(ProjectLeadId.New)
                .WithClient(ClientId.New)
                .WithName("Project Name")
                .WithDescription("Project Description")
                .WithStartDate(DateOnly.FromDateTime(DateTime.Now))
                .WithDeadline(DateOnly.FromDateTime(DateTime.Now.AddDays(1)))
                .BuildBasicProject();

            var mockProjects = new List<BasicProject>
            {
                mockProject
            };

            var request = new SearchProjectsCommand
            {
                TenantId = mockTenantId,
                FilterConditions = mockFilterConditions
            };

            _queryBuilderMock.Setup(x => x.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns(new SqlResult());

            _unitOfWorkMock.Setup(x => x.ProjectsRepository.SearchProjectsAsync(mockTenantId, It.IsAny<SqlResult>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockProjects);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);

            // extract the value from the IResult
            var projects = result.Match(
                               success => success,
                                              failure => throw new Exception("Expected success but got failure"));

            Assert.That(projects, Is.Not.Empty);
            Assert.That(projects.Length, Is.EqualTo(1));
        }

        [Test]
        public async Task Should_Return_Error_When_QueryBuilder_Fails()
        {
            var sut = new SearchProjectsCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _queryBuilderMock.Object);
            var mockTenantId = TenantId.New;
            var mockFilterConditions = new List<FilterCondition>();
            var mockFilterDefinition = new FilterDefinition(ProjectFieldToColumnMapping.ProjectsTable, mockFilterConditions);
            var mockSqlResult = new Mock<SqlResult>();

            var request = new SearchProjectsCommand
            {
                TenantId = mockTenantId,
                FilterConditions = mockFilterConditions
            };

            _queryBuilderMock.Setup(x => x.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns((SqlResult)null);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            Assert.That(result.IsSuccess, Is.False);
            var value = result.Match(
                 res => string.Empty,
                 ex => ex.Message);

            Assert.That(value, Is.EqualTo("The query builder failed to build the query."));
        }
    }
}
