using Clients.API.Middleware;
using Clients.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clients.Tests
{
    public class ValidationExceptionHandingMiddlewareTests
    {
        [Test]
        public async Task Invoke_Should_Handle_ValidationException()
        {
            var defaultContext = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ValidationExceptionHandingMiddleware>>();
            var requestDelegateMock = new Mock<RequestDelegate>();
            
            requestDelegateMock.Setup(x => x(defaultContext))
                .Throws(new ValidationException("Validation exception occurred"));
            
            var middleware = new ValidationExceptionHandingMiddleware(requestDelegateMock.Object, loggerMock.Object);
            
            //act
            await middleware.Invoke(defaultContext);
            
            //assert
            Assert.That(defaultContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            
            requestDelegateMock.Verify(x => x(defaultContext), Times.Once);
        }
        
        [Test]
        public void Invoke_Should_Not_Handle_ValidationException()
        {
            var defaultContext = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ValidationExceptionHandingMiddleware>>();
            var requestDelegateMock = new Mock<RequestDelegate>();
            
            requestDelegateMock.Setup(x => x(defaultContext))
                .Throws(new Exception("Exception occurred"));
            
            var middleware = new ValidationExceptionHandingMiddleware(requestDelegateMock.Object, loggerMock.Object);
            
            //assert
            Assert.ThrowsAsync<Exception>( async () => await middleware.Invoke(defaultContext));
            
            requestDelegateMock.Verify(x => x(defaultContext), Times.Once);
        }
    }
}