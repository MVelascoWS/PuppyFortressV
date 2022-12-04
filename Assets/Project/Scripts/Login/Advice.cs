using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Advice : MonoBehaviour
{
    public GameObject adviceMenu;

    void Start()
    {
        if (PlayerPrefs.HasKey("PPolicy"))
        {
            adviceMenu.SetActive(false);
            SceneManager.LoadScene(1);
        }
        else
            adviceMenu.SetActive(true);
    }

    public void AcceptPrivacyPolicy()
    {
        adviceMenu.SetActive(true);
        PlayerPrefs.SetInt("PPolicy", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void NoAcceptPrivacyPolicy()
    {
        adviceMenu.SetActive(false);
        StartCoroutine("WaitToReload");
    }

    IEnumerator WaitToReload()
    {
        yield return new WaitForSeconds(0.5f);
        adviceMenu.SetActive(true);
        SceneManager.LoadScene(0);
    }
}
