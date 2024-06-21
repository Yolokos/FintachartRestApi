using Database.Models;
using FintachartRestApi.Models.Finchart;
using FintachartRestApi.ThirdPartyServices.Responses.Base;

namespace FintachartRestApi.ThirdPartyServices.Responses
{
	public class FinchartResponse : BaseResponse
	{
		public string AccessToken { get; set; }
		public IEnumerable<Asset> Assets { get; set; }
		public FinchartWebsocketAssetPriceMessage AssetPrice { get; set; } 
	}
}
