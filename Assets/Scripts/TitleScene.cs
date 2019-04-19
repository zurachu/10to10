using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] GameObject clickToStart;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] LeaderboardRequester leaderoardRequester;
    [SerializeField] LeaderboardView leaderboardViewPrefab;
    [SerializeField] GameObject privacyPolicyButton;

    // Use this for initialization
    void Start()
    {
        clickToStart.SetActive(false);

#if UNITY_ANDROID || UNITY_IPHONE
        privacyPolicyButton.SetActive(true);
#else
        privacyPolicyButton.SetActive(false);
#endif

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
        SceneManager.LoadScene("Try10Scene");
    }

    public void OnClickLeaderboard()
    {
        leaderoardRequester.Request(_leaderboard => {
            var view = Instantiate(leaderboardViewPrefab);
            view.Initialize(_leaderboard);
        });
    }

    public void OnClickPrivacyPolicy()
    {
        Application.OpenURL("https://zurachu.github.io/10to10/privacy-policy.html");
    }
}
