using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Flow.SharedKernel.Interfaces;
using Flow.SharedKernel.Models;

namespace Flow.IdentityService
{
    public class AWSCongnito : IIdentityService
    {
        private readonly AmazonCognitoIdentityProviderClient _provider;
        private readonly CognitoUserPool _userPool;
        private string AppClientId = "37jb2mi88p635sl5k6si2metde";

        public AWSCongnito()
        {
            _provider =
                new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());
            _userPool = new CognitoUserPool("us-east-1_t7SXW9J4E", AppClientId, _provider);
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var authenticatedUser = await AuthenticateUserAsync(username, password);

            return authenticatedUser;
        }

        private async Task<User> AuthenticateUserAsync(string emailAddress, string password)
        {
            CognitoUser user = new CognitoUser(emailAddress, AppClientId, _userPool, _provider);
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            {
                Password = password
            };

            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest);
            var result = authResponse.AuthenticationResult;

            var authenticatedUser = new User(user.UserID, user.Username);
            authenticatedUser.ExpiresIn = result.ExpiresIn;
            authenticatedUser.AccessToken = result.AccessToken;
            authenticatedUser.IdToken = result.IdToken;
            authenticatedUser.RefreshToken = result.RefreshToken;
            authenticatedUser.TokenType = result.TokenType;

            // return new Tuple<CognitoUser, AuthenticationResultType>(user, result);
            return authenticatedUser;
        }
    }
}
