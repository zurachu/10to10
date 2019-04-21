public class ScoreManagerSingleton
{
    static ScoreManagerSingleton instance;

    public int GameCount { get; private set; }

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

    public Score.Try10Score Try10Score => new Score.Try10Score(clearCount, totalTimeOnClear);

    public void Initialize()
    {
        GameCount = 0;
        clearCount = 0;
        totalTimeOnClear = 0f;
    }

    public void StartNewGame()
    {
        GameCount++;
    }

    public void ClearGame(float time)
    {
        clearCount++;
        totalTimeOnClear += time;
    }
}
