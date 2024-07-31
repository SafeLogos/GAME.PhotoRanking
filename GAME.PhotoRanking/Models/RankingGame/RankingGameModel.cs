using GAME.PhotoRanking.Models.Photo;
using GAME.PhotoRanking.Models.PhotoGroup;
using MongoDB.Bson.Serialization.Attributes;

namespace GAME.PhotoRanking.Models.RankingGame
{
    public class RankingGameModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public PhotoGroupModel Group { get; set; }

        public List<RankingLayerModel> Layers { get; set; }
        public PhotoModel? Winner { get; set; }
    }
}
