using Database.Models;
using Database.Repositories;
using FintachartRestApi.Services.Base;

namespace FintachartRestApi.Services
{
	public class AssetService : BaseService<Asset>
	{
		public AssetService (MongoRepository repository) : base(repository)
		{
		}
	}
}
