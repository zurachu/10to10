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
    public class Sec60Scene : InGameScene
    {
        [SerializeField] UpdatePlayerStatisticRequester statisticRequester;

        static readonly float initialRestTime = 60f;

        float restTimeOnStartGame;

        // Update is called once per frame
        void Update()
        {
            if (field != null && field.Playable)
            {
                var restTime = restTimeOnStartGame - field.ElapsedSeconds;
                if (restTime <= 0f)
                {
                    restTime = 0f;
                    field.ForceMiss();
                    TimeUp();
                }

                TimeText = restTime.ToString("F2");
            }
        }

        protected override void Initialize()
        {
            var gameCount = 1;
            RoundText = string.Format("Round {0}", gameCount);
            restTimeOnStartGame = initialRestTime;
            TimeText = restTimeOnStartGame.ToString("F2");
        }

        protected override void StartNewGame()
        {
            var scoreManager = ScoreManagerSingleton.Instance;
            var gameCount = scoreManager.GameCount;

            scoreManager.StartNewGame();
            RoundText = string.Format("Round {0}", gameCount);
        }

        protected override void OnGameResult(bool cleared, int firstHalfCount)
        {
            restTimeOnStartGame -= field.ElapsedSeconds;

            var scoreManager = ScoreManagerSingleton.Instance;
            if (cleared)
            {
                scoreManager.ClearGame(field.ElapsedSeconds);
            }
            else
            {
                restTimeOnStartGame -= 10f; // penalty
            }

            TimeText = restTimeOnStartGame.ToString("F2");

            if (restTimeOnStartGame > 0f)
            {
                Invoke("ReadyNewGame", 1f);
            }
            else
            {
                TimeUp();
            }
        }

        protected override Field.BorderDirection BorderDirection
        {
            get
            {
                return Field.BorderDirection.Vertical;
            }
        }

        void TimeUp()
        {
            var score = ScoreManagerSingleton.Instance.Sec60Score;
            statisticRequester.Request(Score.Sec60Score.StatisticName, score.StatisticValue, () => {
                Invoke("LoadResultScene", 1f);
            });
        }

        void LoadResultScene()
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
