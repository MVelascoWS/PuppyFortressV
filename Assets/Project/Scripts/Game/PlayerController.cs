using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using WasdStudio.GameFlowManager;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;

    public float shotDelay;
    public Transform bulletSpawnPoint;
    public float speed;

    public UnityEvent OnShoot;
    public bool Playing { get; set; }
    float angle;

    private float _currentShotDelay;


    private void Awake()
    {
        instance = this;
        _currentShotDelay = shotDelay;
    }

    private void Update()
    {
        MouseControl();

        if(_currentShotDelay < shotDelay ) {
            _currentShotDelay += Time.deltaTime;
            return;
        }
        if (Input.GetMouseButtonDown(0) && Playing)
        {
            OnShoot.Invoke();
            GameObject temp = ObjectPool.instance.TakePoolObject("Bullet", true);
            temp.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            _currentShotDelay = 0;
        }
    }

    public void MouseControl()
    {
        /*if (Input.GetMouseButtonDown(2))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }*/

        //if (!Cursor.visible)
        //{
            float s = 5f;
            float x = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * s;
            float y = Mathf.Clamp(-Input.GetAxis("Mouse Y"), -1, 1) * s;
            Vector3 angle;

            Camera.main.transform.eulerAngles += Vector3.up * x;
            Camera.main.transform.localEulerAngles += Vector3.right * y;

            angle = Camera.main.transform.localEulerAngles;
            angle.x = angle.x > 180 ? angle.x - 360 : angle.x;
            angle.x = Mathf.Clamp(angle.x, -60, 60);
            Camera.main.transform.localRotation = Quaternion.Euler(angle);
        //}
    }

    public void UnlockMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
