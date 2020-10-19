using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	public Button PlayButton; //Imports the play button
	private string existingProperties; //The variable for the properties
	private string existingCards;

	void Start() {
		Debug.Log("User ID: " + UserManager.userID);
		Debug.Log("Username: " + UserManager.username);
		if (!PropertiesHandler.CheckPropertyExists()) //Checks if the properties file doesn't exist.
		{
			StartCoroutine(GetProperties()); //Downloads the properties json.
		}
		else if (!PropertiesHandler.CheckCardExists())
		{
			StartCoroutine(GetCards()); //Downloads the cards json.
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
		}
		if (existingCards != null)
		{
			PropertiesHandler.SaveCards(existingCards); //Saves the downloaded data
		}

		if (PropertiesHandler.CheckPropertyExists() && PropertiesHandler.CheckCardExists())
		{
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
	
	IEnumerator GetCards()
	{
		UnityWebRequest www = UnityWebRequest.Get(Domain.subDomain("includes/get-cards.inc.php"));
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			// Show results as text
			string json = www.downloadHandler.text;
			existingCards = json;
			//existingProperties = JsonConvert.DeserializeObject<List<Property>>(json);
		}
	}

}
