using GAME.PhotoRanking.Extensions;
using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.Photo;
using GAME.PhotoRanking.Models.PhotoGroup;
using GAME.PhotoRanking.Models.RankingGame;
using GAME.PhotoRanking.Repositories.PhotoGroupsRepository;
using GAME.PhotoRanking.Repositories.RankingGamesRepository;
using MongoDB.Bson;
using System.Collections.Generic;

namespace GAME.PhotoRanking.Services.Domain.RankingGames
{
    public class RankingGamesDomainService : IRankingGamesDomainService
    {
        private readonly IPhotoGroupsRepository _photoGroupsRepository;
        private readonly IRankingGamesRepository _rankingGamesRepository;

        public RankingGamesDomainService(IPhotoGroupsRepository photoGroupsRepository, IRankingGamesRepository rankingGamesRepository)
        {
            _photoGroupsRepository = photoGroupsRepository;
            _rankingGamesRepository = rankingGamesRepository;
        }



        public Task<Response<RankingGameModel>> SelectPhoto(string gameId, string photoId) =>
            Response<RankingGameModel>.DoAsync(async resp =>
            {
                RankingGameModel game = (await _rankingGamesRepository.GetDetalization(gameId)).GetResult();

                RankingLayerModel lastLayer = game.Layers.Last();

                RankingResult? result = lastLayer.Results
                    .FirstOrDefault(r => r.ChallengerA.Id == photoId || r.ChallengerB.Id == photoId);

                resp.ThrowIfNull(result, "Экземпляр не найден");
                if (result!.Winner != null)
                    resp.Throw("Этот экземпляр уже был выбран");

                result.Winner = result.ChallengerA.Id == photoId ? result.ChallengerA : result.ChallengerB;

                if(lastLayer.Finished)
                {
                    List<PhotoModel> winners = lastLayer.Results.Select(r => r.Winner!).ToList();
                    if(winners.Count() == 1)
                    {
                        game.Winner = winners.First();
                        resp.Data = (await _rankingGamesRepository.UpdateGame(game)).GetResult();
                        return;
                    }

                    RankingLayerModel newLayer = _createLayer(winners);

                    game.Layers.Add(newLayer);
                }

                resp.Data = (await _rankingGamesRepository.UpdateGame(game)).GetResult();
            });

        public Task<Response<RankingGameModel>> StartGame(string groupId) =>
            Response<RankingGameModel>.DoAsync(async resp =>
            {
                PhotoGroupModel group = (await _photoGroupsRepository.Get(groupId)).GetResult();

                if (group.Photos.Count < 2)
                    resp.Throw("В группе должно быть более двух экземпляров");

                group.Photos = group.Photos.Shuffle();

                RankingGameModel game = new RankingGameModel()
                {
                    Date = DateTime.Now,
                    Group = group,
                    Layers = new List<RankingLayerModel>()
                };

                RankingLayerModel firstLayer = _createLayer(group.Photos);

                resp.Data = (await _rankingGamesRepository.CreateGame(game)).GetResult();
            });

        private RankingLayerModel _createLayer(List<PhotoModel> challengers)
        {
            RankingLayerModel layer = new RankingLayerModel()
            {
                Results = new List<RankingResult>()
            };

            for (int i = 0; i < challengers.Count; i += 2)
            {
                RankingResult result = new RankingResult();
                result.ChallengerA = challengers[i];
                if (challengers.Count > i)
                    result.ChallengerB = challengers[i + 1];
                else
                    result.ChallengerB = result.Winner = result.ChallengerA;

                layer.Results.Add(result);
            }

            return layer;
        }

    }
}
