using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;
using WasdStudio.LeaderboardSystem;

public class EntryValidator : MonoBehaviour
{
    public UIButton entryButton;
    public TextMeshProUGUI nameText;
    public PuppyGameConfig gameConfig;
    public int Points { set; get; }

    void OnEnable()
    {
        //entryButton.Interactable = false;
        entryButton.gameObject.SetActive(false);
        nameText.text = "";        
    }


    public void Register()
    {
        gameConfig.PlayerName = nameText.text;
        LeaderboardManager.AddRecord(nameText.text, Points);
        Points = 0;
    }

    public void OnChangeName(string nameToSet)
    {
        if (string.IsNullOrWhiteSpace(nameToSet))
        {
            Debug.Log("empty name");
            entryButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("full name");
            entryButton.gameObject.SetActive(true);
        }
    }
}
