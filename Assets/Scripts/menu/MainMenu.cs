using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	public Button PlayButton; //Imports the play button
	private string existingProperties; //The variable for the properties
	
	void Start() {
		if (!PropertiesHandler.checkExists()) //Checks if the properties file doesn't exist.
		{
			StartCoroutine(GetProperties()); //Downloads the properties json.
		}
		else
		{
			enabled = false; //Stops the update function.
			PlayButton.interactable = true; //Enables the play button
		}
	}

	void Update()
	{
		if (existingProperties != null) //Checks if the data has been downloaded
		{
			PropertiesHandler.SaveProperties(existingProperties); //Saves the downloaded data
			PlayButton.interactable = true; //Enables the play button
			enabled = false; //Stops the update loop
		}
	}

	public void QuitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}
	
	IEnumerator GetProperties()
	{
		UnityWebRequest www = UnityWebRequest.Get(Domain.subDomain("includes/get-properties.inc.php"));
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			// Show results as text
			string json = www.downloadHandler.text;
			existingProperties = json;
			//existingProperties = JsonConvert.DeserializeObject<List<Property>>(json);
		}
	}

}
