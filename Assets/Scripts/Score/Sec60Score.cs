namespace Score
{
    public class Sec60Score
    {
        public static readonly string StatisticName = "sec60_clear_count";

        public int ClearCount { get; private set; }

        public Sec60Score(int clearCount)
        {
            ClearCount = clearCount;
        }

        public int StatisticValue => ClearCount;

        public static Sec60Score FromStatistic(int statisticValue)
        {
            return new Sec60Score(statisticValue);
        }
    }
}
