using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    
    public Button rollDice;
    public GameObject buyButton;
    public Button buyButtonButton;

    public void disableRollDice()
    {
        rollDice.interactable = false;
    }

    public void enableRollDice()
    {
        rollDice.interactable = true;
    }

    public void disableBuying()
    {
        buyButton.SetActive(false);
    }

    public void enableBuying()
    {
        buyButton.SetActive(true);
    }


    private Main main;

    void Awake()
    { 
        main = FindObjectOfType<Main>();
    }

    void Start()
    {
        buyButtonButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick() {
        main.board.BuyProperty();
    }
}
