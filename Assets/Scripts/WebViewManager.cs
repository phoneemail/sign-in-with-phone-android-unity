using Gpm.WebView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Application;

public class WebViewManager : MonoBehaviour
{
    [System.Serializable]
    public class UserData
    {
        public string iss;
        public string aud;
        public long iat;
        public long exp;
        public string country_code;
        public string phone_no;
    }

    private const string PHONE_COUNTRY_CODE = "+XX";//Please add country code here
    private const string MERCHANT_PHONE_NUMBER = "XXXXXXXXXX";//Please phone number here

    private string deviceId = "";
    private string url = "";

    [SerializeField]
    private Button LoginWithPhoneButton;

    [SerializeField]
    private TMP_Text uiDecodedMobileNumber;

    // Start is called before the first frame update
    void Start()
    {
        deviceId = GetDeviceID();

        StringBuilder urlBuilder = new StringBuilder("https://www.phone.email/auth/sign-in?");
        urlBuilder.Append($"countrycode={PHONE_COUNTRY_CODE}");
        urlBuilder.Append($"&phone_no={MERCHANT_PHONE_NUMBER}");
        urlBuilder.Append("&auth_type=3");
        urlBuilder.Append($"&device={deviceId}");

        url = urlBuilder.ToString();

        Debug.Log("generated URL:" + url);

        if (LoginWithPhoneButton == null)
            LoginWithPhoneButton = GetComponent<Button>();

        LoginWithPhoneButton.onClick.AddListener(OnPressedLoginWithPhone);
    }

    // This function is called when Sign in with phone button is clicked 
    void OnPressedLoginWithPhone()
    {
        //using GpmWebView to load login page
        GpmWebView.ShowUrl(
        url,
        new GpmWebViewRequest.Configuration()
        {
            style = GpmWebViewStyle.POPUP,
            orientation = GpmOrientation.UNSPECIFIED,
            isClearCookie = false,
            isClearCache = false,
            isNavigationBarVisible = true,
            isCloseButtonVisible = true,
            supportMultipleWindows = true,

#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
        },
        OnCallback,
        new List<string>()
        {
             "phoneemail://"
        });

    }


    private void OnCallback(
    GpmWebViewCallback.CallbackType callbackType,
    string data,
    GpmWebViewError error)
    {
        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.Open:
                if (error != null)
                {
                    Debug.LogFormat("Fail to open WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.Close:
                if (error != null)
                {
                    Debug.LogFormat("Fail to close WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageStarted:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("PageStarted Url : {0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageLoad:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("Loaded Page:{0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowOpen:

                Debug.Log("MultiWindowOpen");

                break;
            case GpmWebViewCallback.CallbackType.MultiWindowClose:
                Debug.Log("MultiWindowClose");
                break;
            case GpmWebViewCallback.CallbackType.Scheme:
                Debug.Log($"Scheme: {data}");
                if (error == null)
                {
                    if (data.Contains("phoneemail"))
                    {
                        GpmWebView.Close();

                        // Extract parameters from the custom scheme URL
                        Dictionary<string, string> parameters = GetUrlParameters(data);

                        // Check if the parameters contain 'phtoken'
                        if (parameters.ContainsKey("phtoken"))
                        {
                            // This is your extracted JWT
                            string jwtValue = parameters["phtoken"];

                            // Decode JWT and parse JSON
                            string parsedJWT = DecodeJwtPayload(jwtValue);

                            if (parsedJWT != null)
                            {
                                // Parsing JSON 
                                UserData userData = JsonUtility.FromJson<UserData>(parsedJWT);

                                // Update UI
                                uiDecodedMobileNumber.text = userData.country_code + userData.phone_no;
                            }
                            else
                            {
                                Debug.LogError("Failed to decode JWT payload.");
                            }
                        }
                        else
                        {
                            Debug.LogError("Parameter 'phtoken' not found in the custom scheme URL.");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Failed to handle custom scheme. Error: {error}");
                }
                break;
            case GpmWebViewCallback.CallbackType.GoBack:
                Debug.Log("GoBack");
                break;
            case GpmWebViewCallback.CallbackType.GoForward:
                Debug.Log("GoForward");
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
                break;
                Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
                break;
#if UNITY_ANDROID
            case GpmWebViewCallback.CallbackType.BackButtonClose:
                Debug.Log("BackButtonClose");
                break;

#endif
        }
    }

    // Get Android DeviceID
    string GetDeviceID()
    {
        // Get Android ID
        AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");

        string android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");

        // Get bytes of Android ID
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(android_id);

        // Encrypt bytes with md5
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        string device_id = hashString.PadLeft(32, '0');

        return device_id;
    }

    // Helper function to extract parameters from a URL
    Dictionary<string, string> GetUrlParameters(string url)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        // Split the URL into base and parameters
        string[] parts = url.Split('?');
        if (parts.Length > 1)
        {
            // Split parameters into key-value pairs
            string[] keyValuePairs = parts[1].Split('&');
            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = WWW.UnEscapeURL(keyValue[0]);
                    string value = WWW.UnEscapeURL(keyValue[1]);
                    parameters[key] = value;
                }
            }
        }

        return parameters;
    }

    // Helper function to decode JWT
    string DecodeJwtPayload(string jwtString)
    {
        string[] parts = jwtString.Split('.');
        if (parts.Length > 2)
        {
            string base64UrlEncodedPayload = parts[1];

            // Ensure padding is correct
            int padLength = base64UrlEncodedPayload.Length % 4;
            if (padLength > 0)
            {
                base64UrlEncodedPayload += new string('=', 4 - padLength);
            }

            // Decode Base64Url to bytes
            byte[] payloadBytes = Convert.FromBase64String(base64UrlEncodedPayload);

            // Convert bytes to string
            return Encoding.UTF8.GetString(payloadBytes);
        }
        else
        {
            Debug.LogError("Invalid JWT format. Unable to decode payload.");
            return null;
        }
    }
}
