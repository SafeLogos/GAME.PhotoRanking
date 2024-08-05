using GAME.PhotoRanking.Models.Photo;

namespace GAME.PhotoRanking.Models.RankingGame
{
    public class RankingResult
    {
        public PhotoModel ChallengerA { get; set; }
        public PhotoModel ChallengerB { get; set; }
        public PhotoModel? Winner { get; set; }

        public int RoundNumber { get; set; }
    }
}
