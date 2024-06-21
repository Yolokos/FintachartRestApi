using Database.Common;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database.Repositories
{
	public class MongoRepository
	{
		public IMongoDatabase mongoDatabase;

		public MongoRepository (
			IOptions<DatabaseSettings> dabaseSettings)
		{
			var mongoClient = new MongoClient(dabaseSettings.Value.ConnectionString);

			mongoDatabase = mongoClient.GetDatabase(dabaseSettings.Value.DatabaseName);
		}
	}
}
