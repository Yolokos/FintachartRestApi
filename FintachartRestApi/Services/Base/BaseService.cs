using Database.Models.Base;
using Database.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FintachartRestApi.Services.Base
{
	public class BaseService<T> where T : class, IBaseEntity, new()
	{
		private readonly IMongoCollection<T> collection;

		public BaseService (MongoRepository mongoRepository)
		{
			string className = typeof(T).Name;
			collection = mongoRepository.mongoDatabase.GetCollection<T>(className);
		}

		public async Task<List<T>> GetAsync () =>
			await collection.Find(_ => true).ToListAsync();

		public async Task<T?> GetAsync (string id) =>
			await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task CreateAsync (T newItem) =>
			await collection.InsertOneAsync(newItem);

		public async Task CreateCollectionAsync (IEnumerable<T> newItems) =>
			await collection.InsertManyAsync(newItems);

		public long CountDocument ()
		{
			var filter = Builders<T>.Filter.Empty;
			CountOptions opts = new CountOptions() { Hint = "_id_" };

			return collection.CountDocuments(filter, opts);
		}

		public async Task UpdateAsync (string id, T updatedItem) =>
		await collection.ReplaceOneAsync(x => x.Id == id, updatedItem);

		public async Task RemoveAsync (string id) => await collection.DeleteOneAsync(x => x.Id == id);
	}
}
