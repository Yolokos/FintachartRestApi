namespace FintachartRestApi.Models.Finchart
{
	public record FinchartWebsocketSubscriptionMessage (
		string Type,
		string InstrumentId,
		string Provider,
		bool Subscribe,
		string[] Kinds) : FinchartWebsocketTypeMessage (Type);
}
