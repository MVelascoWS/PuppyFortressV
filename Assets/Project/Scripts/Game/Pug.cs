using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pug : MonoBehaviour {

    public GameObject reticle;
    public Camera currentCamera;
	void Update () {
        //transform.LookAt(new Vector3(reticle.transform.position.x, this.transform.position.y, reticle.transform.position.z));
        transform.forward = currentCamera.transform.forward;
	}
}
