using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultScene : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        var scoreManager = ScoreManagerSingleton.Instance;
        text.text = string.Format("あなたのチャレンジ結果は{0}回中{1}回成功\n所要時間{2:0.00}秒でした"
                                 , scoreManager.GameCount, scoreManager.ClearCount, scoreManager.TotalTimeOnClear);
        audioSource.clip = Resources.Load<AudioClip>("Audio/result");
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickTweet()
    {
        var scoreManager = ScoreManagerSingleton.Instance;
        var message = string.Format("10:10 あなたのチャレンジ結果は{0}回中{1}回成功、所要時間{2:0.00}秒でした"
                                    , scoreManager.GameCount, scoreManager.ClearCount, scoreManager.TotalTimeOnClear);
        naichilab.UnityRoomTweet.Tweet("10to10", message, "unityroom", "unity1week");
    }
}
