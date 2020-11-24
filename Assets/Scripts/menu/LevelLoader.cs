using System;
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
	public Button play; //This is for the play button.
    public GameObject loadingScreen; //Gets the loading screen
    public Slider slider; //Gets the slider

    void Start()
    {
	    play.onClick.AddListener(LoadLevel);
    }

    public void LoadLevel()
	{
		StartCoroutine(UpdateTable()); //Starts the LoadAsynchronously function
		GameSettings.players = Convert.ToInt32(playersInput.text); //This sets the amount of players that has been set to play in the game.
		Debug.Log(GameSettings.players);
		StartCoroutine(LoadAsynchronously("monopoly")); //Starts the LoadAsynchronously function
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
