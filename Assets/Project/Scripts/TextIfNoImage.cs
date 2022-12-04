using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class TextIfNoImage : MonoBehaviour {

    public Image imageToCheck;
    Text textToUse;

    private void Awake()
    {
        imageToCheck = GetComponentInParent<Image>();
        textToUse = GetComponent<Text>();
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        imageToCheck = GetComponentInParent<Image>();
        if (imageToCheck.sprite == null)
        {
            textToUse.text = imageToCheck.name;            
        }
        else
        {
            if(Application.isPlaying)
                gameObject.SetActive(false);
            textToUse.text = null;
        }
    }
}
