using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class UpdatePlayerStatisticRequester : MonoBehaviour
{
    string statisticName;
    int value;
    Action onSuccess;

    public void Request(string statisticName, int value, Action onSuccess)
    {
        this.statisticName = statisticName;
        this.value = value;
        this.onSuccess = onSuccess;
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate {
                    StatisticName = statisticName,
                    Value = value,
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

        onSuccess?.Invoke();
    }

    void OnUpdateFailure(PlayFabError error)
    {
        var report = error.GenerateErrorReport();
        Debug.LogError(report);

        ErrorDialogView.Show("UpdatePlayerStatistics failed", report, () => {
            Request(statisticName, value, onSuccess);
        });
    }
}
