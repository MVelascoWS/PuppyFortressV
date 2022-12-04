using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using Doozy.Engine.UI;
using WasdStudio.GameFlowManager;
using OhmsLibraries.Localization;
using WasdStudio.GameConfig;
using WasdStudio.LeaderboardSystem;

public class GameFlowHelper : MonoBehaviour
{
    public GameFlowManager gameFlowManager;
    public PuppyGameConfig gameConfig;
    [Header("UI Pause Settings")]
    public UIToggle germanToggle;
    public UIToggle englishToggle;
    public UIToggle spanishToggle;
    public UIToggle musicToggle;
    public UIToggle sfxToggle;
    public bool Playing { set; get; }
    public static GameFlowHelper instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //if (gameConfig.VRSelected.SavedData)
        //{
        //    //renderManager.currentMode = RenderingMode.VR;
        //    //renderManager.LoadRenderMode(RenderingMode.VR);
        //}

        if ( musicToggle ) { musicToggle.IsOn = gameConfig.Music.SavedData; musicToggle.OnValueChanged.AddListener( MusicChange ); }
        if ( sfxToggle ) { sfxToggle.IsOn = gameConfig.SFX.SavedData; sfxToggle.OnValueChanged.AddListener( SFxChange ); }
        if ( englishToggle ) { englishToggle.IsOn = false; englishToggle.OnValueChanged.AddListener( OnEnglishActivated ); }
        if (spanishToggle) { spanishToggle.IsOn = false; spanishToggle.OnValueChanged.AddListener(OnSpanishActivated); }
        if ( germanToggle ) { germanToggle.IsOn = false; germanToggle.OnValueChanged.AddListener( OnGermanActivated ); }
        //LanguageManager.Instance.ChangeLanguage(gameConfig.Language.SavedData);
        switch (gameConfig.Language.SavedData)
        {
            case SystemLanguage.English:
                if ( englishToggle ) englishToggle.IsOn = true;
                break;
            case SystemLanguage.Spanish:
                if (englishToggle) spanishToggle.IsOn = true;
                break;
            case SystemLanguage.German:
                if ( germanToggle ) germanToggle.IsOn = true;
                break;
        }
        gameConfig.PlayerName = "";


        //spanishToggle.OnValueChanged.AddListener(OnSpanishActivated);


    }

    private void OnEnable()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }
    private void OnDisable()
    {
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    public void MusicChange(bool state)
    {
        gameConfig.Music.SavedData = state;
    }
    public void SFxChange(bool state)
    {
        gameConfig.SFX.SavedData = state;
    }
    public void OnEnglishActivated(bool state)
    {
        if (state)
        {
            gameConfig.Language.SavedData = SystemLanguage.English;
            LanguageManager.Instance.ChangeLanguage(SystemLanguage.English);
        }
    }

    public void OnSpanishActivated(bool state)
    {
        if (state)
        {
            gameConfig.Language.SavedData = SystemLanguage.Spanish;
            LanguageManager.Instance.ChangeLanguage(SystemLanguage.Spanish);
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

    public void OnGameOver()
    {
        Debug.Log("OnGameOver");
    }

    public void OnLeaderboard()
    {
        Debug.Log("OnLeaderboard");
    }

    public void OnExitPlaying()
    {
        //pointsText.SetData(distanceTracker.Distance);
        //entryValidator.Points = Mathf.FloorToInt(distanceTracker.Distance);
        GameEventMessage.SendEvent("GameOver");
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;
        switch (message.EventName)
        {
            case "StartGame":
            case "CountDown":
            case "RestartGame":
            case "ChangePhase":
                Debug.Log("ChangePhase");
                gameFlowManager.ChangePhase();
                break;
            case "PauseGame":
                Time.timeScale = 0f;
                break;
            case "UnpauseGame":
            case "ExitGame":
                Time.timeScale = 1f;
                break;
            case "ValidateName":
                if (string.IsNullOrWhiteSpace(gameConfig.PlayerName))
                    GameEventMessage.SendEvent("ShowEntry");
                else
                {
                    
                    LeaderboardManager.AddRecord(gameConfig.PlayerName, PlayerManager.instance.Score);
                    GameEventMessage.SendEvent("RegisterDone");
                }
                break;
            case "LevelActivated":
                Debug.Log("Scene activated");
                break;
        }
    }
}
