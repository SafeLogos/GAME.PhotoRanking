using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GAME.PhotoRanking.Models
{
    public interface IBaseMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
    }
}
