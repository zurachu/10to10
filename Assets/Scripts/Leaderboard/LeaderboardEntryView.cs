using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryView : MonoBehaviour
{
    [SerializeField] GameObject defaultBackground;
    [SerializeField] GameObject playerBackground;
    [SerializeField] Text rank;
    [SerializeField] Text playerName;
    [SerializeField] Text clearCount;
    [SerializeField] Text totalTimeOnClear;

    public void Initialize(PlayerLeaderboardEntry entry)
    {
        var isMyEntry = (PlayFabLoginManagerSingleton.Instance.PlayFabId == entry.PlayFabId);
        defaultBackground.SetActive(!isMyEntry);
        playerBackground.SetActive(isMyEntry);

        var score = Score.Try10Score.FromStatistic(entry.StatValue);
        rank.text = (entry.Position + 1).ToString();
        playerName.text = entry.DisplayName;
        clearCount.text = score.ClearCount.ToString();
        totalTimeOnClear.text = score.TotalTimeOnClear.ToString("F2");
    }
}
