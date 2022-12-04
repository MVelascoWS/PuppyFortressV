using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PartaGames.Android;

public class TestPermissions : MonoBehaviour
{

    public Text text;

    private static readonly string WRITE_EXTERNAL_STORAGE = "android.permission.READ_PHONE_STATE";


    public void CheckStorage()
    {
        text.text += "\nCheck permission: READ_PHONE_STATE" + ": " + (PermissionGranterUnity.IsPermissionGranted(WRITE_EXTERNAL_STORAGE) ? "Yes" : "No");
    }


    public void GrantStorage()
    {
        PermissionGranterUnity.GrantPermission(WRITE_EXTERNAL_STORAGE, PermissionGrantedCallback);
    }

    // this is the callback that will be called after user has decided to grant or deny the permission(s)
    // will be called once per permission
    public void PermissionGrantedCallback(string permission, bool isGranted)
    {
        //text.text += "\nPermission granted: " + permission + ": " + (isGranted ? "Yes" : "No");
        if (isGranted)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
        else
        {
            GrantStorage();
        }
    }

    private void Start()
    {        
        GrantStorage();      
    }
}
