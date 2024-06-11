using Designly.Auth.Identity;
using Designly.Filter;
using Microsoft.Extensions.Logging;
using Moq;
using Projects.Application.Builders;
using Projects.Application.Features.SearchTasks;
using Projects.Domain.StonglyTyped;
using Projects.Domain.Tasks;
using Projects.Infrastructure.Filter;
using Projects.Infrastructure.Interfaces;
using SqlKata;

namespace Projects.Tests.Tasks
{
    [TestFixture]
    public class SearchTasksCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<SearchTasksCommandHandler>> _loggerMock;
        private Mock<IQueryBuilder> _queryBuilderMock;
        private Mock<ITenantProvider> _tenantProviderMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<SearchTasksCommandHandler>>();
            _queryBuilderMock = new Mock<IQueryBuilder>();
            _tenantProviderMock = new Mock<ITenantProvider>();
        }

        [Test]
        public async Task Should_Return_Empty_List_When_No_Tasks_Found()
        {
            // Arrange
            var sut = new SearchTasksCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _queryBuilderMock.Object);
            var mockTenantId = TenantId.New;
            var mockProjectId = ProjectId.New;
            var mockFilterConditions = new List<FilterCondition>();
            var mockFilterDefinition = new FilterDefinition(TaskItemFieldToColumnMapping.TaskItemTable, mockFilterConditions);
            var mockSqlResult = new Mock<SqlResult>();

            var request = new SearchTasksCommand
            {
                TenantId = mockTenantId,
                ProjectId = mockProjectId,
                Filters = mockFilterConditions
            };

            _queryBuilderMock.Setup(x => x.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns(new SqlResult());

            _unitOfWorkMock.Setup(x => x.TaskItemsRepository.SearchAsync(mockTenantId, mockSqlResult.Object, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<TaskItem>)[]);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);

            // extract the value from the IResult
            var taskItems = result.Match(
                success => success,
                failure => throw new Exception("Expected success but got failure"));

            Assert.That(taskItems, Is.Empty);;
        }

        [Test]
        public async Task Should_Return_Task_Items_When_Found()
        {
            // Arrange
            var sut = new SearchTasksCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _queryBuilderMock.Object);
            var mockTenantId = TenantId.New;
            var mockProjectId = ProjectId.New;
            var mockFilterConditions = new List<FilterCondition>();
            var mockFilterDefinition = new FilterDefinition(TaskItemFieldToColumnMapping.TaskItemTable, mockFilterConditions);
            var mockSqlResult = new Mock<SqlResult>();
            var mockTaskItems = new List<TaskItem>
            {
                GetRandomTaskItem(mockTenantId, mockProjectId),
                GetRandomTaskItem(mockTenantId, mockProjectId)
            };

            var request = new SearchTasksCommand
            {
                TenantId = mockTenantId,
                ProjectId = mockProjectId,
                Filters = mockFilterConditions
            };

            _queryBuilderMock.Setup(x => x.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns(new SqlResult());

            _unitOfWorkMock.Setup(x => x.TaskItemsRepository.SearchAsync(mockTenantId, It.IsAny<SqlResult>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTaskItems);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);

            // extract the value from the IResult
            var taskItems = result.Match(
                               success => success,
                                              failure => throw new Exception("Expected success but got failure"));

            Assert.That(taskItems, Is.Not.Empty);
            Assert.That(taskItems.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Should_ReturnFailure_InCaseSqlResultIsNull()
        {
            // Arrange
            var sut = new SearchTasksCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _queryBuilderMock.Object);
            var mockTenantId = TenantId.New;
            var mockProjectId = ProjectId.New;
            var mockFilterConditions = new List<FilterCondition>();
            var mockFilterDefinition = new FilterDefinition(TaskItemFieldToColumnMapping.TaskItemTable, mockFilterConditions);
            var mockSqlResult = new Mock<SqlResult>();

            var request = new SearchTasksCommand
            {
                TenantId = mockTenantId,
                ProjectId = mockProjectId,
                Filters = mockFilterConditions
            };

            _queryBuilderMock.Setup(x => x.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns((SqlResult)null!);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            var value = result.Match(
                 res => string.Empty,
                 ex => ex.Message);

            Assert.That(value, Is.EqualTo("The query builder failed to build the query."));
        }

        private TaskItem GetRandomTaskItem(TenantId tenantId, ProjectId projectId)
        {

            var taskItemBuilder = new TaskItemBuilder(_tenantProviderMock.Object);

            _tenantProviderMock.Setup(x => x.GetTenantId())
                .Returns(tenantId);

            var taskItem = taskItemBuilder.CreateTaskItem(Guid.NewGuid().ToString(), projectId, Guid.NewGuid().ToString())
                .WithAssignedBy(Guid.NewGuid())
                .WithAssignedTo(Guid.NewGuid())
                .Build();

            return taskItem;
        }
    }
}
