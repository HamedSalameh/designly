using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Flow.SharedKernel.Interfaces;
using Flow.SharedKernel.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flow.IdentityService
{ 
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

        public async Task<ITokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogError($"{nameof(username)} must not be null or empty");
                throw new ArgumentException($"{nameof(username)} must not be null or empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                _logger.LogError($"{nameof(password)} must not be null or empty");
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

                var tokenResponse = new TokenResponse
                {
                    IdToken = authResponse.AuthenticationResult.IdToken,
                    AccessToken = authResponse.AuthenticationResult.AccessToken,
                    ExpiresIn = authResponse.AuthenticationResult.ExpiresIn,
                    TokenType = authResponse.AuthenticationResult.TokenType
                };

                return tokenResponse;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not perform signin against AWS Cognito due to error: {exception.Message}");
                return null;
            }
        }
    }
}
