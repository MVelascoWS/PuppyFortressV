using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;

public enum Country
{
    MEXICO = 1,
    BRASIL = 2,
    REPUBLICA_DOMINICANA =3,
    HONDURAS = 4,
    EL_SALVADOR = 5,
    GUATEMALA = 6,
    PANAMA = 7    
}

public enum CarrierMexico
{
    AT_and_T = 6,
    TELCEL = 50,
    NEXTEL = 17,
    IUSACELL = 17,
    MOVISTAR = 0
}

public enum CarrierBrasil
{
    NEXTEL = 17
}

public enum CarrierRepublicaDominicana
{
    CLARO = 27,
    ORANGE = 21,
    VIVA = 7
}

public enum CarrierHonduras
{
    TIGO = 26
}

public enum CarrierElSalvador
{
    TIGO = 18,
    CLARO = 37,
    DIGICEL = 33
}

public enum CarrierGuatemala
{
    TIGO = 13,
    CLARO = 36
}

public enum CarrierPanama
{
    CLARO = 0,
    DIGICEL = 30
}


public class LoginOld : MonoBehaviour
{
    public bool devMode = false;
    public GameObject loginForm;
    public GameObject loading;
    public GameObject failLogin;
    public Text failText;
    /*public InputField phone;
    public GameObject dropdownMexico;
    public GameObject dropdownBrasil;
    public GameObject dropdownRepublicaDominicana;
    public GameObject dropdownHonduras;
    public GameObject dropdownElSalvador;
    public GameObject dropdownGuatemala;
    public GameObject dropdownPanama;*/
    public InputField tokenInput;

    //public Button loginOkButton;   
    public string urlLoginDev = "http://192.241.98.181:8080/sac/rest/portal/login/token";
    public string urlLoginProd = "http://169.46.142.118:8080/sac/rest/portal/login/token";
    public string urlRedirectDev = "http://192.241.98.181:8080/sac/rest/admin/landingPage?productId=148";
    public string urlRedirectProd = "http://169.46.142.118:8080/sac/rest/admin/landingPage?productId=148";
    //public string urlSubscribe;
    //private string number;
    /*private Country country;
    private CarrierBrasil carrierBrasil;
    private CarrierElSalvador carrierElSalvador;
    private CarrierGuatemala carrierGuatemala;
    private CarrierHonduras carrierHonduras;
    private CarrierMexico carrierMexico;
    private CarrierPanama carrierPanama;
    private CarrierRepublicaDominicana carrierRepublicaDominicana;

    private int countryID;
    private int carrierID;*/
    private string token;

	// Use this for initialization
	void Start ()
    {
        //SelectCountry(0);
        //PlayerPrefs.DeleteAll();
	    /*if (PlayerPrefs.HasKey ("number") &&
            PlayerPrefs.HasKey ("countryID") &&
            PlayerPrefs.HasKey ("carrierID"))
        {
            loginForm.SetActive(false);
            number = PlayerPrefs.GetString("number");
            countryID = PlayerPrefs.GetInt("countryID");
            carrierID = PlayerPrefs.GetInt("carrierID");
            StartCoroutine("CheckLogin");
        }	*/


        if (PlayerPrefs.HasKey("token"))
        {
            loginForm.SetActive(false);
            token = PlayerPrefs.GetString("token");            
            StartCoroutine("CheckToken");
        }
        else
        {
            loginForm.SetActive(true);
            loading.SetActive(false);
            failLogin.SetActive(false);
        }
	}

