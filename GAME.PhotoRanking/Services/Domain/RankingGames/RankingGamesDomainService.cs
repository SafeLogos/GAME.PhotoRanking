using GAME.PhotoRanking.Extensions;
using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.Photo;
using GAME.PhotoRanking.Models.PhotoGroup;
using GAME.PhotoRanking.Models.RankingGame;
using GAME.PhotoRanking.Repositories.PhotoGroupsRepository;
using GAME.PhotoRanking.Repositories.RankingGamesRepository;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

                if (lastLayer.Finished)
                {
                    List<PhotoModel> winners = lastLayer.Results.Select(r => r.Winner!).ToList();
                    if (winners.Count() == 1)
                    {
                        game.Winner = winners.First();
                        resp.Data = (await _rankingGamesRepository.UpdateGame(game)).GetResult();
                        return;
                    }

                    RankingLayerModel newLayer = _createLayer(game, winners);

                    game.Layers.Add(newLayer);
                }

                resp.Data = (await _rankingGamesRepository.UpdateGame(game)).GetResult();
            });

        public Task<Response<string>> StartGame(string groupId) =>
            Response<string>.DoAsync(async resp =>
            {
                PhotoGroupModel group = (await _photoGroupsRepository.Get(groupId)).GetResult();

                if (group.Photos.Count < 2)
                    resp.Throw("В группе должно быть более двух экземпляров");

                group.Photos = group.Photos.Shuffle();

                RankingGameModel game = new RankingGameModel()
                {
                    Date = DateTime.Now,
                    Group = group,
                    Layers = new List<RankingLayerModel>(),
                    RoundsCount = GetRoundsCount(group)
                };

                RankingLayerModel firstLayer = _createLayer(game, group.Photos);

                game.Layers.Add(firstLayer);

                RankingGameModel createdGame = (await _rankingGamesRepository.CreateGame(game)).GetResult();
                resp.Data = createdGame.Id;
            });

        private int GetRoundsCount(PhotoGroupModel group)
        {
            RankingGameModel game = new RankingGameModel()
            {
                Group = group,
                Layers = new List<RankingLayerModel>()
            };

            List<PhotoModel> challengers = group.Photos.Select(p => p).ToList();
            do
            {
                RankingLayerModel layer = _createLayer(game, challengers);
                game.Layers.Add(layer);

                challengers = layer.Results.Select(r => r.ChallengerA).ToList();
                layer.Results.ForEach(r => r.Winner = r.ChallengerA);
            } while (challengers.Count > 2);

            return game.Layers.Last().Results.Max(r => r.RoundNumber) + 1;
        }

        private RankingLayerModel _createLayer(RankingGameModel game, List<PhotoModel> challengers)
        {
            RankingLayerModel layer = new RankingLayerModel()
            {
                Results = new List<RankingResult>()
            };

            challengers = challengers.Shuffle();

            int round = 0;

            if (game.Layers.Count > 0)
                round = Math.Max(round, game.Layers.Last().Results.Max(r => r.RoundNumber));

            for (int i = 0; i < challengers.Count; i += 2)
            {
                RankingResult result = new RankingResult();
                result.ChallengerA = challengers[i];
                if (challengers.Count > i + 1)
                {
                    result.ChallengerB = challengers[i + 1];
                    result.RoundNumber = ++round;
                }
                else
                {
                    result.ChallengerB = result.Winner = result.ChallengerA;
                    result.RoundNumber = 0;
                }

                layer.Results.Add(result);
            }

            return layer;
        }

    }
}
