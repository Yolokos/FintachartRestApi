using FintachartRestApi.Services;
using FintachartRestApi.ThirdPartyHelpers.Finchart;

namespace FintachartRestApi
{
	public static class ServiceExtension
	{
		public static async Task SeedMongoDatabase (this IServiceProvider services)
		{
			var finchartHelper = services.GetService<FinchartApiService>();
			var assetService = services.GetService<AssetService>();

			if (assetService.CountDocument() == 0)
			{
				var finchartResponse = await finchartHelper.LoadAssets();
				await assetService.CreateCollectionAsync(finchartResponse.Assets);
			}
		}
	}
}
