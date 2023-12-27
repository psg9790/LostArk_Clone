using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebRequestTest : MonoBehaviour
{
    [SerializeField] RawImage img;
    void Start()
    {
        StartCoroutine(WeatherCo());
    }

    IEnumerator WeatherCo()
    {
        string url = "https://openweathermap.org/img/wn/50d@2x.png";
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
        req.certificateHandler = new AcceptCeritificates();
        yield return req.SendWebRequest();
        if (req.isDone)
        {
            Debug.Log("done");
        }
        if (req.isNetworkError || req.isHttpError || req.isNetworkError)
        {
            Debug.Log(req.error);
        }
        img.texture = DownloadHandlerTexture.GetContent(req);
    }
}

public class AcceptCeritificates : CertificateHandler
{
    //Public Key
    private static string PUB_KEY = "";

    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        Debug.Log(pk);
        return true;
    }
}
