using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ForgotPassword : MonoBehaviour
{
    
    public TMP_InputField email; //This is for the email text input.
    public Button submit; //This is the button used to submit forgot password.

    private bool CheckIsEmpty() //This checks if the strings are empty.
    {
        var strings = new List<string> {email.text}; //This puts all of the text boxes into an array.
        string check = "​"; //This is used for the check, the ZWSP is TMP's way of identifying nothing as null.
        if (strings.Contains(check)) //If any of the inputs are null.
        {
            return true;
        }
        
        return false;
    }
    
    void InteractableForm(bool decision)
    {
        email.interactable = decision;
        enabled = decision;
    }

    // Start is called before the first frame update
    void Start()
    {
        submit.onClick.AddListener(delegate
        {
            StartCoroutine(ForgotPasswordForm());
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIsEmpty()) //This checks if all of the inputs are empty.
        {
            submit.interactable = false; //If they are empty, disable the register button.
        }
        else
        {
            submit.interactable = true; //If all of them are filled, then allow the user to register.
        }
    }
    
    IEnumerator ForgotPasswordForm()
    {
        InteractableForm(false);
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //POST Data
        formData.Add(new MultipartFormDataSection("email", email.text)); //For the email.

        UnityWebRequest www = UnityWebRequest.Post(Domain.subDomain("includes/reset-request.inc.php"), formData); //This initiates the post request.
        
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
