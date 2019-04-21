using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace InGame
{
    public class Try10Scene : InGameScene
    {
        [SerializeField] UpdatePlayerStatisticRequester statisticRequester;

        // Update is called once per frame
        void Update()
        {
            if (field != null)
            {
                TimeText = field.ElapsedSeconds.ToString("F2");
            }
        }

        protected override void Initialize()
        {
            var scoreManager = ScoreManagerSingleton.Instance;
            scoreManager.Initialize();
            RoundText = string.Format("Round\n{0}/{1}", scoreManager.GameCount, scoreManager.MaxGameCount);
            var seconds = 0f;
            TimeText = seconds.ToString("F2");
        }

        protected override void StartNewGame()
        {
            var scoreManager = ScoreManagerSingleton.Instance;
            var gameCount = scoreManager.GameCount;

            if (8 <= gameCount)
            {
                var counters = new List<SpriteRenderer>();
                switch (BorderDirection)
                {
                    case Field.BorderDirection.Vertical:
                        counters = field.Counters.OrderBy(_counter => _counter.transform.position.x).ToList();
                        break;

                    case Field.BorderDirection.Horizontal:
                        counters = field.Counters.OrderByDescending(_counter => _counter.transform.position.y).ToList();
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

            scoreManager.StartNewGame();
            RoundText = string.Format("Round\n{0}/{1}", scoreManager.GameCount, scoreManager.MaxGameCount);
            TimeText = field.ElapsedSeconds.ToString("F2");
        }

        protected override void OnGameResult(bool cleared, int firstHalfCount)
        {
            var scoreManager = ScoreManagerSingleton.Instance;
            if (cleared)
            {
                scoreManager.ClearGame(field.ElapsedSeconds);
            }

            if (scoreManager.GameCount < scoreManager.MaxGameCount)
            {
                Invoke("ReadyNewGame", 1f);
            }
            else
            {
                var score = scoreManager.Score;
                statisticRequester.Request(Score.StatisticName, score.StatisticValue, () => {
                    Invoke("LoadResultScene", 1f);
                });
            }
        }

        protected override Field.BorderDirection BorderDirection
        {
            get
            {
                switch (ScoreManagerSingleton.Instance.GameCount)
                {
                    case 0:
                    case 1:
                    case 2:
                        return Field.BorderDirection.Vertical;
                    case 3:
                    case 4:
                    case 5:
                        return Field.BorderDirection.Horizontal;
                    case 6:
                        return Field.BorderDirection.Vertical;
                    case 7:
                        return Field.BorderDirection.Horizontal;
                    case 8:
                        return Field.BorderDirection.Vertical;
                    case 9:
                        return Field.BorderDirection.Horizontal;
                }
                return Field.BorderDirection.Vertical;
            }
        }

        void LoadResultScene()
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
