using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Image loadingBar;
    public Text loadingPercent;
    public float pulseTime = 0.25f;
    private string sceneToLoad = "Game";
    private AsyncOperation loading;
    
	// Use this for initialization
	void Start ()
    {
        if (PlayerPrefs.HasKey("SceneToLoad"))
            sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        loading = SceneManager.LoadSceneAsync(sceneToLoad);
        StartCoroutine("FadeOut");
	}
	
	// Update is called once per frame
	void Update ()
    {
        loadingBar.fillAmount = loading.progress;
        loadingPercent.text = loading.progress.ToString("00%");
	}

    IEnumerator FadeOut()
    {
        loadingPercent.CrossFadeAlpha(0, pulseTime,true);
        yield return new WaitForSeconds(pulseTime);
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        loadingPercent.CrossFadeAlpha(1, pulseTime, true);
        yield return new WaitForSeconds(pulseTime);
        StartCoroutine("FadeOut");
    }
}
