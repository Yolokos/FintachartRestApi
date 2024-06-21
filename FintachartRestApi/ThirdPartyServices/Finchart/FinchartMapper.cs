using Database.Models;
using FintachartRestApi.Common;

namespace FintachartRestApi.ThirdPartyHelpers.Finchart
{
	public class FinchartMapper
	{
		public static IEnumerable<Asset> ToAssets (FinchartApiResponse finchartResponse) {
			foreach(var rawAsset in finchartResponse.data) {
				yield return new Asset()
				{
					Id = rawAsset.id,
					Symbol  = rawAsset.symbol,
					Kind = rawAsset.kind,
					Exchange = rawAsset.exchange,
					Description = rawAsset.description,
					TickSize = rawAsset.tickSize,
					Currency = rawAsset.currency,
					BaseCurrency = rawAsset.baseCurrency,
					Mappings = rawAsset.mappings.ToDictionary(k => k.Key, am => new AssetMapping(am.Value.symbol, am.Value.exchange, am.Value.defaultOrderSize))
				};
			}
		}
	}
}
