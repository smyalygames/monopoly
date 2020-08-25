using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{

    private Main main;
    
    //Inventory
    public GameObject inventoryPanel;
    public Button[] properties; //These are all of the buttons in the inventory corresponding to each unique property.
    
    //Property Panel
    public GameObject propertyPanel;
    public Button backButton;
    public TextMeshProUGUI propertyName;
    public Image propertyColour;
    public Button buyHouse;
    public Button sellHouse;
    public Button Mortgage;

    //This is the function that will be used when the user clicks to open the inventory.
    public void OpenInventory() 
    {
        Player currentPlayer = main.board.players[main.board.currentPlayer]; //Initialises the current player to make the code look neater.
        for (int i = 0; i < properties.Length; i++) //Checks through all of the buttons.
        {
            for (int j = 0; j < currentPlayer.ownedProperties.Count; j++) //Checks through all of the owned properties.
            {
                if (currentPlayer.ownedProperties[j].property_name == properties[i].name) //Checks if the names of the property is the same as the button name/
                {
                    properties[i].interactable = true; //Makes the button for the property interactable.
                    break; //Stops the j for loop as the owned properties list is in order.
                }
            }
        }
    }

    void OpenProperties(Button button)
    {
        inventoryPanel.SetActive(false);
        propertyPanel.SetActive(true);
        Debug.Log(button.name);
    }

    void CloseProperties()
    {
        propertyPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }
    

    private void Awake()
    {
        main = FindObjectOfType<Main>();
    }

    private void Start()
    {
        for (int i = 0; i < properties.Length; i++)
        {
            properties[i].onClick.AddListener(delegate { OpenProperties(properties[i]); });
        }
        
        backButton.onClick.AddListener(CloseProperties);
    }
}
