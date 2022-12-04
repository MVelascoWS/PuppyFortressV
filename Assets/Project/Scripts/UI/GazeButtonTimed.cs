using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GazeButtonTimed : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image gazeImage;
    public UnityEvent GVRClick;
    public float gazeTime = 1.5f;
    public bool useNormalButton = false;   

    private bool gvrStatus;
    private float gvrTimer;
    private Button keybutton;

    void Start()
    {
        //gazeImage.fillAmount = gvrTimer = 0;
        if(useNormalButton)
            keybutton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gvrStatus)
        {
            gvrTimer += Time.unscaledDeltaTime;
            gazeImage.fillAmount = gvrTimer / gazeTime;
            if (gvrTimer >= gazeTime)
            {
                if(keybutton != null)
                    keybutton.onClick.Invoke();
                GVRClick.Invoke();
                gvrStatus = false;
                gvrTimer = 0;
                gazeImage.fillAmount = 0;
            }

        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gvrStatus = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gvrStatus = false;
        gvrTimer = 0;
        gazeImage.fillAmount = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gvrStatus = false;
        gvrTimer = 0;
        gazeImage.fillAmount = 0;
    }
}
