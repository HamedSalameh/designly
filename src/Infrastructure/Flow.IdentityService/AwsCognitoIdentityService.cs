using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime.Internal.Util;
using Flow.SharedKernel.Interfaces;
using Flow.SharedKernel.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flow.IdentityService
{
    //public class AWSCongnito : IIdentityService
    //{
    //    private readonly AmazonCognitoIdentityProviderClient _provider;
    //    private readonly CognitoUserPool _userPool;
    //    private string AppClientId = "37jb2mi88p635sl5k6si2metde";

    //    public AWSCongnito()
    //    {
    //        _provider =
    //            new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());
    //        _userPool = new CognitoUserPool("us-east-1_t7SXW9J4E", AppClientId, _provider);
    //    }

    //    public async Task<User> LoginAsync(string username, string password)
    //    {
    //        var authenticatedUser = await AuthenticateUserAsync(username, password);

    //        return authenticatedUser;
    //    }

    //    private async Task<User> AuthenticateUserAsync(string emailAddress, string password)
    //    {
    //        CognitoUser user = new CognitoUser(emailAddress, AppClientId, _userPool, _provider);
    //        InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
    //        {
    //            Password = password
    //        };

    //        AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest);
    //        var result = authResponse.AuthenticationResult;

    //        var authenticatedUser = new User(user.UserID, user.Username);
    //        authenticatedUser.ExpiresIn = result.ExpiresIn;
    //        authenticatedUser.AccessToken = result.AccessToken;
    //        authenticatedUser.IdToken = result.IdToken;
    //        authenticatedUser.RefreshToken = result.RefreshToken;
    //        authenticatedUser.TokenType = result.TokenType;

    //        // return new Tuple<CognitoUser, AuthenticationResultType>(user, result);
    //        return authenticatedUser;
    //    }
    //}

    public class AwsCognitoIdentityService : IIdentityService
    {
        private readonly ILogger<AwsCognitoIdentityService> _logger;
        private readonly string _clientId;
        private readonly string _poolId;
        private readonly AmazonCognitoIdentityProviderClient _client;

        public AwsCognitoIdentityService( 
            IOptions<AWSCognitoConfiguration> AWSCognitoConfiguration,
            ILogger<AwsCognitoIdentityService> logger)
        {
            _clientId = AWSCognitoConfiguration.Value.ClientId;
            _poolId = AWSCognitoConfiguration.Value.PoolId;

            if (string.IsNullOrEmpty(_clientId))
            {
                throw new ArgumentException($"Invalid value for {nameof(_client)} : must not be null or empty.");
            }
            if (string.IsNullOrEmpty(_poolId))
            {
                throw new ArgumentException($"Invalid value for {nameof(_poolId)} : must not be null or empty");
            }

            
            _client = new AmazonCognitoIdentityProviderClient();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"{nameof(username)} must not be null or empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"{nameof(password)} must not be null or empty");
            }

            try
            {
                var userPool = new CognitoUserPool(_poolId, _clientId, _client);
                var user = new CognitoUser(username, _clientId, userPool, _client);
                var authRequest = new InitiateSrpAuthRequest()
                {
                    Password = password
                };
                var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
                return true;
            }
            catch (Exception exception)
            {
                // Handle any exceptions here
                return false;
            }
        }
    }
}
