using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
	
	public TMP_InputField playersInput; //This is for how many players the user has selected.
    public GameObject loadingScreen; //Gets the loading screen
    public Slider slider; //Gets the slider
    
    public void LoadLevel (string sceneName)
	{
		StartCoroutine(UpdateTable()); //Starts the LoadAsynchronously function
		StartCoroutine(LoadAsynchronously(sceneName)); //Starts the LoadAsynchronously function
	}

    IEnumerator UpdateTable()
    {
	    List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
	    //POST Data
	    formData.Add(new MultipartFormDataSection("id", UserManager.userID.ToString())); //For the username.

	    UnityWebRequest www = UnityWebRequest.Post(Domain.subDomain("includes/updateplays.inc.php"), formData); //This initiates the post request.

	    yield return www.SendWebRequest(); //This sends the post request.

	    if (www.isNetworkError || www.isHttpError) //This checks for an error with the server.
	    {
		    Debug.Log(www.error); //This prints the error.
	    }
	    else
	    {
		    Debug.Log(www.downloadHandler.text); //This sends the error code or if it worked on the server side.
	    }
    }
    
    IEnumerator LoadAsynchronously (string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); //Loads the monopoly board.

        loadingScreen.SetActive(true); //Shows the loading screen.

        while (!operation.isDone) //Runs the while loop whilst the level is loading.
		{
            float progress = Mathf.Clamp01(operation.progress / .9f); //Makes the unity loading value a nicer value as the original is 0-0.9

            slider.value = progress; //Sets the loading bar progress

            yield return null;
		}
    }
    
}
