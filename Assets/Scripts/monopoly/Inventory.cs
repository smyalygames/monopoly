using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{

    private Main main;
    private Player currentPlayer;
    
    //Inventory
    public GameObject inventoryPanel;
    public Button[] properties; //These are all of the buttons in the inventory corresponding to each unique property.
    
    //Property Panel
    public GameObject propertyPanel;
    public Button backButton;
    public TextMeshProUGUI propertyName;
    public Image propertyColour;
    public TextMeshProUGUI propertyRent;
    public TextMeshProUGUI propertyHouses;
    public Button buyHouse;
    public Button sellHouse;
    public Button mortgage;
    public TextMeshProUGUI mortgageText;

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

    void OpenProperties(Button button) //This functions runs when a property has been clicked on in the inventory.
    {
        Property property = new Property(); //This is used for the property information to make the code look neater.
        int currentProperty = 50; //This is used for identifying the property in the ownedProperties list - 50 is used if the property wasn't found for some reason.

        for (int i = 0; i < main.board.players[main.board.currentPlayer].ownedProperties.Count; i++) //This checks through the current player's owned properties.
        {
            if (main.board.players[main.board.currentPlayer].ownedProperties[i].property_name == button.name) //This checks if owned property matches with the button pressed's name.
            {
                property = main.board.players[main.board.currentPlayer].ownedProperties[i]; //The clicked on property is set to the variable property.
                currentProperty = i; //The current property identifier is set.
                break; //This stops the for loop prematurely as the search is done.
            }
        }

        //Initialising the texts:
        /*
         *      TODO: Add House, Remove House
         *            buyHouse,  sellHouse
         */
        propertyName.text = property.property_name; //This sets the name of the property title on screen.
        propertyColour.color = button.image.color; //This changes the background colour of the title to the colour of the button that was pressed.
        propertyRent.text = property.ParseRentInformation(); //This shows the rent for each possibilities of houses.
        propertyHouses.text = property.ParseHouses(); //This shows how many houses there are.
        
        backButton.onClick.AddListener(CloseProperties); //This adds a listener to go back to the main menu screen for the inventory.
        
        buyHouse.onClick.AddListener(() => BuyHouse(currentProperty));
        sellHouse.onClick.AddListener(() => SellHouse(currentProperty));

        switch (property.houses) //Checks how many houses the player has.
        {
            case 0: //This stops the player from selling a house when they have none.
                sellHouse.interactable = false;
                buyHouse.interactable = true;
                break;
            case 4: //This stops the player from buying a house when they cannot buy more.
                buyHouse.interactable = false;
                sellHouse.interactable = true;
                break;
            default: //This is the default case.
                buyHouse.interactable = true;
                sellHouse.interactable = true;
                break;
        }
        
        //Buttons
        if (!main.board.players[main.board.currentPlayer].ownedProperties[currentProperty].mortgage) //This checks if the property is not mortgaged.
        {
            mortgage.onClick.RemoveAllListeners(); //This removes all previous onClick listeners.
            mortgage.onClick.AddListener(() => Mortgage(currentProperty)); //This adds a click listener to mortgage the property.
        }
        else //If the property is already mortgaged then...
        {
            mortgage.onClick.RemoveAllListeners(); //This removes all previous onClick listeners.
            mortgage.onClick.AddListener(() => Unmortgage(currentProperty)); //This adds a click listener to unmortgage the property.
        }
        
        inventoryPanel.SetActive(false); //This hides the inventory main menu.
        propertyPanel.SetActive(true); //This shows the property specific menu.
    }

    void BuyHouse(int currentProperty)
    {
        main.board.players[main.board.currentPlayer].ownedProperties[currentProperty].addHouse();
        Property property = main.board.players[main.board.currentPlayer].ownedProperties[currentProperty];
        propertyHouses.text = property.ParseHouses();
        if (property.houses >= 4)
        {
            buyHouse.interactable = false;
        }

        sellHouse.interactable = true;
    }

    void SellHouse(int currentProperty)
    {
        main.board.players[main.board.currentPlayer].ownedProperties[currentProperty].removeHouse();
        Property property = main.board.players[main.board.currentPlayer].ownedProperties[currentProperty];
        propertyHouses.text = property.ParseHouses();
        if (property.houses == 0)
        {
            sellHouse.interactable = false;
        }
        
        buyHouse.interactable = true;
    }

    void Mortgage(int currentProperty) //This function runs when the Mortgage button has been pressed.
    {
        if (main.board.players[main.board.currentPlayer].Mortgage(currentProperty)) //This runs the mortgage function for the property and checks if it was done so successfully.
        {
            mortgageText.text = "Unmortgage"; //This changes the button from Mortgage to Unmortgage.
            mortgage.onClick.RemoveAllListeners(); //This removes all previous onClick listeners.
            mortgage.onClick.AddListener(() => Unmortgage(currentProperty)); //This adds a click listener to unmortgage the property.
        }
    }

    void Unmortgage(int currentProperty) //This function runs when the Unmortgage button has been pressed.
    {
        if (main.board.players[main.board.currentPlayer].Unmortgage(currentProperty)) //This runs the unmortgage command for the property and checks if it was done so successfully.
        {
            mortgageText.text = "Mortgage"; //This changes the button from Unmortgage to Mortgage.
            mortgage.onClick.RemoveAllListeners(); //This removes all previous onClick listeners.
            mortgage.onClick.AddListener(() => Mortgage(currentProperty)); //This adds a click listener to mortgage the property.
        }
    }

    void CloseProperties() //This closes the property specific window and goes to the inventory main menu.
    {
        propertyPanel.SetActive(false); //This closes the property specific window.
        inventoryPanel.SetActive(true); //This opens the inventory main menu.
    }
    

    void Awake()
    {
        main = FindObjectOfType<Main>();
    }

    void Start()
    {
        for (int i = 0; i < properties.Length; i++) //This goes through all of the buttons in the main menu.
        {
            int i1 = i;
            properties[i].onClick.AddListener(() => OpenProperties(properties[i1])); //This adds a specific menu for each of the buttons.
        }
        
        backButton.onClick.AddListener(CloseProperties); //This adds the function to go back to the main inventory page on the property specific window.
    }
    
}
