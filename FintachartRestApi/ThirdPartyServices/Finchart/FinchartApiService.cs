using Database.Models;
using FintachartRestApi.Common;
using FintachartRestApi.ThirdPartyServices.Responses;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using NewtonsoftJsonConvert = Newtonsoft.Json.JsonConvert;

namespace FintachartRestApi.ThirdPartyHelpers.Finchart
{
	public class FinchartApiService
	{
		private readonly string _apiUrl;

		private const string _getAllAssetsEndpoint = "/api/instruments/v1/instruments";

		private readonly ILogger<FinchartApiService> _logger;
		private readonly FinchartAuthService _finchartAuthService;

		public FinchartApiService (IOptions<FinchartApiSettings> finchartSettings, FinchartAuthService finchartAuthService, ILogger<FinchartApiService> logger)
		{
			_logger = logger;
			_finchartAuthService = finchartAuthService;

			_apiUrl = finchartSettings.Value.Uri;
		}

		public async Task<FinchartResponse> LoadAssets ()
		{
			using (var httpClient = new HttpClient())
			{
				List<Asset> resultAssetList = new();
				var finchartResponse = await _finchartAuthService.GetAccessToken();

				if (finchartResponse.StatusCode != StatusCodes.Status200OK)
					return new FinchartResponse { StatusCode = StatusCodes.Status400BadRequest };


				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", finchartResponse.AccessToken);

				var response = await httpClient.GetAsync(_apiUrl + _getAllAssetsEndpoint);

				if (response.StatusCode != (HttpStatusCode)StatusCodes.Status200OK)
				{
					_logger.LogError($"{GetType().Name}:{MethodBase.GetCurrentMethod().Name}. Status code: {response.StatusCode}.");

					return new FinchartResponse() { StatusCode = (int)response.StatusCode };
				}

				var data = await response.Content.ReadAsStringAsync();

				var finchartApiResponse = NewtonsoftJsonConvert.DeserializeObject<FinchartApiResponse>(data);

				return new FinchartResponse()
				{
					Assets = FinchartMapper.ToAssets(finchartApiResponse),
					StatusCode = StatusCodes.Status200OK
				};
			}
		}
	}
}
