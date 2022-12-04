using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class PopUpManager : MonoBehaviour
{
    public string PopupName = "DeleteLeaderboard";

    private UIPopup tmpPopup;


    public void ShowPopup()
    {
        tmpPopup = UIPopupManager.GetPopup(PopupName);
        if (tmpPopup == null)
            return;

        UIPopupManager.ShowPopup(tmpPopup, tmpPopup.AddedToQueue,true);
    }

    public void ClearPopupQueue()
    {
        UIPopupManager.ClearQueue();
    }
}
