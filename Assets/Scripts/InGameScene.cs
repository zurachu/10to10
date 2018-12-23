using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviour
{
    [SerializeField] Field fieldPrefab;

    Field field;

    // Use this for initialization
    void Start()
    {
        ScoreManagerSingleton.Instance.Initialize();
        StartNewGame();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartNewGame()
    {
        if (field != null)
        {
            Destroy(field.gameObject);
        }
        field = Instantiate(fieldPrefab);

        var gameCount = ScoreManagerSingleton.Instance.GameCount;
        var borderType = Field.BorderDirection.Vertical;
        if (gameCount < 3)
        {
            borderType = Field.BorderDirection.Vertical;
        }
        else if (gameCount < 6)
        {
            borderType = Field.BorderDirection.Horizontal;
        }
        else
        {
            borderType = ((gameCount % 2 == 0) ? Field.BorderDirection.Vertical : Field.BorderDirection.Horizontal);
        }
        field.SetBorderType(borderType);

        field.OnCounterCreated = (_counters) => {
            if (8 <= gameCount)
            {
                var counters = new List<SpriteRenderer>();
                switch (borderType)
                {
                    case Field.BorderDirection.Vertical:
                        counters = _counters.OrderBy(_counter => _counter.transform.position.x).ToList();
                        break;

                    case Field.BorderDirection.Horizontal:
                        counters = _counters.OrderByDescending(_counter => _counter.transform.position.y).ToList();
                        break;
                }
                var candidates = new int[] { 7, 8, 9, 11, 12, 13 };
                var count = candidates[Random.Range(0, candidates.Length)];
                var index = 0;
                foreach (var counter in counters)
                {
                    if (index < count)
                    {
                        counter.color = new Color(0.75f, 0.75f, 1.0f);
                    }
                    else
                    {
                        counter.color = new Color(1.0f, 0.75f, 0.75f);
                    }
                    index++;
                }
            }
        };

        field.OnGameEnd = () => {
            var scoreManager = ScoreManagerSingleton.Instance;
            if (scoreManager.GameCount < scoreManager.MaxGameCount)
            {
                StartCoroutine("WaitAndNextGame");
            }
            else
            {
                UpdatePlayerStatistics(scoreManager.Score);
            }
        };
    }

    IEnumerator WaitAndNextGame()
    {
        yield return new WaitForSeconds(1f);
        StartNewGame();
    }

    void UpdatePlayerStatistics(Score score)
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

        StartCoroutine("WaitAndResultScene");
    }

    IEnumerator WaitAndResultScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("ResultScene");
    }

    void OnUpdateFailure(PlayFabError error)
    {
        var report = error.GenerateErrorReport();
        Debug.LogError(report);

        ErrorDialogView.Show("UpdatePlayerStatistics failed", report, () => {
            UpdatePlayerStatistics(ScoreManagerSingleton.Instance.Score);
        });
    }
}
