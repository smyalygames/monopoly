﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

class LoginInformation
{
    public string user;
    public bool success;
    public string errors;
}

public class Login : MonoBehaviour
{
    
    public TMP_InputField username; //This is for the username text input.
    public TMP_InputField password; //This is for the password text input.
    public Button login; //This is the button used to log in.
    
    private bool CheckIsEmpty() //This checks if the strings are empty.
    {
        var strings = new List<string> {username.text, password.text}; //This puts all of the text boxes into an array.
        string check = "​"; //This is used for the check, the ZWSP is TMP's way of identifying nothing as null.
        if (strings.Contains(check)) //If any of the inputs are null.
        {
            return true;
        }
        
        return false;
    }

    void InteractableForm(bool decision)
    {
        login.interactable = decision;
        username.interactable = decision;
        password.interactable = decision;
        enabled = decision;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        login.onClick.AddListener(delegate
        {
            StartCoroutine(LoginForm());
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIsEmpty())
        {
            login.interactable = false;
        }
        else
        {
            login.interactable = true;
        }
    }
    
    IEnumerator LoginForm()
    {
        InteractableForm(false); //This disables the form.
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //POST Data
        formData.Add(new MultipartFormDataSection("username", username.text)); //For the username.
        formData.Add(new MultipartFormDataSection("password", password.text)); //For the password.

        UnityWebRequest www = UnityWebRequest.Post(Domain.subDomain("includes/login.inc.php"), formData); //This initiates the post request.
        
        yield return www.SendWebRequest(); //This sends the post request.

        if (www.isNetworkError || www.isHttpError) //This checks for an error with the server.
        {
            Debug.Log(www.error); //This prints the error.
        }
        else
        {
            string status = www.downloadHandler.text; //This downloads the data from the server.
            LoginInformation information = JsonConvert.DeserializeObject<LoginInformation>(status); //This deserializes the JSON string to a class.
            

            if (information.success) //If the user has successfully logged in.
            {
                UserManager.username = information.user; //This sets the logged in username.
                UnityEngine.SceneManagement.SceneManager.LoadScene(1); //Loads the menu scene.
            }

            InteractableForm(true); // This enables the form again.
        }

    }
}