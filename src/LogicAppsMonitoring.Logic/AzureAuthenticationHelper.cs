using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;

namespace LogicAppsMonitoring.Logic
{
    public class AzureAuthenticationHelper
    {
        private const string AuthenticationContextAuthority = "https://login.windows.net/";
        private const string AuthenticationContextResource = "https://management.core.windows.net/";
        private const double TokenCredentialsTimeToLiveInMinutes = 3;
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private TokenCredentials _tokenCredentials;
        private DateTime _tokenDateTime = DateTime.MinValue;

        public AzureAuthenticationHelper(string tenantId, string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(tenantId))
                throw new ArgumentException();
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException();
            if (string.IsNullOrEmpty(clientSecret))
                throw new ArgumentException();

            _clientId = clientId;
            _clientSecret = clientSecret;
            _tenantId = tenantId;
        }

        public TokenCredentials GetTokenCredentials()
        {
            CreateTokenCredentials();

            return _tokenCredentials;
        }

        private void CreateTokenCredentials()
        {
            if (!IsValidToken())
            {
                var authenticationContext = new AuthenticationContext(AuthenticationContextAuthority + _tenantId);
                var credential = new ClientCredential(_clientId, _clientSecret);
                var result = authenticationContext.AcquireTokenAsync(AuthenticationContextResource, credential);

                _tokenCredentials = new TokenCredentials(result.Result.CreateAuthorizationHeader().Substring("Bearer ".Length));
                _tokenDateTime = DateTime.UtcNow;
            }
        }

        private bool IsValidToken()
        {
            return _tokenCredentials != null &&
                   DateTime.UtcNow < _tokenDateTime.AddMinutes(TokenCredentialsTimeToLiveInMinutes);
        }
    }
}
