using static FintachartRestApi.Models.Finchart.FinchartWebsocketAssetPriceMessage;

namespace FintachartRestApi.Models.Finchart
{
	public record FinchartWebsocketAssetPriceMessage
	{
		public LastAssetPrice Last { get; set; }
		public AskAssetPrice Ask { get; set; }
		public BidAssetPrice Bid { get; set; }

		public record LastAssetPrice (DateTime TimeStamp, decimal Price, decimal Volume, double Change, double ChangePct);
		public record AskAssetPrice (DateTime TimeStamp, decimal Price, decimal Volume);
		public record BidAssetPrice (DateTime TimeStamp, decimal Price, decimal Volume);
	}
}