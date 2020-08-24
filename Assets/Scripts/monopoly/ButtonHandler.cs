using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{

    public GameObject GameUI;
    public Button rollDice;
    public GameObject buyButton;
    public Button buyButtonButton;

    //Inventory
    public GameObject PropertyUI;
    public Button inventory;
    public TextMeshProUGUI inventoryText;
    private bool inventoryOpen = false;
    

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

    void OpenInventory()
    {
        GameUI.SetActive(false);
        PropertyUI.SetActive(true);
        inventoryText.text = "Close Inventory";
    }

    void CloseInventory()
    {
        GameUI.SetActive(true);
        PropertyUI.SetActive(false);
        inventoryText.text = "Open Inventory";
    }

    void ToggleInventory()
    {
        if (!inventoryOpen)
        {
            OpenInventory();
            inventoryOpen = true;
        }
        else
        {
            CloseInventory();
            inventoryOpen = false;
        }
    }


    private Main main;

    void Awake()
    { 
        main = FindObjectOfType<Main>();
    }

    void Start()
    {
        buyButtonButton.onClick.AddListener(BuyPropertyClick);
        inventory.onClick.AddListener(ToggleInventory);
    }

    void BuyPropertyClick() {
        main.board.BuyProperty();
    }
}
