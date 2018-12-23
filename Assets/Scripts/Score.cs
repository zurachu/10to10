public class Score
{
    public static readonly string StatisticName = "miss_count_and_time_negative";

    public static readonly int MaxGameCount = 10;

    static readonly int missCountCoefficient = 100000000;
    static readonly int totalTimeOnClearCofficient = 100;

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

    public static Score FromStatistic(int statisticValue)
    {
        var score = new Score();
        var missCount = -statisticValue / missCountCoefficient;
        score.ClearCount = MaxGameCount - missCount;
        score.TotalTimeOnClear = (float)(-statisticValue % missCountCoefficient) / totalTimeOnClearCofficient;
        return score;
    }

    public int ClearCount;
    public float TotalTimeOnClear;
}
