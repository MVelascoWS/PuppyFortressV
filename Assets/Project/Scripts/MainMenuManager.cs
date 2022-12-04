using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource sfx;
    public AudioSource music;
    public Toggle soundToggle;
    public Toggle musicToggle;
    public Text[] namesHS;
    public Text[] pointsHS;

    private int highScoreCount = 5;
    // Use this for initialization
    void Start ()
    {
        LoadHighScores();
        UpdateSettings();
        if (PlayerPrefs.HasKey("Sound"))
            soundToggle.isOn = PlayerPrefs.GetInt("Sound") == 0 ? false : true;
        if (PlayerPrefs.HasKey("Music"))
            musicToggle.isOn = PlayerPrefs.GetInt("Music") == 0 ? false : true;
    }
	
    private void LoadHighScores()
    {
        for (int i=0;i<highScoreCount;i++)
        {
            if (PlayerPrefs.HasKey("Name" + i.ToString("00")))
                namesHS[i].text = (i + 1).ToString() + "." + PlayerPrefs.GetString("Name" + i.ToString("00"));
            else
                PlayerPrefs.SetString("Name" + i.ToString("00"), "__________");
            if (PlayerPrefs.HasKey("Points" + i.ToString("00")))
                pointsHS[i].text = PlayerPrefs.GetString("Points" + i.ToString("00"));
            else
                PlayerPrefs.SetString("Points" + i.ToString("00"),"0");
        }
    }

    private void UpdateSettings()
    {
        if (PlayerPrefs.HasKey("Sound"))
            sfx.mute = PlayerPrefs.GetInt("Sound") == 1 ? false : true;
        if (PlayerPrefs.HasKey("Music"))
            music.mute = PlayerPrefs.GetInt("Music") == 1 ? false : true;
    }

    public void SoundStatus(bool status)
    {
        PlayerPrefs.SetInt("Sound", status ? 1 : 0);
        Analytics.CustomEvent("SoundSettings", new Dictionary<string, object>
        {
            { "sfx", status }
        });
        UpdateSettings();
    }

    public void MusicStatus(bool status)
    {
        PlayerPrefs.SetInt("Music", status ? 1 : 0);
        Analytics.CustomEvent("MusicSettings", new Dictionary<string, object>
        {
            { "music", status }
        });
        UpdateSettings();
    }

    public void DeleteHighScores()
    {
        for (int i = 0; i < highScoreCount; i++)
        {
            PlayerPrefs.SetString("Name" + i.ToString("00"), "__________");
            PlayerPrefs.SetString("Points" + i.ToString("00"), "0");
        }
        LoadHighScores();
        
    }

    public void SetVR(bool vrMode)
    {
        PlayerPrefs.SetInt("VR Mode", vrMode ? 1 : 0);
    }

    public void LoadScene(string sceneIndex)
    {
        PlayerPrefs.SetString("SceneToLoad",sceneIndex);
        SceneManager.LoadScene("Loading");
    }
    
}
