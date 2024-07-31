using GAME.PhotoRanking.Models.PhotoGroup;
using GAME.PhotoRanking.Models.RankingGame;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace GAME.PhotoRanking.DBContext
{
    public class MongoDBContext
    {
        private readonly IConfiguration _configuration;

        private readonly MongoClient _client;
        private readonly IMongoDatabase _mainDatabase;

        public IMongoCollection<PhotoGroupModel> PhotoGroups => _mainDatabase.GetCollection<PhotoGroupModel>("photoGroups");
        public IMongoCollection<RankingGameModel> RankingGames => _mainDatabase.GetCollection<RankingGameModel>("rankingGames");
        public IMongoCollection<GridFSFileInfo<ObjectId>> FilesInfo => _mainDatabase.GetCollection<GridFSFileInfo<ObjectId>>("fs.files");
        public IGridFSBucket Files => new GridFSBucket(_mainDatabase);

        public MongoDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            ConventionRegistry.Register("camelCase", new ConventionPack { new CamelCaseElementNameConvention() }, t => true);
            _client = new MongoClient(_configuration["mongoCS"]);
            _mainDatabase = _client.GetDatabase("photo-ranking");
        }
    }
}
