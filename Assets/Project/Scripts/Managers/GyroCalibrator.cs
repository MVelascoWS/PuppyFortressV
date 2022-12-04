using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Doozy.Engine;

public class GyroCalibrator : MonoBehaviour
{
    private const float SMOTHNESS = 6f;
    private const float DRAG_RATE = .2f;
    float dragYawDegrees;
    Quaternion newRotation;
    Quaternion offsetRotation;
    float YawOffset;
    private float timeCount;
    public static GyroCalibrator instance;
    Vector3 forward;
    bool calibrated;
    private void Awake()
    {
#if UNITY_EDITOR
        this.enabled = false;
#endif
        instance = this;
        /*Input.compensateSensors = true;
        Input.gyro.enabled = true;*/
        //SceneManager.sceneLoaded += SceneLoaded;
        calibrated = false;
        YawOffset = 0f;
        Debug.Log("gyro euler " + Input.gyro.attitude.eulerAngles);
    }

    /*public void OnPostRender()
    {
        if (calibrated)
            return;
        offsetRotation = Input.gyro.attitude;
        Debug.Log("offset gyro: " + offsetRotation);
        YawOffset = GyroToUnity(offsetRotation).eulerAngles.y;
        Debug.Log("offset gyro angle: " + YawOffset);
        calibrated = true;
    }*/
    private void OnEnable()
    {
        Message.AddListener<GameEventMessage>(OnMessage);

        
    }

    private void OnDisable()
    {
        Message.RemoveListener<GameEventMessage>(OnMessage);

        
    }
   

    void CheckDrag()
    {
        if (Input.touchCount != 1)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Moved)
        {
            return;
        }

        dragYawDegrees += touch.deltaPosition.x * DRAG_RATE;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
            StartCoroutine("WaitForGyro");
    }

    IEnumerator WaitForGyro()
    {
        //yield return new WaitUntil(() => Quaternion.Angle(Input.gyro.attitude, Quaternion.identity) > 0f);
        yield return new WaitForSeconds(1f);
        /*offsetRotation = Input.gyro.attitude;
        Debug.Log("offset gyro: " + offsetRotation);
        YawOffset = GyroToUnity(offsetRotation).eulerAngles.y;
        YawOffset += 180f;*/        
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    public void Calibrate()
    {

        //StartCoroutine("WaitForReRotate");
        //YawOffset = YawOffsetf;
    }

    IEnumerator WaitForReRotate()
    {
        yield return new WaitForEndOfFrame();
        
        Debug.Log("offset gyro angle: " + YawOffset);
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;
        switch (message.EventName)
        {
            case "RenderGame":
                offsetRotation = Input.gyro.attitude;
                Debug.Log("offset gyro: " + offsetRotation);
                YawOffset = GyroToUnity(offsetRotation).eulerAngles.y;
                YawOffset += 180f;
                /*YawOffset = Input.gyro.attitude.eulerAngles.x;
                
                Debug.Log("offset gyro angle: " + YawOffset);*/
                break;
        }
    }
}
