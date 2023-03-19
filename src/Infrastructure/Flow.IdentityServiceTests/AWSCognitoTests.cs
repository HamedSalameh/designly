using Amazon.CognitoIdentity.Model;
using Flow.IdentityService;
using Flow.SharedKernel.Interfaces;

namespace Flow.IdentityServiceTests
{
    public class AWSCognitoTests
    {
        public IIdentityService IdentityService { get; set; }

        internal string? username = default;
        internal string? password = default;

        [SetUp]
        public void Setup()
        {
            IdentityService = new AWSCongnito();
        }

        [Test]
        [TestCase("hamedsalami@gmail.com", "%V9O$%H3xlVX1Rl&!9u8xlyb")]
        public async Task LoginAsync(string username, string password)
        {
            var response = await IdentityService.LoginAsync(username, password);
            
            Assert.NotNull(response);
            Assert.That(response.UserID, Is.SameAs(username));
        }

        [Test]
        [TestCase("hamedsalami@gmail.com", "bad-password")]
        public async Task LoginAsyncFails(string username, string password)
        {
            Assert.ThrowsAsync<Amazon.CognitoIdentityProvider.Model.NotAuthorizedException>( () => IdentityService.LoginAsync(username, password));            
        }
    }
}