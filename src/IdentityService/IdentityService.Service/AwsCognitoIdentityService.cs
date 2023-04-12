using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using IdentityService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityService.Service
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

            var request = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "PASSWORD", password }
                }
            };

            var response = await _client.InitiateAuthAsync(request, cancellationToken).ConfigureAwait(false);

            var tokenResponse = new TokenResponse
            {
                IdToken = response.AuthenticationResult.IdToken,
                RefreshToken = response.AuthenticationResult.RefreshToken,
                AccessToken = response.AuthenticationResult.AccessToken,
                ExpiresIn = response.AuthenticationResult.ExpiresIn,
                TokenType = response.AuthenticationResult.TokenType
            };

            return tokenResponse;
        }

        public async Task<ITokenResponse?> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentException("Refresh token cannot be null or empty");
            }

            var request = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    {
                        "REFRESH_TOKEN", refreshToken
                    }
                }
            };

            var response = await _client.InitiateAuthAsync(request);
            TokenResponse tokenResponse = BuildTokenResponse(response);

            return tokenResponse;
        }

        public async Task<ITokenResponse?> LoginJwtAsync(string username, string password, CancellationToken cancellationToken)
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

                // TODO: Wrap in Cancellation Token
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

        public async Task<bool> SignoutAsync(string accessToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentException(nameof(accessToken));
            }

            var signoutRequest = new GlobalSignOutRequest
            {
                AccessToken = accessToken
            };

            var signoutResponse = await _client.GlobalSignOutAsync(signoutRequest, cancellationToken).ConfigureAwait(false);

            return signoutResponse.HttpStatusCode is System.Net.HttpStatusCode.OK;
        }

        private TokenResponse BuildTokenResponse(InitiateAuthResponse response)
        {
            return new TokenResponse
            {
                IdToken = response.AuthenticationResult.IdToken,
                RefreshToken = response.AuthenticationResult.RefreshToken,
                AccessToken = response.AuthenticationResult.AccessToken,
                ExpiresIn = response.AuthenticationResult.ExpiresIn,
                TokenType = response.AuthenticationResult.TokenType
            };
        }
    }
}
