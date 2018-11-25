using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI startText;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Blink");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SceneManager.LoadScene("InGameScene");
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            startText.enabled = !startText.enabled;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
