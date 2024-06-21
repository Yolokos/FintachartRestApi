using Database.Models;
using Database.Repositories;
using FintachartRestApi.ThirdPartyHelpers.Finchart;
using FintachartRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using FintachartRestApi.Models.Finchart;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace FintachartRestApi.Controllers
{
	[Route("api/assets")]
	[ApiController]
	public class AssetController : ControllerBase
	{
		private readonly AssetService _assetService;
		private readonly FinchartWebsocketService _finchartWebsocketService;

		public AssetController (MongoRepository mongoRepository, FinchartWebsocketService finchartWebsocketService)
		{
			_assetService = new AssetService(mongoRepository);
			_finchartWebsocketService = finchartWebsocketService;
		}


		[HttpGet]
		public async Task<JsonResult> GetAssets ([FromQuery] string[] assetIds)
		{
			List<Asset> resultAssets = new();

			foreach (var id in assetIds)
			{
				var asset = await _assetService.GetAsync(id);

				resultAssets.Add(asset);
			}

			return new JsonResult(resultAssets) { 
				StatusCode = 200
			};
        }

		[HttpGet("prices")]
		public async Task<JsonResult> GetAssetsPrice ([FromQuery] string[] assetIds)
		{
			List<FinchartWebsocketAssetPriceMessage> resultAssetsPrices = new();

			foreach (var id in assetIds)
			{
				var finchartResponse = await _finchartWebsocketService.GetAssetPriceInfo(id);

				resultAssetsPrices.Add(finchartResponse.AssetPrice);
			}

			return new JsonResult(resultAssetsPrices)
			{
				StatusCode = 200
			};
		}
	}
}
