namespace Score
{
    public class Try10Score
    {
        public static readonly string StatisticName = "miss_count_and_time_negative";

        public static readonly int MaxGameCount = 10;

        static readonly int missCountCoefficient = 100000000;
        static readonly int totalTimeOnClearCofficient = 100;

        public int ClearCount { get; private set; }
        public float TotalTimeOnClear { get; private set; }

        public Try10Score(int clearCount, float totalTimeOnClear)
        {
            ClearCount = clearCount;
            TotalTimeOnClear = totalTimeOnClear;
        }

        /*
            Int32 ひとつで成功数と所要時間を合わせたランキング比較を行うための値
            GetLeaderboard() で昇順取得ができないので、少ないほど好成績となる値を正負反転する
        */
        public int StatisticValue
        {
            get
            {
                var missCount = MaxGameCount - ClearCount;
                return -(missCount * missCountCoefficient + (int)(TotalTimeOnClear * totalTimeOnClearCofficient));
            }
        }

        public static Try10Score FromStatistic(int statisticValue)
        {
            var missCount = -statisticValue / missCountCoefficient;
            var clearCount = MaxGameCount - missCount;
            var totalTimeOnClear = (float)(-statisticValue % missCountCoefficient) / totalTimeOnClearCofficient;
            return new Try10Score(clearCount, totalTimeOnClear);
        }
    }
}
