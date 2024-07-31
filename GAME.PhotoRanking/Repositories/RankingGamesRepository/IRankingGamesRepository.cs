using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.RankingGame;

namespace GAME.PhotoRanking.Repositories.RankingGamesRepository
{
    public interface IRankingGamesRepository
    {
        Task<Response<RankingGameModel>> CreateGame(RankingGameModel game);
        Task<Response<RankingGameModel>> UpdateGame(RankingGameModel game);

        Task<Response<List<RankingGameModel>>> GetAll();
        Task<Response<RankingGameModel>> GetDetalization(string id);
    }
}
