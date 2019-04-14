public class ScoreManagerSingleton
{
    static ScoreManagerSingleton instance;

    int gameCount;
    Score score;
    int clearCount;
    float totalTimeOnClear;

    public static ScoreManagerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScoreManagerSingleton();
            }
            return instance;
        }
    }

    public int MaxGameCount => Score.MaxGameCount;

    public int GameCount => gameCount;

    public Score Score
    {
        get
        {
            if (score == null)
            {
                score = new Score();
            }
            return score;
        }
    }

    public void Initialize()
    {
        gameCount = 0;
        score = new Score();
    }

    public void StartNewGame()
    {
        gameCount++;
    }

    public void ClearGame(float time)
    {
        score.ClearCount++;
        score.TotalTimeOnClear += time;
    }
}
