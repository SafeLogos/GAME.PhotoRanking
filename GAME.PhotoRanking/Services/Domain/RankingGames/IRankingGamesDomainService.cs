using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.RankingGame;

namespace GAME.PhotoRanking.Services.Domain.RankingGames
{
    public interface IRankingGamesDomainService
    {
        Task<Response<RankingGameModel>> StartGame(string groupId);
        Task<Response<RankingGameModel>> SelectPhoto(string gameId, string photoId);

    }
}
