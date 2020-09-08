using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{

    public TMP_InputField username; //This is for the username text input.
    public TMP_InputField email; //This is for the email text input.
    public TMP_InputField password; //This is for the password text input.
    public TMP_InputField repeatPassword; //This is for the password that has been repeated in the text input.
    public Button register; //This is the button used to register.

    private bool CheckIsEmpty() //This checks if the strings are empty.
    {
        var strings = new List<string> {username.text, email.text, password.text, repeatPassword.text}; //This puts all of the text boxes into an array.
        string check = "​"; //This is used for the check, the ZWSP is TMP's way of identifying nothing as null.
        if (strings.Contains(check)) //If any of the inputs are null.
        {
            return true;
        }
        
        return false;
    }

    void InteractableForm(bool decision)
    {
        register.interactable = decision;
        username.interactable = decision;
        email.interactable = decision;
        password.interactable = decision;
        repeatPassword.interactable = decision;
        enabled = decision;
    }

    // Start is called before the first frame update
    void Start()
    {
        register.onClick.AddListener(delegate
        {
            StartCoroutine(RegisterForm());
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIsEmpty()) //This checks if all of the inputs are empty.
        {
            register.interactable = false; //If they are empty, disable the register button.
        }
        else
        {
            register.interactable = true; //If all of them are filled, then allow the user to register.
        }
    }

    IEnumerator RegisterForm()
    {
        InteractableForm(false);
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //POST Data
        formData.Add(new MultipartFormDataSection("username", username.text)); //For the username.
        formData.Add(new MultipartFormDataSection("email", email.text)); //For the email.
        formData.Add(new MultipartFormDataSection("password", password.text)); //For the password.
        formData.Add(new MultipartFormDataSection("password-repeat", repeatPassword.text)); //For the password repeat check.
        
        UnityWebRequest www = UnityWebRequest.Post(Domain.subDomain("includes/signup.inc.php"), formData); //This initiates the post request.
        
        yield return www.SendWebRequest(); //This sends the post request.

        if (www.isNetworkError || www.isHttpError) //This checks for an error with the server.
        {
            Debug.Log(www.error); //This prints the error.
        }
        else
        {
            Debug.Log(www.downloadHandler.text); //This sends the error code or if it worked on the server side.
            InteractableForm(true);
        }

    }
}
