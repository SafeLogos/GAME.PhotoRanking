using GAME.PhotoRanking.Repositories.RankingGamesRepository;
using GAME.PhotoRanking.Services.Domain.RankingGames;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAME.PhotoRanking.Controllers
{
    [Route("api/ranking-games")]
    [ApiController]
    public class RankingGamesController : ControllerBase
    {
        private readonly IRankingGamesDomainService _rankingGamesDomainService;
        private readonly IRankingGamesRepository _rankingGamesRepository;

        public RankingGamesController(IRankingGamesDomainService rankingGamesDomainService, 
            IRankingGamesRepository rankingGamesRepository)
        {
            _rankingGamesDomainService = rankingGamesDomainService;
            _rankingGamesRepository = rankingGamesRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _rankingGamesRepository.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) =>
            Ok(await _rankingGamesRepository.GetDetalization(id));

        [HttpPost("start-game/{groupId}")]
        public async Task<IActionResult> Create(string groupId) =>
            Ok(await _rankingGamesDomainService.StartGame(groupId));

        [HttpPost("select-photo/{gameId}/{photoId}")]
        public async Task<IActionResult> SelectPhoto(string gameId, string photoId) =>
            Ok(await _rankingGamesDomainService.SelectPhoto(gameId, photoId));
    }
}
