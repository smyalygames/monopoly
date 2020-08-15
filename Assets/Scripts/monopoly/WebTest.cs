using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public static class Domain
{
    static string domain = "https://monopoly.smyalygames.com/"; //The domain used to access the website.
    public static string subDomain(string subDirectory)
    {
        return (domain + subDirectory); //Adds the subdomain to the website URL
    }

}

public class WebTest : MonoBehaviour {

    void Start() {
        StartCoroutine(GetText());
    }
 
    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get(Domain.subDomain("test.php"));
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
        }
    }
}