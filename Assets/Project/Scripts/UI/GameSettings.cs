using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WasdStudio.LeaderboardSystem;
using Doozy.Engine.UI;
using OhmsLibraries.Localization;

public class GameSettings : MonoBehaviour
{
    public UIToggle musicToggle;
    public UIToggle SFxToggle;
    public UIToggle spanishToggle;
    public UIToggle englishToggle;
    public UIToggle germanToggle;
   
    public PuppyGameConfig gameConfig;

    void Start()
    {
        Input.compensateSensors = true;
        Input.gyro.enabled = true;

        gameConfig.ManualStart();
        SFxToggle.IsOn = gameConfig.SFX.SavedData;
        musicToggle.IsOn = gameConfig.Music.SavedData;
        englishToggle.IsOn = false;
        spanishToggle.IsOn = false;
        germanToggle.IsOn = false;
        LanguageManager.Instance.ChangeLanguage(gameConfig.Language.SavedData);
        switch (gameConfig.Language.SavedData)
        {
            case SystemLanguage.English:
                englishToggle.IsOn = true;
                break;
            case SystemLanguage.Spanish:
                spanishToggle.IsOn = true;
                break;
            case SystemLanguage.German:
                germanToggle.IsOn = true;
                break;
        }

        musicToggle.OnValueChanged.AddListener(MusicChange);
        SFxToggle.OnValueChanged.AddListener(SFxChange);
        spanishToggle.OnValueChanged.AddListener(OnSpanishActivated);
        englishToggle.OnValueChanged.AddListener(OnEnglishActivated);
        germanToggle.OnValueChanged.AddListener(OnGermanActivated);       
        
    }

    public void MusicChange(bool state)
    {
        gameConfig.Music.SavedData = state;        
    }
    public void SFxChange(bool state)
    {
        gameConfig.SFX.SavedData = state;
    }

    public void OnSpanishActivated(bool state)
    {
        if (state)
        {
            gameConfig.Language.SavedData = SystemLanguage.Spanish;
            LanguageManager.Instance.ChangeLanguage(SystemLanguage.Spanish);
        }
    }

    public void OnEnglishActivated(bool state)
    {
        if (state)
        {
            gameConfig.Language.SavedData = SystemLanguage.English;
            LanguageManager.Instance.ChangeLanguage(SystemLanguage.English);
        }
    }

    public void OnGermanActivated(bool state)
    {
        if (state)
        {
            gameConfig.Language.SavedData = SystemLanguage.German;
            LanguageManager.Instance.ChangeLanguage(SystemLanguage.German);
        }
    } 

    
    public void SetRenderMode(bool isVR)
    {
        gameConfig.VRSelected.SavedData = isVR;
    }

    public void ResetLeaderboard()
    {
        LeaderboardManager.ResetRecords();
    }

}
