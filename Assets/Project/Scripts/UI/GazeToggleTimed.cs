using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Doozy.Engine.UI;

public class GazeToggleTimed : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image gazeImage;
    public UnityEvent GVRClick;
    public float gazeTime = 1.5f;
    

    private bool gvrStatus;
    private float gvrTimer;
    private UIToggle keyToggle;

    void Start()
    {
        gazeImage.fillAmount = gvrTimer = 0;
        
            keyToggle = GetComponent<UIToggle>();
    }

    void Update()
    {
        if (gvrStatus)
        {
            gvrTimer += Time.unscaledDeltaTime;
            gazeImage.fillAmount = gvrTimer / gazeTime;
            if (gvrTimer >= gazeTime)
            {
                if (keyToggle != null)
                    if(!keyToggle.IsOn)
                        keyToggle.ToggleOn();
                    else
                        keyToggle.ToggleOff();
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
