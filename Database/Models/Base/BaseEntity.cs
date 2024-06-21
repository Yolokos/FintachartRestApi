using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Database.Models.Base
{
	public class BaseEntity : IBaseEntity
	{
		[BsonId]
		public string Id { get; set; }
	}
}
