using System;
using System.Collections.Generic;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class LeaderboardRequester : MonoBehaviour
{
    public void Request(Action<List<PlayerLeaderboardEntry>> onReceiveLeaderboard)
    {
        var connectingView = ConnectingView.Show();

        var request = new GetLeaderboardRequest {
            MaxResultsCount = 10,
            StatisticName = Score.StatisticName,
        };
        PlayFabClientAPI.GetLeaderboard(
            request,
            _result => {
                DebugLogLeaderboard(_result);

                connectingView.Close();
                if (onReceiveLeaderboard != null)
                {
                    onReceiveLeaderboard(_result.Leaderboard);
                }
            },
            _error => {
                var report = _error.GenerateErrorReport();
                Debug.LogError(report);

                connectingView.Close();
                ErrorDialogView.Show("GetLeaderboard failed", report, () => {
                    Request(onReceiveLeaderboard);
                }, true);
            });
    }

    void DebugLogLeaderboard(GetLeaderboardResult result)
    {
        var stringBuilder = new StringBuilder();
        foreach (var entry in result.Leaderboard)
        {
            stringBuilder.AppendFormat(string.Format("{0}:{1}:{2}:{3}\n", entry.Position, entry.StatValue, entry.PlayFabId, entry.DisplayName));
        }
        Debug.Log(stringBuilder);
    }
}
