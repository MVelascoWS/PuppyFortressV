using UnityEngine;

public class RaycastVR : MonoBehaviour {

    public static RaycastVR instance;

    public float maxDistance = 100f;

    public RaycastHit raycastHit;
    public bool hit;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {       
        if (hit = Physics.Raycast(transform.position, transform.forward, out raycastHit, maxDistance))
        {
            Reticle.instance.SetPosAndAngle(Vector3.Distance(transform.position, raycastHit.point));
            SelectableObject obj = raycastHit.transform.GetComponent<SelectableObject>();
            if (obj)
            {
                obj.Focus();
                Reticle.instance.Focus();
                switch (obj.type)
                {
                    case SelectableType.TIME_INTERACTABLE:
                        Reticle.instance.loadingCanvas.enabled = true;
                        break;
                    case SelectableType.TAP_INTERACTABLE:
                        break;
                }
            }               
        }
        else
        {
            Reticle.instance.SetPosAndAngle(maxDistance);
        }
    }

}