    IEnumerator CheckToken()
    {
        loginForm.SetActive(false);
        loading.SetActive(true);
        failLogin.SetActive(false);        
        string postData = "{\"token\":\"" + token +"\"}";
        Dictionary<string, string> headers = new Dictionary<string, string>();
        Debug.Log(postData);
        headers["content-type"] = "application/json";
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(postData.ToCharArray());
        WWW www = new WWW(devMode?urlLoginDev:urlLoginProd, pData, headers);
        yield return www;
        loginForm.SetActive(false);
        loading.SetActive(false);
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!:\n" + www.text + "\n");
            var result = Json.Deserialize(www.text) as Dictionary<string, object>;
            var data = result["data"] as Dictionary<string, object>;
            var extraPoperty = data["extraProperty"] as Dictionary<string, object>;

            bool isSubscriber = (bool)data["isSubscriber"];
            string state = (string)extraPoperty["Estado"];


            Debug.Log("Is Subscriber: " + isSubscriber.ToString());
            Debug.Log("State: " + state);        

            if (isSubscriber && state == "ACTIVE")
            {
                gameObject.SetActive(false);
                PlayerPrefs.SetString("token", token);          
            }
            else
            {
                failText.text = "Usuario no suscrito o activo";
                //Application.OpenURL(urlRedirect);
                failLogin.SetActive(true);
                PlayerPrefs.DeleteKey("token");
                StartCoroutine("WaitMessage");
            }
        }
        else
        {
            Debug.Log("WWW Error:\n" + www.error + "\n");
            failText.text = "Error de conexion: " + www.error;
            failLogin.SetActive(true);
            StartCoroutine("WaitMessage");
        }
    }

    /*IEnumerator CheckSubscribe()
    {
        loginForm.SetActive(false);
        loading.SetActive(true);
        failLogin.SetActive(false);
        
        string postData = "{\"msisdn\":\"" + number.ToString() + "\",\"serviceId\":\"AMK01\",\"carrierId\":" + carrierID.ToString() + ",\"countryId\":" + countryID.ToString() + "}";
        
        Dictionary<string, string> headers = new Dictionary<string, string>();
        Debug.Log(postData);
        headers["content-type"] = "application/json";
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(postData.ToCharArray());
        WWW www = new WWW(urlSubscribe, pData, headers);
        yield return www;
        loginForm.SetActive(false);
        loading.SetActive(false);
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!:\n" + www.text + "\n");
            var result = Json.Deserialize(www.text) as Dictionary<string,object>;
            var data = result["data"] as Dictionary<string, object>;
            var extraPoperty = data["extraProperty"] as Dictionary<string, object>;

            string rMessage = (string)result["rMessage"];
            bool subscribed1 = (bool)data["subscribed"];
            bool subscribed2 = (bool)extraPoperty["subscribed"];
            string tokenTmp = (string)extraPoperty["token"];


            Debug.Log("rMessage: " + rMessage);
            Debug.Log("suscribed1: " + subscribed1.ToString());
            Debug.Log("suscribed2: " + subscribed2.ToString());
            Debug.Log("token: " + tokenTmp);

            if (subscribed1 && subscribed2 && rMessage == "SUCCESS" && !string.IsNullOrEmpty(tokenTmp))
            {
                gameObject.SetActive(false);
                PlayerPrefs.SetString("token",tokenTmp);
                PlayerPrefs.SetInt("countryID",countryID);
                PlayerPrefs.SetInt("carrierID",carrierID);
            }
            else
            {
                failText.text = "Fallo en la suscripción, intentelo más tarde";
                //Application.OpenURL(urlRedirect);
                failLogin.SetActive(true);
                StartCoroutine("WaitMessage");
            }
        }
        else
        {
            Debug.Log("WWW Error:\n" + www.error + "\n");
            failText.text = "Error de conexion: " + www.error;
            failLogin.SetActive(true);
            StartCoroutine("WaitMessage");            
        }
    }*/


    IEnumerator WaitMessage()
    {
        yield return new WaitForSeconds(3f);
        loginForm.SetActive(true);
        loading.SetActive(false);
        failLogin.SetActive(false);
    }
    
    public void SuscribeInBrowser()
    {

        StartCoroutine("GetURLForSubscribe");
        //Debug.Log("Redirect to " + urlRedirectDev);
        //Application.OpenURL(urlRedirectDev);
    }

    IEnumerator GetURLForSubscribe()
    {       
        WWW www = new WWW(devMode?urlRedirectDev:urlRedirectProd);
        yield return www;
        loginForm.SetActive(false);
        loading.SetActive(true);
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!:\n" + www.text + "\n");
            var result = Json.Deserialize(www.text) as Dictionary<string, object>;
            var data = result["data"] as Dictionary<string, object>;
          
            string urlFromJSON = (string)data["URL_LP_REDIRECT"];
            Debug.Log("URL: " + urlFromJSON);
            Application.OpenURL(urlFromJSON);
            loginForm.SetActive(true);
            loading.SetActive(false);
        }
        else
        {
            Debug.Log("WWW Error:\n" + www.error + "\n");
            failText.text = "Error de conexion: " + www.error;
            failLogin.SetActive(true);
            StartCoroutine("WaitMessage");
        }
    }

    public void SendLogin()
    {
        if (!string.IsNullOrEmpty(tokenInput.text))
        {
            token = tokenInput.text;
            StartCoroutine("CheckToken");
        }
    }


    /*public void CheckPhone()
    {
        loginOkButton.interactable = (phone.text.Length == 12);      
    }*/

    /*public void SelectCountry(int selection)
    {
        switch (selection)
        {
            case 0://MEXICO
                dropdownMexico.SetActive(true);
                dropdownBrasil.SetActive(false);
                dropdownRepublicaDominicana.SetActive(false);
                dropdownHonduras.SetActive(false);
                dropdownElSalvador.SetActive(false);
                dropdownGuatemala.SetActive(false);
                dropdownPanama.SetActive(false);
                country = Country.MEXICO;
                break;
            case 1://BRASIL
                dropdownMexico.SetActive(false);
                dropdownBrasil.SetActive(true);
                dropdownRepublicaDominicana.SetActive(false);
                dropdownHonduras.SetActive(false);
                dropdownElSalvador.SetActive(false);
                dropdownGuatemala.SetActive(false);
                dropdownPanama.SetActive(false);
                country = Country.BRASIL;
                break;
            case 2://REPUBLICA DOMINICANA
                dropdownMexico.SetActive(false);
                dropdownBrasil.SetActive(false);
                dropdownRepublicaDominicana.SetActive(true);
                dropdownHonduras.SetActive(false);
                dropdownElSalvador.SetActive(false);
                dropdownGuatemala.SetActive(false);
                dropdownPanama.SetActive(false);
                country = Country.REPUBLICA_DOMINICANA;
                break;
            case 3://HONDURAS
                dropdownMexico.SetActive(false);
                dropdownBrasil.SetActive(false);
                dropdownRepublicaDominicana.SetActive(false);
                dropdownHonduras.SetActive(true);
                dropdownElSalvador.SetActive(false);
                dropdownGuatemala.SetActive(false);
                dropdownPanama.SetActive(false);
                country = Country.HONDURAS;
                break;
            case 4://EL SALVADOR
                dropdownMexico.SetActive(false);
                dropdownBrasil.SetActive(false);
                dropdownRepublicaDominicana.SetActive(false);
                dropdownHonduras.SetActive(false);
                dropdownElSalvador.SetActive(true);
                dropdownGuatemala.SetActive(false);
                dropdownPanama.SetActive(false);
                country = Country.EL_SALVADOR;
                break;
            case 5://GUATEMALA
                dropdownMexico.SetActive(false);
                dropdownBrasil.SetActive(false);
                dropdownRepublicaDominicana.SetActive(false);
                dropdownHonduras.SetActive(false);
                dropdownElSalvador.SetActive(false);
                dropdownGuatemala.SetActive(true);
                dropdownPanama.SetActive(false);
                country = Country.GUATEMALA;
                break;
            case 6://PANAMA
                dropdownMexico.SetActive(false);
                dropdownBrasil.SetActive(false);
                dropdownRepublicaDominicana.SetActive(false);
                dropdownHonduras.SetActive(false);
                dropdownElSalvador.SetActive(false);
                dropdownGuatemala.SetActive(false);
                dropdownPanama.SetActive(true);
                country = Country.PANAMA;
                break;
        }
        countryID = country.GetHashCode();
    }*/

    /*public void SendLogin()
    {
        number = phone.text;

        switch (country)
        {
            case Country.BRASIL:
                carrierID = CarrierBrasil.NEXTEL.GetHashCode();
                break;
            case Country.EL_SALVADOR:
                switch (dropdownElSalvador.GetComponent<Dropdown>().value)
                {
                    case 0:
                        carrierID = CarrierElSalvador.TIGO.GetHashCode();
                        break;
                    case 1:
                        carrierID = CarrierElSalvador.CLARO.GetHashCode();
                        break;
                    case 2:
                        carrierID = CarrierElSalvador.DIGICEL.GetHashCode();
                        break;
                }
                break;
            case Country.GUATEMALA:
                switch (dropdownGuatemala.GetComponent<Dropdown>().value)
                {
                    case 0:
                        carrierID = CarrierGuatemala.TIGO.GetHashCode();
                        break;
                    case 1:
                        carrierID = CarrierGuatemala.CLARO.GetHashCode();
                        break;                  
                }
                break;
            case Country.HONDURAS:
                carrierID = CarrierHonduras.TIGO.GetHashCode();
                break;
            case Country.MEXICO:
                switch (dropdownMexico.GetComponent<Dropdown>().value)
                {
                    case 0:
                        carrierID = CarrierMexico.AT_and_T.GetHashCode();
                        break;
                    case 1:
                        carrierID = CarrierMexico.TELCEL.GetHashCode();
                        break;
                    case 2:
                        carrierID = CarrierMexico.NEXTEL.GetHashCode();
                        break;
                    case 3:
                        carrierID = CarrierMexico.IUSACELL.GetHashCode();
                        break;
                    case 4:
                        carrierID = CarrierMexico.MOVISTAR.GetHashCode();
                        break;                   
                }
                break;
            case Country.PANAMA:
                switch (dropdownPanama.GetComponent<Dropdown>().value)
                {
                    case 0:
                        carrierID = CarrierPanama.CLARO.GetHashCode();
                        break;
                    case 1:
                        carrierID = CarrierPanama.DIGICEL.GetHashCode();
                        break;
                }
                break;
            case Country.REPUBLICA_DOMINICANA:
                switch (dropdownRepublicaDominicana.GetComponent<Dropdown>().value)
                {
                    case 0:
                        carrierID = CarrierRepublicaDominicana.CLARO.GetHashCode();
                        break;
                    case 1:
                        carrierID = CarrierRepublicaDominicana.ORANGE.GetHashCode();
                        break;
                    case 2:
                        carrierID = CarrierRepublicaDominicana.VIVA.GetHashCode();
                        break;
                }
                break;
        }

        StartCoroutine("CheckSubscribe");
    }*/

    
}
