using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    public static Reticle instance;
    public Transform cam;
    public Texture2D point;
    public Texture2D circle;
    //Params for Interactable   
    public Color tintColor;
    public Color selectedColor;
    public float scale = 1.1f;
    [Range(0, 5f)]
    public float scaleTime = 0.1f;
    public Canvas loadingCanvas;
    public Image filledImage;
    public bool onSelection = false;

    private float fadeTimer;
    private float coolDownTime = 0.1f;
    private float coolDownTimer;
    private Vector3 originalScale;
    private Vector3 selectedScale;
    private bool selected = false;
    private Vector3 canvasOriginalScale;
    private Material mat;
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        canvasOriginalScale = transform.localScale;
        mat = transform.GetComponent<Renderer>().material;

        mat.SetColor("_Color", tintColor);
        originalScale = transform.localScale;
        selectedScale = originalScale * scale;
        mat.mainTexture = point;

        loadingCanvas.enabled = false;
        filledImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (Time.time - fadeTimer <= scaleTime)
            {
                transform.localScale = Vector3.Lerp(originalScale, selectedScale, (Time.time - fadeTimer) / scaleTime);
                mat.SetColor("_Color", Color.Lerp(tintColor, selectedColor, (Time.time - fadeTimer) / scaleTime));
            }

            if (Time.time - coolDownTimer >= coolDownTime)
            {
                transform.localScale = Vector3.Lerp(selectedScale, originalScale, (Time.time - coolDownTimer - coolDownTime) / (scaleTime * 2f));
                mat.SetColor("_Color", Color.Lerp(selectedColor, tintColor, (Time.time - coolDownTimer - coolDownTime) / (scaleTime * 2f)));

                if (Time.time - coolDownTimer >= coolDownTime + scaleTime)
                {
                    selected = false;
                    onSelection = false;
                    mat.mainTexture = point;
                    mat.SetColor("_Color", tintColor);
                }
            }
        }
    }

    public void SetTintColor(Color color)
    {
        tintColor = color;
    }

    public void Focus()
    {
        if (!selected)
        {
            onSelection = true;
            fadeTimer = Time.time;
        }
        coolDownTimer = Time.time;
        mat.mainTexture = circle;
        selected = true;
    }

    public void SetPosAndAngle(float dist)
    {
        dist -= 0.5f;
        transform.position = cam.position + (cam.forward * dist);
        transform.LookAt(cam.position);
        //Rotate?
        if (dist < 10.0f)
        {
            dist *= 1f + 5f * Mathf.Exp(-dist);
        }
        transform.localScale = canvasOriginalScale * dist;
    }
}
