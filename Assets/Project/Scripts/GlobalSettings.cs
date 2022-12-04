using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSettings : MonoBehaviour
{
    public GameObject hardwareAdvice;
    public GameObject loginObject;
	
	void Start ()
    {
#if LOGIN
        loginObject.SetActive(true);

#endif
#if NO_LOGIN
        loginObject.SetActive(false);
        if (!SystemInfo.supportsGyroscope)
        {
            hardwareAdvice.SetActive(true);
        }
        else
            hardwareAdvice.SetActive(false);
#endif
    }

}
