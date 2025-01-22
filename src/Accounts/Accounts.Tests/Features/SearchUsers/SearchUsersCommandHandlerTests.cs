using Accounts.Application.Features.SearchUsers;
using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using Designly.Base.Exceptions;
using Designly.Filter;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Accounts.Domain.Consts;

namespace Accounts.Tests.Features.SearchUsers
{
    [TestFixture]
    public class SearchUsersCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<SearchUsersCommandHandler>> _loggerMock;
        private Mock<IQueryBuilder> _queryBuilderMock;
        private Mock<IUsersRepository> _usersRepositoryMock;
        private SearchUsersCommandHandler _handler;
        private static Guid _tenantId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<SearchUsersCommandHandler>>();
            _queryBuilderMock = new Mock<IQueryBuilder>();
            _usersRepositoryMock = new Mock<IUsersRepository>();

            _unitOfWorkMock.Setup(u => u.UsersRepository)
                .Returns(_usersRepositoryMock.Object);

            _handler = new SearchUsersCommandHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _queryBuilderMock.Object);
        }

        [Test]
        public async Task Handle_WhenQueryBuilderReturnsNull_ReturnsBusinessLogicException()
        {
            // Arrange
            SqlResult? sqlResult = null;
            var searchUsersCommand = new SearchUsersCommand(_tenantId);
            searchUsersCommand.FilterConditions = new List<FilterCondition>();
           
            _queryBuilderMock.Setup(q => q.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns(sqlResult!);

            // Act
            var handlerResult = await  _handler.Handle(searchUsersCommand, CancellationToken.None);

            // extract the handlerResult using ResultPattern
            Assert.That(handlerResult.IsFaulted, Is.True);
            var message = handlerResult.Match(
                Succ: id => string.Empty,
                Fail: ex => ex.Message);

            // Assert
            Assert.That(message, Is.EqualTo("The query builder failed to build the query."));
        }

        [Test]
        public async Task Handle_WhenQueryBuilderReturnsSqlResult_ReturnsUserDtos()
        {
            // Arrange
            var user = new User("firstName", "lastName", "email@server.com", new Account("account"));
            SqlResult sqlResult = new SqlResult();
            var searchUsersCommand = new SearchUsersCommand(_tenantId);
            searchUsersCommand.FilterConditions = new List<FilterCondition>();

            _queryBuilderMock.Setup(q => q.BuildAsync(It.IsAny<FilterDefinition>()))
                .Returns(sqlResult!);
            _unitOfWorkMock.Setup( u => u.UsersRepository.GetUsersAsync(_tenantId, sqlResult, CancellationToken.None))
                .ReturnsAsync(new List<User>() { user });

            // Act

            var handlerResult = await _handler.Handle(searchUsersCommand, CancellationToken.None);

            // extract the handlerResult using ResultPattern
            Assert.That(handlerResult.IsSuccess, Is.True);
            var users = handlerResult.Match(
                Succ: users => users,
                Fail: ex => throw new Exception("Should not fail"));

            // Assert
            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().FirstName, Is.EqualTo(user.FirstName));
            Assert.That(users.First().LastName, Is.EqualTo(user.LastName));
            Assert.That(users.First().Email, Is.EqualTo(user.Email));
        }
    }
}
