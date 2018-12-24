using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultScene : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] AudioSource audioSource;
    [SerializeField] LeaderboardRequester leaderboardRequester;
    [SerializeField] LeaderboardView leaderboardView;

    // Use this for initialization
    void Start()
    {
        var scoreManager = ScoreManagerSingleton.Instance;
        // 所要時間の最下位桁の切り上げ切り捨てがハイスコア登録と一致するよう、一旦ハイスコア登録の形式に変換する
        var score = Score.FromStatistic(scoreManager.Score.StatisticValue);
        text.text = string.Format("あなたのチャレンジ結果\n{0}回中{1}回成功\n所要時間{2:0.00}秒"
                                 , scoreManager.GameCount, score.ClearCount, score.TotalTimeOnClear);
        audioSource.clip = Resources.Load<AudioClip>("Audio/result");
        audioSource.Play();

        leaderboardView.gameObject.SetActive(false);
        StartCoroutine("GetLeaderboard", 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GetLeaderboard(float waitSeconds)
    {
        if (waitSeconds > 0f)
        {
            yield return new WaitForSeconds(waitSeconds);
        }
        leaderboardRequester.Request(SetupLeaderboard);
    }

    void SetupLeaderboard(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        var statisticValue = ScoreManagerSingleton.Instance.Score.StatisticValue;
        var myEntry = leaderboardEntries.Find(_entry => _entry.PlayFabId == PlayFabLoginManagerSingleton.Instance.PlayFabId);
        if (myEntry != null && myEntry.StatValue < statisticValue
            || myEntry == null && leaderboardEntries.Count < LeaderboardRequester.MaxEntriesCount
            || myEntry == null && leaderboardEntries[leaderboardEntries.Count - 1].StatValue < statisticValue)
        {
            Debug.Log("Leaderboard does not updated yet.");
            StartCoroutine("GetLeaderboard", 0.5f);
            return;
        }

        leaderboardView.gameObject.SetActive(true);
        leaderboardView.Initialize(leaderboardEntries);
    }

    public void OnClickTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickTweet()
    {
        var scoreManager = ScoreManagerSingleton.Instance;
        // Start() と同様
        var score = Score.FromStatistic(scoreManager.Score.StatisticValue);
        var message = string.Format("10:10 あなたのチャレンジ結果は{0}回中{1}回成功、所要時間{2:0.00}秒でした"
                                    , scoreManager.GameCount, score.ClearCount, score.TotalTimeOnClear);
#if UNITY_WEBGL
        naichilab.UnityRoomTweet.Tweet("10to10", message, "unityroom", "unity1week");
#else
        message += "\nPC: https://unityroom.com/games/10to10  #unityroom #unity1week";
        Application.OpenURL("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL(message));
#endif
    }
}
