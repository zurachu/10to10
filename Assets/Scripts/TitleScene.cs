using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] GameObject clickToStart;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] LeaderboardRequester leaderoardRequester;
    [SerializeField] LeaderboardView leaderboardViewPrefab;

    // Use this for initialization
    void Start()
    {
        clickToStart.SetActive(false);
        PlayFabLoginManagerSingleton.Instance.TryLogin(OnLoginSuccess, OnLoginFailure);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnLoginSuccess()
    {
        clickToStart.SetActive(true);
        StartCoroutine("Blink");
    }

    void OnLoginFailure(string report)
    {
        ErrorDialogView.Show("Login failed", report, () => {
            SceneManager.LoadScene(this.GetType().Name);
        });
    }

    IEnumerator Blink()
    {
        while (true)
        {
            startText.enabled = !startText.enabled;
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void OnClick()
    {
        SceneManager.LoadScene("InGameScene");
    }

    public void OnClickLeaderboard()
    {
        leaderoardRequester.Request(_leaderboard => {
            var view = Instantiate(leaderboardViewPrefab);
            view.Initialize(_leaderboard);
        });
    }
}
