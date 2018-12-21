using System.Collections.Generic;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabPlayerEventManagerSingleton
{
    static PlayFabPlayerEventManagerSingleton instance;

    static readonly string RoundEndEventName = "custom_round_end";
    static readonly string RoundKey = "Round";
    static readonly string ClearedKey = "Cleared";
    static readonly string TimeKey = "Time";
    static readonly string ResultKey = "Result";

    public static PlayFabPlayerEventManagerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayFabPlayerEventManagerSingleton();
            }
            return instance;
        }
    }

    public void RoundEnd(int round, bool cleared, float time, string result)
    {
        var body = new Dictionary<string, object>();
        body.Add(RoundKey, round);
        body.Add(ClearedKey, cleared);
        body.Add(TimeKey, time);
        body.Add(ResultKey, result);

        var request = new WriteClientPlayerEventRequest {
            Body = body,
            EventName = RoundEndEventName
        };
        PlayFabClientAPI.WritePlayerEvent(request, OnWriteSuccess, OnWriteFailure);
    }

    void OnWriteSuccess(WriteEventResponse response)
    {
        var request = response.Request as WriteClientPlayerEventRequest;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat(string.Format("{0}:{1}\n", request.EventName, response.EventId));
        foreach (var item in request.Body)
        {
            stringBuilder.AppendFormat(string.Format("{0}:{1}\n", item.Key, item.Value));
        }
        Debug.Log(stringBuilder);
    }

    void OnWriteFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
