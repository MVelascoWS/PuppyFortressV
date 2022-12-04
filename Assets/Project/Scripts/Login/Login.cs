using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Text;


public class Login : MonoBehaviour
{
    public string url = "http://api.pacificmediainc.net/apps/subscribers.php";
    public bool validData = true;
    public GameObject loginUI;
    public GameObject loginLoading;
    public GameObject failLoading;
    public Text failText;
    public Text failButtonText;

    private int mcc;
    private int mnc;
    private string phone;
    private string bundleID;
    private string aid;
    private bool loginButtonFirst = false;

    private string urlLogin;
    private string msg2;
    private string labelButton2;
    private List<IMultipartFormSection> formData;
    // Use this for initialization
    void Start()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.Log("No Gyro");
            loginLoading.SetActive(false);

            failText.text = "Tu dispositivo no es compatible con este juego";
            failButtonText.text = "";
            failButtonText.transform.parent.gameObject.SetActive(false);
            failLoading.SetActive(true);
            return;
        }
        else
        {

            Debug.Log("Gyro supported");
        }
        formData = new List<IMultipartFormSection>();
        // PlayerPrefs.DeleteAll();
        GetPhoneInfo();
        
    }

    void AdvertisingIDCallback(string advertisingId, bool trackingEnabled, string error)
    {
        Debug.Log("advertisingId " + advertisingId + " " + trackingEnabled + " " + error);
        aid = advertisingId;
        AndroidJavaObject jo = AndroidUtil.Activity.Call<AndroidJavaObject>("getSystemService", "phone");
        ///
        //AndroidJavaObject jo2 = AndroidUtil.Activity.Call<AndroidJavaObject>("getSystemService", "TELEPHONY_SERVICE");
        ///
        string mccmnc = jo.Call<string>("getNetworkOperator");
        string simSN = jo.Call<string>("getSimSerialNumber");
        ///
        phone = jo.Call<string>("getLine1Number");
        ///
        if (mccmnc.Length>1)
        {
            mcc = int.Parse(mccmnc.Substring(0, 3));
            mnc = int.Parse(mccmnc.Substring(3));
        }
        else
        {
            mcc = 0;
            mnc = 0;
        }
        bundleID = Application.identifier;
    
        if (PlayerPrefs.HasKey("expiration"))
        {
            string expirationDate = PlayerPrefs.GetString("expiration");
            DateTime now = DateTime.Now;
            DateTime expiration = DateTime.Parse(expirationDate);
            TimeSpan difference = expiration - now;
            Debug.Log("Expiration: " + expiration + " Now: " + now + " Difference: " + difference.TotalSeconds);
            if (difference.TotalSeconds > 0)
            {
                //Play
                loginUI.SetActive(false);
            }
            else
            {
                PlayerPrefs.DeleteKey("expiration");
                StartCoroutine(LoginRequest());
            }
        }
        else
        {
            StartCoroutine(LoginRequest());
        }
    }
   
    // this is the callback that will be called after user has decided to grant or deny the permission(s)
    // will be called once per permission
    public void PermissionGrantedCallback(string permission, bool isGranted)
    {
        Debug.Log("Permission granted: " + permission + ": " + (isGranted ? "Yes" : "No"));

    }

    private void GetPhoneInfo()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Application.RequestAdvertisingIdentifierAsync(AdvertisingIDCallback))
                Debug.Log("advertising ID sucessful");
            else
            {
                Debug.Log("advertising ID failed");
                //SceneManager.LoadScene(3);
                GetPhoneInfo();
            }
        }
       /* else
        {
            if (validData)
            {
                phone = "+525531125771";
                aid = "7b044f92-0756-4ea5-828f-8445f49a549e";
                mcc = 334;
                mnc = 20;
            }
            else
            {
                phone = "+52";
                aid = "6b044f92-0756-4ea5-828f-8445f49a549e";
                mcc = 334;
                mnc = 20;
            }
            bundleID = Application.identifier;
        }*/
        
    }

    IEnumerator LoginRequest()
    {
        formData.Clear();
        if (phone == null )
            phone = "00";
        if (String.IsNullOrEmpty(phone))
        {
            //Debug.Log("Blank number");
            phone = "0";
        }
        Debug.Log("mcc: " + mcc + " mnc: " + mnc + " m: " + phone + " aid: " + aid + " name: " + bundleID);
        formData.Add(new MultipartFormDataSection("mcc", mcc.ToString()));
        formData.Add(new MultipartFormDataSection("mnc", mnc.ToString()));
       
        formData.Add(new MultipartFormDataSection("m", phone));
        formData.Add(new MultipartFormDataSection("aid", aid));
        formData.Add(new MultipartFormDataSection("name", bundleID));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            loginLoading.SetActive(false);

            failText.text = www.error;
            failButtonText.text = "Reintentar";
            loginButtonFirst = true;
            failLoading.SetActive(true);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log("Response code: " + www.responseCode);
            Debug.Log("Response Text: " + www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                CheckLogin(www.downloadHandler.text);
            }

        }
    }

    private void CheckLogin(string response)
    {
        var result = Json.Deserialize(response) as Dictionary<string, object>;
        int status = int.Parse(result["status"].ToString());

        if (status == 1)
        {
            //login
            string expirationDate = (string)result["expiration"];
            PlayerPrefs.SetString("expiration", expirationDate);
            loginUI.SetActive(false);
        }
        else
        {
            //No suscriber
            urlLogin = (string)result["url"];
            string msg1 = (string)result["message1"];
            string labelButton1 = (string)result["label-button1"];
            msg2 = (string)result["message2"];
            labelButton2 = (string)result["label-button2"];
            loginLoading.SetActive(false);

            Debug.Log("*" + urlLogin + aid + "*");

            failText.text = msg1.ToString();
            failButtonText.text = labelButton1.ToString();

            failLoading.SetActive(true);
        }
    }

    public void LoginButtonAction()
    {
        if (!loginButtonFirst)
        {
            Debug.Log("#" + urlLogin + aid + "#");
            Application.OpenURL(urlLogin + aid);
            failText.text = msg2.ToString();
            failButtonText.text = labelButton2.ToString();
        }
        else
        {
            StartCoroutine(LoginRequest());
        }
        loginButtonFirst = !loginButtonFirst;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    
    
}
