using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using Doozy.Engine.SceneManagement;

public class Loader : MonoBehaviour
{
    public Doozy.Engine.SceneManagement.SceneLoader sceneLoader;
    public PuppyGameConfig gameConfig;
    public GameObject deviceOrientationCanvas;

    private void OnEnable()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;
        switch (message.EventName)
        {
            case "LoadWorld":
                
                if (sceneLoader != null)
                {
#if !UNITY_EDITOR
                    StartCoroutine("ValidateOrientation");
#else
                    deviceOrientationCanvas.SetActive(false);
                    sceneLoader.SceneBuildIndex = 1;
                    sceneLoader.LoadSceneAsync();
#endif
                }
                else
                {
                    Debug.Log("no loader ");
                }
                break;
        }
    }

    IEnumerator ValidateOrientation()
    {
        while (Input.deviceOrientation != DeviceOrientation.LandscapeLeft)
        {
            deviceOrientationCanvas.SetActive(true);
            yield return null;
        }
        deviceOrientationCanvas.SetActive(false);
        sceneLoader.SceneBuildIndex = 1;
        sceneLoader.LoadSceneAsync();
    }
}
