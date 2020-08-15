using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    public GameObject loadingScreen; //Gets the loading screen
    public Slider slider; //Gets the slider
    
    public void LoadLevel (string sceneName)
	{
        StartCoroutine(LoadAsynchronously(sceneName)); //Starts the LoadAsynchronously function
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
