using FintachartRestApi.Common;
using FintachartRestApi.Enums.Finchart;
using FintachartRestApi.Models.Finchart;
using FintachartRestApi.ThirdPartyServices.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.WebSockets;
using System.Text;

namespace FintachartRestApi.ThirdPartyHelpers.Finchart
{
	public class FinchartWebsocketService
	{
		private readonly string _websocketUri;

		private readonly ClientWebSocket _clientWebSocket;
		private readonly FinchartAuthService _finchartAuthService;

		public FinchartWebsocketService (IOptions<FinchartApiSettings> finchartSettings, FinchartAuthService finchartAuthService)
		{
			_websocketUri = finchartSettings.Value.WebsocketUri;
			_clientWebSocket = new ClientWebSocket();
			_finchartAuthService = finchartAuthService;
		}

		public async Task<FinchartResponse> GetAssetPriceInfo (string assetId)
		{
			var finchartResponse = await _finchartAuthService.GetAccessToken();
			bool subscribe = true;

			if (finchartResponse.StatusCode != StatusCodes.Status200OK)
				return new FinchartResponse { StatusCode = StatusCodes.Status400BadRequest };

			await _clientWebSocket.ConnectAsync(new Uri(string.Format(_websocketUri, finchartResponse.AccessToken)), CancellationToken.None);

			await SubscribeToAsset(assetId, subscribe);

			var receiveTimeout = new CancellationTokenSource(5000).Token;

			return await ReceiveAssetPrice(receiveTimeout);
		}

		private async Task SubscribeToAsset (string assetId, bool subscribe)
		{
			FinchartWebsocketSubscriptionMessage subscriptionMessage =
				new(FinchartWebsocketTypes.SubscriptionType,
					assetId,
					FinchartWebsocketProviders.Simulation,
					subscribe,
					new string[] { FinchartWebsocketKinds.Bid, FinchartWebsocketKinds.Ask, FinchartWebsocketKinds.Last });

			var serializerSettings = new JsonSerializerSettings();
			serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			string jsonSubscriptionMessage = JsonConvert.SerializeObject(subscriptionMessage, serializerSettings);

			var bytes = Encoding.UTF8.GetBytes(jsonSubscriptionMessage);
			await _clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
		}

		private async Task<FinchartResponse> ReceiveAssetPrice (CancellationToken cancellationToken)
		{
			var buffer = new byte[2048];
			FinchartWebsocketAssetPriceMessage finchartAssetPrice = new();

			while (true)
			{
				try
				{
					var message = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

					if (message.MessageType == WebSocketMessageType.Text)
					{
						var rawMessageContent = Encoding.UTF8.GetString(buffer, 0, message.Count);

						var finchartMessageType = JsonConvert.DeserializeObject<FinchartWebsocketTypeMessage>(rawMessageContent);

						if (finchartMessageType.Type != FinchartWebsocketTypes.UpdateType)
						{
							continue;
						}

						var finchartAssetPriceMessage = JsonConvert.DeserializeObject<FinchartWebsocketAssetPriceMessage>(rawMessageContent);

						if (finchartAssetPriceMessage.Bid != null)
							finchartAssetPrice.Bid = finchartAssetPriceMessage.Bid;

						if (finchartAssetPriceMessage.Ask != null)
							finchartAssetPrice.Ask = finchartAssetPriceMessage.Ask;

						if (finchartAssetPriceMessage.Last != null)
							finchartAssetPrice.Last = finchartAssetPriceMessage.Last;
					}
					else if (message.MessageType == WebSocketMessageType.Close)
					{
						await _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

						return new FinchartResponse() { StatusCode = 200, AssetPrice = finchartAssetPrice };
					}
				}
				catch (TaskCanceledException)
				{
					return new FinchartResponse() { StatusCode = 200, AssetPrice = finchartAssetPrice };
				}
			}
		}
	}
}
