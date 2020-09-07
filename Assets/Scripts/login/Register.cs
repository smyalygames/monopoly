using System;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{

    public TextMeshProUGUI username; //This is for the username text input.
    public TextMeshProUGUI email; //This is for the email text input.
    public TextMeshProUGUI password; //This is for the password text input.
    public TextMeshProUGUI repeatPassword; //This is for the password that has been repeated in the text input.
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

    void Awake()
    {
        
    } 

    // Start is called before the first frame update
    void Start()
    {
        
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
}
