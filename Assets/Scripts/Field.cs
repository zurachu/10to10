using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using TMPro;

public class Field : MonoBehaviour
{
    public enum BorderDirection
    {
        Vertical,
        Horizontal,
    }

    [SerializeField] SpriteRenderer counterPrefab;
    [SerializeField] Border verticalBorderPrefab;
    [SerializeField] Border horizontalBorderPrefab;
    [SerializeField] TextMeshProUGUI ratioText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] AudioSource audioSource;

    public Action<List<SpriteRenderer>> OnCounterCreated;
    public Action OnGameEnd;

    AudioClip missAudio;
    AudioClip rightAudio;
    List<SpriteRenderer> counters;
    Border border;
    bool clicked;
    float seconds;

    // Use this for initialization
    void Start()
    {
        var counterPositions = CreateCounterPositionList();
        counters = new List<SpriteRenderer>();
        foreach (var position in counterPositions)
        {
            var counter = Instantiate(counterPrefab, transform);
            counter.transform.position = position;
            counters.Add(counter);
        }
        if (OnCounterCreated != null)
        {
            OnCounterCreated(counters);
        }

        seconds = 0;

        var scoreManager = ScoreManagerSingleton.Instance;
        scoreManager.StartNewGame();
        roundText.text = string.Format("Round\n{0}/{1}", scoreManager.GameCount, scoreManager.MaxGameCount);

        audioSource.clip = Resources.Load<AudioClip>("Audio/countdown");
        audioSource.Play();

        missAudio = Resources.Load<AudioClip>("Audio/mistake");
        rightAudio = Resources.Load<AudioClip>("Audio/right2");
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f;
        var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (!clicked)
        {
            seconds += Time.deltaTime;
            timeText.text = seconds.ToString("F2");

            border.Move(worldPosition);

            if (Input.GetMouseButtonUp(0))
            {
                Result(worldPosition);
                clicked = true;
            }
        }
    }

    void Result(Vector3 mousePosition)
    {
        border.CheckResult(counters, (_firstHalfCount) => {
            var scoreManager = ScoreManagerSingleton.Instance;
            var secondHalfCount = counters.Count - _firstHalfCount;
            ratioText.text = string.Format("<color=blue>{0}</color>:<color=red>{1}</color>", _firstHalfCount, secondHalfCount);
            audioSource.Stop();
            var cleared = (_firstHalfCount * 2 == counters.Count);
            if (cleared)
            {
                scoreManager.ClearGame(seconds);
                audioSource.PlayOneShot(rightAudio);
            }
            else
            {
                GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                audioSource.PlayOneShot(missAudio);
            }

            var result = string.Format("{0}:{1}", _firstHalfCount, secondHalfCount);
            PlayFabPlayerEventManagerSingleton.Instance.RoundEnd(scoreManager.GameCount, cleared, seconds, result);

            if (OnGameEnd != null)
            {
                OnGameEnd();
            }
        });
    }

    List<Vector3> CreateCounterPositionList()
    {
        while (true)
        {
            var positions = new List<Vector3>();
            for (var i = 0; i < 10 * 2; i++)
            {
                Vector3 position;
                do
                {
                    position = new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), UnityEngine.Random.Range(-4.5f, 4.5f));
                } while (positions.Any(_position => Vector3.Distance(position, _position) < 1.0f));
                positions.Add(position);
            }
            var orderByX = positions.OrderBy(_position => _position.x).ToArray();
            var orderByY = positions.OrderBy(_position => _position.y).ToArray();
            if (Mathf.Abs(orderByX[10 - 1].x - orderByX[10].x) > 0.5f
                && Mathf.Abs(orderByY[10 - 1].y - orderByY[10].y) > 0.5f)
            {
                return positions;
            }
        }
    }

    public void SetBorderType(BorderDirection direction)
    {
        if (border != null)
        {
            Destroy(border.gameObject);
        }

        switch (direction)
        {
            case BorderDirection.Vertical:
                border = Instantiate(verticalBorderPrefab, transform);
                break;

            case BorderDirection.Horizontal:
                border = Instantiate(horizontalBorderPrefab, transform);
                break;
        }
    }
}
