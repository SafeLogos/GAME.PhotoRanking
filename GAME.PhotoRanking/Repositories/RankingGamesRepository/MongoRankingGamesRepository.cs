using AutoMapper;
using GAME.PhotoRanking.DBContext;
using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.RankingGame;
using MongoDB.Driver;

namespace GAME.PhotoRanking.Repositories.RankingGamesRepository
{
    public class MongoRankingGamesRepository : BaseMongoRepository, IRankingGamesRepository
    {
        public MongoRankingGamesRepository(MongoDBContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public Task<Response<RankingGameModel>> CreateGame(RankingGameModel game) =>
            Response<RankingGameModel>.DoAsync(async resp =>
            {
                game.Date = DateTime.Now;
                _db.RankingGames.InsertOne(game);
                resp.Data = game;
            });

        public Task<Response<RankingGameModel>> UpdateGame(RankingGameModel game) =>
            Response<RankingGameModel>.DoAsync(async resp =>
            {
                var filter = Builders<RankingGameModel>.Filter.Eq(g => g.Id, game.Id);
                _db.RankingGames.ReplaceOne(filter, game);
                resp.Data = game;
            });

        public Task<Response<List<RankingGameModel>>> GetAll() =>
            Response<List<RankingGameModel>>.DoAsync(async resp =>
            {
                _db.RankingGames.Find("{}").Project(rg => new RankingGameModel()
                {
                    Id = rg.Id,
                    Date = rg.Date,
                    Winner = rg.Winner,
                    Group = null,
                    Layers = new List<RankingLayerModel>()
                });
            });

        public Task<Response<RankingGameModel>> GetDetalization(string id) =>
            Response<RankingGameModel>.DoAsync(async resp =>
            {
                var filter = Builders<RankingGameModel>.Filter.Eq(rg => rg.Id, id);
                List<RankingGameModel> list = _db.RankingGames.Find(filter).ToList();
                resp.ThrowIfEmptyArray(list, "Игра не найдена");
                resp.Data = list.First();
            });
    }
}
