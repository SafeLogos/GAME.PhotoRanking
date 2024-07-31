namespace GAME.PhotoRanking.Extensions
{
    public static class ArrayExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            Random rnd = new Random();
            return list.OrderBy(l => rnd.Next(0, 50_000)).ToList();
        }

        public static List<T> GetReversed<T>(this List<T> list)
        {
            List<T> reversed = new List<T>();
            for(int i = list.Count - 1; i >= 0; i--)
                reversed.Add(list[i]);

            return reversed;
        }
    }
}
