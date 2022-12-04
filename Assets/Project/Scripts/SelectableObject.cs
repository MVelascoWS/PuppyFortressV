using UnityEngine;
using UnityEngine.Events;


public enum SelectableType
{
    TAP_INTERACTABLE,
    TIME_INTERACTABLE,
    HOLD_INTERACTABLE
}

public class SelectableObject : MonoBehaviour
{
    public SelectableType type;
    //Params for Interactable   
    public Color tintColor;    
    public float scale = 1.1f;    
    public float scaleTime = 0.1f;
    [HideInInspector()]
    public float timeToSelect = 1f;
    [HideInInspector()]
    public int tapsToAction = 1;
    [Header("On Select Event")]
    public UnityEvent onSelected;
    [Header("Material in Children")]
    public GameObject children; 

    private float fadeTimer;
    private Vector3 originalScale;
    private Vector3 selectedScale;
    private Color originalColor;
    private bool selected;
    private float coolDownTime = 0.1f;
    private float coolDownTimer;
    private Material mat;
    private float selectedTime;
    private int tapCount;
    private float holdTime = 0.2f;
    private float holdTimer;

	// Use this for initialization
	void Start () 
    {
        if (children) mat = children.GetComponent<Renderer>().material;
        else if (GetComponent<Renderer>()) mat = GetComponent<Renderer>().material;
        if (mat) originalColor = mat.color;
        originalScale = transform.localScale;
        selectedScale = originalScale * scale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (selected)
        {
            switch (type)
            {
                case SelectableType.TAP_INTERACTABLE:
                    if (Input.GetMouseButtonDown(0))
                    {
                        tapCount++;
                        if (tapCount>=tapsToAction)
                            LaunchEvent();
                    }

                    if (Time.time-fadeTimer<=scaleTime)
                    {
                        transform.localScale = Vector3.Lerp(originalScale, selectedScale, (Time.time - fadeTimer) / scaleTime);
                        if (mat) mat.color = Color.Lerp(originalColor, tintColor, (Time.time - fadeTimer) / scaleTime);
                    }

                    if (Time.time-coolDownTimer>=coolDownTime)
                    {
                        transform.localScale = Vector3.Lerp(selectedScale, originalScale, (Time.time - coolDownTimer - coolDownTime) / scaleTime);
                        if (mat) mat.color = Color.Lerp(tintColor, originalColor, (Time.time - coolDownTimer - coolDownTime) / scaleTime);

                        if (Time.time-coolDownTimer>=coolDownTime + scaleTime)
                        {
                            tapCount = 0;
                            selected = false;
                            Reticle.instance.loadingCanvas.enabled = false;
                        }
                    }
                    break;

                case SelectableType.TIME_INTERACTABLE:
                    if (selected)
                    {
                        Reticle.instance.filledImage.fillAmount = (Time.time - selectedTime) / timeToSelect;

                        if (Time.time-selectedTime>=timeToSelect)
                        {
                            LaunchEvent();
                        }
                    }

                    if (Time.time - fadeTimer <= scaleTime)
                    {
                        transform.localScale = Vector3.Lerp(originalScale, selectedScale, (Time.time - fadeTimer) / scaleTime);
                        if (mat) mat.color = Color.Lerp(originalColor, tintColor, (Time.time - fadeTimer) / scaleTime);
                    }

                    if (Time.time - coolDownTimer >= coolDownTime)
                    {
                        transform.localScale = Vector3.Lerp(selectedScale, originalScale, (Time.time - coolDownTimer - coolDownTime) / scaleTime);
                        if (mat) mat.color = Color.Lerp(tintColor, originalColor, (Time.time - coolDownTimer - coolDownTime) / scaleTime);

                        if (Time.time - coolDownTimer >= coolDownTime + scaleTime)
                        {
                            tapCount = 0;                            
                            selected = false;
                            Reticle.instance.loadingCanvas.enabled = false;
                        }
                    }
                    break;

                case SelectableType.HOLD_INTERACTABLE:
                    if (Input.GetMouseButtonDown(0))
                    {
                        holdTimer = Time.time;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        if (Time.time - holdTimer > holdTime)
                        {
                            LaunchEvent();
                            if (mat) mat.color = tintColor;
                        }
                        else
                        {
                            if (mat) mat.color = Color.Lerp(originalColor, tintColor, (-Time.time + holdTimer + holdTime));
                        }
                    }
                    else
                    {
                        if (mat) mat.color = Color.Lerp(originalColor, tintColor, (-Time.time + holdTimer + holdTime));
                    }
                    break;
            }
        }
        else
        {
            if(type == SelectableType.HOLD_INTERACTABLE)
                if (mat) mat.color = originalColor;
        }
    }

    public void LaunchEvent()
    {
        tapCount = 0;
        Reticle.instance.loadingCanvas.enabled = false;
        selected = false;
        holdTimer = Time.time;
        onSelected.Invoke();
              
    }

    public void Focus()
    {
        if (!selected)
        {
            fadeTimer = Time.time;
            selectedTime = Time.time;
            tapCount = 0;
            Reticle.instance.filledImage.fillAmount = 0;
        }
        coolDownTimer = Time.time;        
        selected = true;
    }
    
    
}
