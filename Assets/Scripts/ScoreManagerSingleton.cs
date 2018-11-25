using UnityEngine;

public class ScoreManagerSingleton
{
    static ScoreManagerSingleton instance;

    int gameCount;
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

    public int MaxGameCount
    {
        get
        {
            return 10;
        }
    }

    public int GameCount
    {
        get
        {
            return gameCount;
        }
    }

    public int ClearCount
    {
        get
        {
            return clearCount;
        }
    }

    public float TotalTimeOnClear
    {
        get
        {
            return totalTimeOnClear;
        }
    }

    public void Initialize()
    {
        gameCount = 0;
        clearCount = 0;
        totalTimeOnClear = 0f;
    }

    public void StartNewGame()
    {
        gameCount++;
    }

    public void ClearGame(float time)
    {
        clearCount++;
        totalTimeOnClear += time;
    }
}
