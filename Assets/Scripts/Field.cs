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

    public static readonly int NumHalfCounters = 10;
    public static readonly int NumCounters = NumHalfCounters * 2;
    public static readonly float CounterPositionRange = 4.5f;
    public static readonly float CountersDistance = 1.0f;
    public static readonly float HalfDistance = 0.5f;

    [SerializeField] SpriteRenderer counterPrefab;
    [SerializeField] Border verticalBorderPrefab;
    [SerializeField] Border horizontalBorderPrefab;
    [SerializeField] TextMeshProUGUI ratioText;
    [SerializeField] AudioSource audioSource;

    public List<SpriteRenderer> Counters { get; private set; }
    public float ElapsedSeconds { get; private set; }
    public bool Playable;

    AudioClip missAudio;
    AudioClip rightAudio;
    Border border;
    Action<bool, int> onResult;

    // Use this for initialization
    void Start()
    {
        missAudio = Resources.Load<AudioClip>("Audio/mistake");
        rightAudio = Resources.Load<AudioClip>("Audio/right2");
    }

    public void Initialize(BorderDirection direction, Action<bool, int> onResult)
    {
        this.onResult = onResult;

        var counterPositions = CreateCounterPositionList();
        Counters = new List<SpriteRenderer>();
        foreach (var position in counterPositions)
        {
            var counter = Instantiate(counterPrefab, transform);
            counter.transform.position = position;
            Counters.Add(counter);
        }

        ElapsedSeconds = 0f;
        Playable = true;

        SetBorderType(direction);

        audioSource.clip = Resources.Load<AudioClip>("Audio/countdown");
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Counters == null || Counters.Count < NumCounters || border == null)
        {
            return;
        }

        var mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.localPosition.z;
        var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Playable)
        {
            ElapsedSeconds += Time.deltaTime;

            border.Move(worldPosition);

            if (Input.GetMouseButtonUp(0))
            {
                Playable = false;
                Result();
            }
        }
    }

    void Result()
    {
        var firstHalfCount = border.CheckResult(Counters);
        var secondHalfCount = Counters.Count - firstHalfCount;
        ratioText.text = string.Format("<color=blue>{0}</color>:<color=red>{1}</color>", firstHalfCount, secondHalfCount);

        audioSource.Stop();
        var cleared = (firstHalfCount == NumHalfCounters);
        if (cleared)
        {
            audioSource.PlayOneShot(rightAudio);
        }
        else
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            audioSource.PlayOneShot(missAudio);
        }

        onResult?.Invoke(cleared, firstHalfCount);
    }

    List<Vector3> CreateCounterPositionList()
    {
        while (true)
        {
            var positions = new List<Vector3>();
            for (var i = 0; i < NumCounters; i++)
            {
                Vector3 position;
                do
                {
                    var x = UnityEngine.Random.Range(-CounterPositionRange, CounterPositionRange);
                    var y = UnityEngine.Random.Range(-CounterPositionRange, CounterPositionRange);
                    position = new Vector3(x, y);
                } while (positions.Any(_position => Vector3.Distance(position, _position) < CountersDistance));
                positions.Add(position);
            }
            var orderByX = positions.OrderBy(_position => _position.x).ToArray();
            var orderByY = positions.OrderBy(_position => _position.y).ToArray();
            if (Mathf.Abs(orderByX[NumHalfCounters - 1].x - orderByX[NumHalfCounters].x) > HalfDistance
                && Mathf.Abs(orderByY[NumHalfCounters - 1].y - orderByY[NumHalfCounters].y) > HalfDistance)
            {
                return positions;
            }
        }
    }

    void SetBorderType(BorderDirection direction)
    {
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
