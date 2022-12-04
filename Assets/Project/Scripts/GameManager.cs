using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections.Generic;

public enum GameState
{
    HOW_TO_PLAY=0,
    COUNTDOWN,
    PLAY,
    GAME_OVER
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentState = GameState.HOW_TO_PLAY;
    
    public AudioClip buttonSound;

    public GameObject howToPlay;
    public Text counter;
    public GameObject gameCanvas;
    public GameObject keyboardCanvas;
    public GameObject gameOverCanvas;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        howToPlay.SetActive(true);
    }

    IEnumerator CountDown()
    {
        currentState = GameState.COUNTDOWN;

        counter.text = "3";
        counter.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        AudioManager.instance.PlaySound(buttonSound);
        counter.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        AudioManager.instance.PlaySound(buttonSound);
        counter.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        AudioManager.instance.PlaySound(buttonSound);
        counter.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);
        
        counter.transform.parent.gameObject.SetActive(false);
        gameCanvas.SetActive(true);

        currentState = GameState.PLAY;
    }

    public void HideHowToPlay()
    {
        AudioManager.instance.PlaySound(buttonSound);
        howToPlay.SetActive(false);
        StartCoroutine("CountDown");
    }

    public void GameOverNewRecord()
    {
        keyboardCanvas.SetActive(true);
        currentState = GameState.GAME_OVER;
    }

    public void GameOverNormal()
    {
        gameOverCanvas.SetActive(true);
        currentState = GameState.GAME_OVER;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        PlayerPrefs.SetString("SceneToLoad", "Menu");
        RuntimeCardboardLoader.instance.DisableVR();
        SceneManager.LoadScene("Loading");
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            HideHowToPlay();
    }
#endif
}
