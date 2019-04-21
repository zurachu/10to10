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
        ScoreManagerSingleton.Instance.Initialize();
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
}
