using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Designly.Auth.Models;
using Designly.Base;
using Designly.Base.Exceptions;
using IdentityService.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Designly.Auth.Providers
{
    public class AwsCognitoIdentityService : IIdentityService
    {
        private readonly ILogger<AwsCognitoIdentityService> _logger;
        private readonly string _clientId;
        private readonly string _poolId;
        
        private readonly AmazonCognitoIdentityProviderClient _client;

        public AwsCognitoIdentityService(
            IOptions<IdentityProviderConfiguration> AWSCognitoConfiguration,
            ILogger<AwsCognitoIdentityService> logger)
        {
            _clientId = AWSCognitoConfiguration.Value.ClientId;
            _poolId = AWSCognitoConfiguration.Value.PoolId;
            var _region = AWSCognitoConfiguration.Value.Region;

            if (string.IsNullOrEmpty(_clientId))
            {
                throw new ConfigurationException($"Invalid value for {nameof(_client)} : must not be null or empty.");
            }
            if (string.IsNullOrEmpty(_poolId))
            {
                throw new ConfigurationException($"Invalid value for {nameof(_poolId)} : must not be null or empty");
            }
            if (string.IsNullOrEmpty(_region))
            {
                throw new ConfigurationException("Region configuration is not set or empty");
            }

            var awsRegion = RegionEndpoint.GetBySystemName(_region);

            _client = new AmazonCognitoIdentityProviderClient(awsRegion);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("AWS Region is set to {awsRegion}", awsRegion.DisplayName);
            }
            
        }

        public async Task<ITokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogError($"{nameof(username)} must not be null or empty");
                throw new ConfigurationException($"{nameof(username)} must not be null or empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                _logger.LogError($"{nameof(password)} must not be null or empty");
                throw new ConfigurationException($"{nameof(password)} must not be null or empty");
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

            var response = await _client.InitiateAuthAsync(request, cancellationToken);
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
                cancellationToken.ThrowIfCancellationRequested();

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
            catch (NotAuthorizedException notAuthorizedException)
            {
                _logger.LogInformation(notAuthorizedException, "Could not perform signin against AWS Cognito due to error: {Message}", notAuthorizedException.Message);
                throw new BusinessLogicException(AuthenticationErrors.InvalidCredentials(notAuthorizedException.Message));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not perform signin against AWS Cognito due to error: {Message}", exception.Message);
                return null;
            }
        }

        public async Task<bool> SignoutAsync(string accessToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var signoutRequest = new GlobalSignOutRequest
            {
                AccessToken = accessToken
            };

            var signoutResponse = await _client.GlobalSignOutAsync(signoutRequest, cancellationToken).ConfigureAwait(false);

            return signoutResponse.HttpStatusCode is System.Net.HttpStatusCode.OK;
        }

        private static TokenResponse BuildTokenResponse(InitiateAuthResponse response)
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

        public async Task<bool> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(email);
            ArgumentException.ThrowIfNullOrEmpty(firstName);
            ArgumentException.ThrowIfNullOrEmpty(lastName);

            var request = new AdminCreateUserRequest
            {
                UserPoolId = _poolId,
                Username = email,
                UserAttributes =
                [
                    new AttributeType
                    {
                        Name = "email",
                        Value = email
                    },
                    new AttributeType
                    {
                        Name = "given_name",
                        Value = firstName
                    },
                    new AttributeType
                    {
                        Name = "family_name",
                        Value = lastName
                    }
                ],
                TemporaryPassword = "Changeme1!",
                DesiredDeliveryMediums =
                [
                    "EMAIL"
                ]
            };

            try
            {
                var response = await _client.AdminCreateUserAsync(request, cancellationToken).ConfigureAwait(false);
                return response.HttpStatusCode is System.Net.HttpStatusCode.OK;
            } 
            catch (UsernameExistsException exception)
            {
                _logger.LogError(exception, "Could not create user in AWS Cognito due to error: {Message}", exception.Message);
                throw new BusinessLogicException(AuthenticationErrors.UsernameExists(exception.Message));
            }
            catch (TooManyRequestsException tooManyRequestsException)
            {
                Error error = new("TooManyRequests", tooManyRequestsException.Message);
                _logger.LogError(tooManyRequestsException, "Could not create user in AWS Cognito due to error: {Message}", tooManyRequestsException.Message);
                throw new BusinessLogicException(error);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create user in AWS Cognito due to error: {Message}", exception.Message);
                return false;
            }
        }

        /// <summary>
        /// Create a group in AWS Cognito
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> CreateGroupAsync(string groupName, string groupDescription, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(groupName);
            ArgumentException.ThrowIfNullOrEmpty(groupDescription);

            var request = new CreateGroupRequest
            {
                GroupName = groupName,
                Description = groupDescription,
                UserPoolId = _poolId
            };

            try
            {
                var response = await _client.CreateGroupAsync(request, cancellationToken).ConfigureAwait(false);
                return response.HttpStatusCode is System.Net.HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not create group in AWS Cognito due to error: {Message}", exception.Message);
                return false;
            }
        }

        /// <summary>
        /// Adds a user to a group in AWS Cognito
        /// </summary>
        /// <param name="email"></param>
        /// <param name="groupName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> AddUserToGroupAsync(string email, string groupName, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(email);
            ArgumentException.ThrowIfNullOrEmpty(groupName);

            var request = new AdminAddUserToGroupRequest
            {
                GroupName = groupName,
                UserPoolId = _poolId,
                Username = email
            };

            try
            {
                var response = await _client.AdminAddUserToGroupAsync(request, cancellationToken).ConfigureAwait(false);
                return response.HttpStatusCode is System.Net.HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not add user to group in AWS Cognito due to error: {Message}", exception.Message);
                return false;
            }
        }

        public async Task<bool> SetUserPasswordAsync(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(email);
            ArgumentException.ThrowIfNullOrEmpty(password);

            var request = new AdminSetUserPasswordRequest
            {
                UserPoolId = _poolId,
                Username = email,
                Password = password,
                Permanent = true
            };

            try
            {
                var response = await _client.AdminSetUserPasswordAsync(request, cancellationToken).ConfigureAwait(false);
                return response.HttpStatusCode is System.Net.HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not set user password in AWS Cognito due to error: {Message}", exception.Message);
                return false;
            }
        }
    }
}
