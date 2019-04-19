using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

public class InGameScene : MonoBehaviour
{
    [SerializeField] Field fieldPrefab;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI readyText;

    protected string TimeText
    {
        get => timeText.text;
        set
        {
            timeText.text = value;
        }
    }

    protected string RoundText
    {
        get => roundText.text;
        set
        {
            roundText.text = value;
        }
    }

    protected Field field;

    // Use this for initialization
    void Start()
    {
        Initialize();
        Invoke("ReadyFirstGame", 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReadyFirstGame()
    {
        readyText.gameObject.SetActive(false);
        ReadyNewGame();
    }

    protected virtual void Initialize()
    {
    }

    protected virtual void StartNewGame()
    {
    }

    protected virtual void OnGameResult(bool cleared, int firstHalfCount)
    {
    }

    protected virtual Field.BorderDirection BorderDirection => Field.BorderDirection.Vertical;

    void ReadyNewGame()
    {
        if (field != null)
        {
            Destroy(field.gameObject);
        }
        field = Instantiate(fieldPrefab);

        field.Initialize(BorderDirection, OnGameResult);

        StartNewGame();
    }

    protected void UpdatePlayerStatistics(Score score)
    {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate {
                    StatisticName = Score.StatisticName,
                    Value = score.StatisticValue
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateSuccess, OnUpdateFailure);
    }

    void OnUpdateSuccess(UpdatePlayerStatisticsResult result)
    {
        var request = result.Request as UpdatePlayerStatisticsRequest;
        var statisticUpdate = request.Statistics[0];
        Debug.Log(string.Format("{0}:{1}:{2}", statisticUpdate.StatisticName, statisticUpdate.Version, statisticUpdate.Value));

        Invoke("LoadResultScene", 1f);
    }

    void OnUpdateFailure(PlayFabError error)
    {
        var report = error.GenerateErrorReport();
        Debug.LogError(report);

        ErrorDialogView.Show("UpdatePlayerStatistics failed", report, () => {
            UpdatePlayerStatistics(ScoreManagerSingleton.Instance.Score);
        });
    }

    protected virtual void LoadResultScene()
    {
    }
}
