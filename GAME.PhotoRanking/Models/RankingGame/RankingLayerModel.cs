namespace GAME.PhotoRanking.Models.RankingGame
{
    public class RankingLayerModel
    {
        public List<RankingResult> Results { get; set; } = new List<RankingResult>();
        public bool Finished => Results.All(r => r.Winner != null);
    }
}
