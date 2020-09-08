using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginUIHandler : MonoBehaviour
{

    //Login page
    public GameObject loginUI;
    public Button register;
    public Button forgotPassword;
    
    //Register page
    public GameObject registerUI;
    public Button backButtonRegister;

    void GoToRegister()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }

    void GoBack()
    {
        registerUI.SetActive(false);
        loginUI.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        register.onClick.AddListener(GoToRegister);
        backButtonRegister.onClick.AddListener(GoBack);
    }
}
