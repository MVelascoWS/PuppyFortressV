using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public float speed;
    public float pulseSpeed;
    public Color color1;
    public Color color2;

    private Image loading;
	// Use this for initialization
	void Start () {
        loading = GetComponent<Image>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        loading.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time * pulseSpeed, 1f));
	}
}
