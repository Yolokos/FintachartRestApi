using FintachartRestApi.Common;
using FintachartRestApi.ThirdPartyServices.Responses;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace FintachartRestApi.ThirdPartyHelpers.Finchart
{
	public class FinchartAuthService
	{
		private readonly string _apiUrl;
		private readonly string _clientId;
		private readonly string _userName;
		private readonly string _password;

		private const string _getAuthTokenEndpoint = "/identity/realms/fintatech/protocol/openid-connect/token";

		private readonly ILogger<FinchartAuthService> _logger;

		public FinchartAuthService(IOptions<FinchartApiSettings> finchartSettings, ILogger<FinchartAuthService> logger)
        {
			_apiUrl = finchartSettings.Value.Uri;
			_clientId = finchartSettings.Value.ClientId;
			_userName = finchartSettings.Value.UserName;
			_password = finchartSettings.Value.Password;

			_logger = logger;
		}

        public async Task<FinchartResponse> GetAccessToken ()
		{
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
				{
					Address = $"{_apiUrl}{_getAuthTokenEndpoint}",
					ClientId = _clientId,
					UserName = _userName,
					Password = _password
				});

				if (!string.IsNullOrEmpty(response.AccessToken))
				{
					return new FinchartResponse() { StatusCode = 200, AccessToken = response.AccessToken };
				}
				else
				{
					_logger.LogError($"{GetType().Name}:{MethodBase.GetCurrentMethod().Name}. Status code: {response.HttpStatusCode}. Error message: {response.ErrorDescription}");
					return new FinchartResponse() { StatusCode = 400, Message = response.ErrorDescription };
				}
			}
		}
	}
}
