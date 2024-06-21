using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database.Models.Base
{
	public interface IBaseEntity
	{
		[BsonId]
		string Id { get; set; }
	}
}
