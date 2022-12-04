// Run in split-screen mode

using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class RuntimeCardboardLoader : MonoBehaviour
{
    public static RuntimeCardboardLoader instance;
    public GameObject enableVRCanvas;
    public bool vrMode = false;
    private bool oldMode = false;

    public GameObject howToPlay;
    public Sprite CardboardTex, touchTex;
    Material howToPlayMat;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(LoadDevice("cardboard"));
        howToPlayMat = howToPlay.GetComponent<Renderer>().material;
    }

    IEnumerator LoadDevice(string newDevice)
    {
        yield return null;
        if (!XRSettings.loadedDeviceName.Equals(newDevice))
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return new WaitForEndOfFrame();
            do
            {
                yield return null;
            } while (!XRSettings.loadedDeviceName.Equals(newDevice));

            XRSettings.enabled = PlayerPrefs.GetInt("VR Mode")==1?true:false;
            vrMode = XRSettings.enabled;
        }
        InputTracking.Recenter();
    }

    public void SetVR(bool vrStatus)
    {
        vrMode = vrStatus;
    }

    public void DisableVR()
    {
        XRSettings.enabled = false;
        vrMode = false;
        oldMode = vrMode;
        Camera.main.ResetAspect();
        enableVRCanvas.SetActive(true);
    }

    public void EnableVR()
    {
        XRSettings.enabled = true;
        vrMode = true;
        oldMode = vrMode;
        enableVRCanvas.SetActive(false);
        //Camera.main.ResetAspect();        
    }

    void FixedUpdate()
    {
        if (!vrMode)
        {
            Camera.main.GetComponent<Transform>().localRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);
            //howToPlayMat.SetTexture("_MainTex", touchTex.texture);
        }
        else
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                vrMode = false;
                //howToPlayMat.SetTexture("_MainTex", CardboardTex.texture);
            }
        }

        if (oldMode != vrMode)
        {
            XRSettings.enabled = vrMode;
            oldMode = vrMode;
            Camera.main.ResetAspect();
            enableVRCanvas.SetActive(!vrMode);
        }
    }
}