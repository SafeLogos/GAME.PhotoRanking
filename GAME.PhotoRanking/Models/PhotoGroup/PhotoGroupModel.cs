using GAME.PhotoRanking.Models.Photo;
using MongoDB.Bson.Serialization.Attributes;

namespace GAME.PhotoRanking.Models.PhotoGroup
{
    public class PhotoGroupModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public bool IsDeleted { get; set; }

        public List<PhotoModel> Photos { get; set; }
    }
}
