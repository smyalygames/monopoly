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
    public TextMeshProUGUI buyHouseText;
    public GameObject buyHouseObject;
    public Button sellHouse;
    public TextMeshProUGUI sellHouseText;
    public GameObject sellHouseObject;
    public Button mortgage;
    public TextMeshProUGUI mortgageText;
    private Button lastButton;

    //This is the function that will be used when the user clicks to open the inventory.

    public void UpdateInventory() //This is meant to update what is on the property panel when 
    {
        if (inventoryPanel.activeSelf) //This checks if the inventory panel is open.
        {
            OpenInventory(); //This updates only the inventory panel.
        }
        else if (propertyPanel.activeSelf) //This checks if the property panel is open.
        {
            OpenProperties(lastButton); //This updates the properties panel.
        }
    }

    public void OpenInventory() //This function renders the inventory panel.
    {
        Player currentPlayer = main.board.players[main.board.currentPlayer]; //Initialises the current player to make the code look neater.
        for (int i = 0; i < properties.Length; i++) //Checks through all of the buttons.
        {
            if (currentPlayer.ownedProperties.Count == 0)
            {
                properties[i].interactable = false;
                break;
            }
            for (int j = 0; j < currentPlayer.ownedProperties.Count; j++) //Checks through all of the owned properties.
            {
                if (currentPlayer.ownedProperties[j].property_name == properties[i].name) //Checks if the names of the property is the same as the button name/
                {
                    properties[i].interactable = true; //Makes the button for the property interactable.
                    break; //Stops the j for loop as the owned properties list is in order.
                }
                
                properties[i].interactable = false;
            }
        }
    }

    void OpenProperties(Button button) //This functions runs when a property has been clicked on in the inventory.
    {
        lastButton = button; //This caches the previously used button for updating when closed and reopened.
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
        
        propertyName.text = property.property_name; //This sets the name of the property title on screen.
        propertyColour.color = button.image.color; //This changes the background colour of the title to the colour of the button that was pressed.
        propertyRent.text = property.ParseRentInformation(); //This shows the rent for each possibilities of houses.
        propertyHouses.text = property.ParseHouses(); //This shows how many houses there are.
        
        backButton.onClick.AddListener(CloseProperties); //This adds a listener to go back to the main menu screen for the inventory.

        //These clears all of the previous listeners.
        buyHouse.onClick.RemoveAllListeners();
        sellHouse.onClick.RemoveAllListeners();
        //These adds listeners to the sell and buy buttons for this specific property.
        buyHouse.onClick.AddListener(() => BuyHouse(currentProperty, property.property_name));
        sellHouse.onClick.AddListener(() => SellHouse(currentProperty, property.property_name));

        switch (property.houses) //Checks how many houses the player has.
        {
            case 0: //This stops the player from selling a house when they have none.
                buyHouseText.text = "Buy House";
                sellHouse.interactable = false;
                buyHouse.interactable = true;
                break;
            case 4: //This stops the player from buying a house when they cannot buy more.
                buyHouseText.text = "Buy Hotel";
                buyHouse.interactable = true;
                sellHouse.interactable = true;
                break;
            case 5:
                buyHouseText.text = "Buy Hotel";
                buyHouse.interactable = false;
                sellHouse.interactable = true;
                break;
            default: //This is the default case.
                buyHouseText.text = "Buy House";
                buyHouse.interactable = true;
                sellHouse.interactable = true;
                break;
        }
        
        if (!main.board.players[main.board.currentPlayer].CheckColourSet(property.property_group)) //This checks if all of the colour group properties are owned.
        {
            //If they aren't all owned, the buy and sell buttons will be disabled.
            buyHouse.interactable = false;
            sellHouse.interactable = false;
        }

        if (property.property_group == "station" || property.property_group == "utilities") //This checks if the property is a station or a utility.
        {
            //This disables buying for stations or utility.
            buyHouseObject.SetActive(false);
            sellHouseObject.SetActive(false);
        }
        else
        {
            //This re-enables buying for houses.
            buyHouseObject.SetActive(true);
            sellHouseObject.SetActive(true);
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

    void BuyHouse(int currentProperty, string propertyName) //This is the function for the buy button.
    {
        main.board.BuyHouseOnProperty(propertyName); //This will pass the buy function in the main script.
        
        Property property = main.board.players[main.board.currentPlayer].ownedProperties[currentProperty]; //This caches the current property information.
        
        propertyHouses.text = property.ParseHouses(); //This changes the text for how many houses there are on the property.

        switch (property.houses) //This now checks how many houses there are on the property.
        {
            case 4: //This checks if there are enough houses to buy a hotel now.
                buyHouseText.text = "Buy Hotel"; //This changes the text on the button to buying a hotel.
                sellHouseText.text = "Sell House"; //This changes the text on the button to selling a house.
                break;
            case 5: //This checks if the property has reached its limit on buying properties.
                buyHouseText.text = "Buy Hotel"; //This changes the text on the button to buying a hotel.
                sellHouseText.text = "Sell Hotel"; //This changes the text on the button to selling a hotel.
                buyHouse.interactable = false; //This disables the user from buying more houses.
                break;
            default:
                buyHouseText.text = "Buy House"; //If there aren't enough for a hotel, the button will display Buy House.
                sellHouseText.text = "Sell House"; //This changes the button text to selling a house.
                break;
        }

        sellHouse.interactable = true; //As when a property will be bought, it will immediately allow the user to sell their houses.
    }

    void SellHouse(int currentProperty, string propertyName) //This is the function for the buy button.
    {
        main.board.SellHouseOnProperty(propertyName); //This will pass the sell function in the main script.
        
        Property property = main.board.players[main.board.currentPlayer].ownedProperties[currentProperty]; //This caches the current property information.
        
        propertyHouses.text = property.ParseHouses(); //This changes the text for how many houses there are on the property.

        switch (property.houses) //This now checks how many houses there are on the property.
        {
            case 0: //This checks if there are 0 houses left.
                buyHouseText.text = "Buy House"; //This changes the button text to buying a house.
                sellHouseText.text = "Sell House"; //This changes the button text to selling a house.
                sellHouse.interactable = false; //If there are no more houses, the player can't sell anymore, so the sell button will be disabled.
                break;
            case 4: //This is for when they have sold the hotel.
                buyHouseText.text = "Buy Hotel"; //This changes the button text to buying a hotel.
                sellHouseText.text = "Sell House"; //This changes the button text to selling a house.
                break;
            default:
                buyHouseText.text = "Buy House"; //This changes the button text to buying a house.
                sellHouseText.text = "Sell House"; //This changes the button text to selling a house.
                break;
        }

        buyHouse.interactable = true; //As when a property is sold, it means that the houses aren't at its max and hence enables the buy button. 
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
        OpenInventory(); //This updates the inventory panel.
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
