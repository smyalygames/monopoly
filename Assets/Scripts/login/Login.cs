using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    
    public TextMeshProUGUI username; //This is for the username text input.
    public TextMeshProUGUI password; //This is for the password text input.
    public Button login; //This is the button used to register.

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
    
    // Start is called before the first frame update
    void Start()
    {
        
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
}
