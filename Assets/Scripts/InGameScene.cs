using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        field.OnCounterCreated = (_counters) =>
        {
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

        field.OnGameEnd = () =>
        {
            StartCoroutine("GameEnd");
        };
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(1f);
        if (ScoreManagerSingleton.Instance.GameCount < ScoreManagerSingleton.Instance.MaxGameCount)
        {
            StartNewGame();
        }
        else
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
