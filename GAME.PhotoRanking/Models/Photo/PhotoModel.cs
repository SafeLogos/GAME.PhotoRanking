using MongoDB.Bson.Serialization.Attributes;

namespace GAME.PhotoRanking.Models.Photo
{
    [BsonIgnoreExtraElements]
    public class PhotoModel
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ImageId { get; set; }
    }
}
