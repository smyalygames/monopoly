using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trade : MonoBehaviour
{
    //Essentials
    private Main main; //This is the main class in the game.
    
    //Screens
    public GameObject PlayerSelector; //This is where the current player selects who they want to trade with.
    public GameObject TradeMenu;
    
    //Player Selection
    private int player1; //This is the current player that is playing.
    private int player2;
    public TMP_Dropdown playerSelectDropdown; //This lets the player select what player it wants to trade with.
    public Button playerSelectionNext; //This is a button to click next on the player selection screen.
    
    //Player information
    private List<Property> player1Properties; //This creates a list of all the properties from player 1.
    private List<Property> player2Properties; //This creates a list of all the properties from player 2.
    
    //Core Trading
    public List<Button> player1PropertiesButtons; //This is a list for all the buttons for the properties for player 1.
    public List<Button> player2PropertiesButtons; //This is a list for all the buttons for the properties for player 2.

    private List<bool> player1SelectedProperties = new List<bool>(); //This is the selected properties for player 1 to trade.
    private List<bool> player2SelectedProperties = new List<bool>(); //This is the selected properties for player 2 to trade.

    //Function when opening the trade UI
    public void OpenTrade()
    {
        PlayerSelector.SetActive(true); //This opens the player selector.
        player1 = main.board.currentPlayer; //This gets the current player who wants to trade.

        //Editing the dropdown.
        List<string> playerNames = new List<string>(); //This creates a string of players for the dropdown list.
        playerSelectDropdown.ClearOptions(); //This clears all the dropdown items.
        foreach (Player player in main.board.players) //This goes through all of the players on the board
        {
            if (player.playerNumber != player1) //This checks so that the player cannot trade with themselves.
            {
                playerNames.Add(player.name); //Adds the player to the list of names.
            }
        }
        playerSelectDropdown.AddOptions(playerNames); //Adds all of the player names to the dropdown list.
    }

    //This function goes to the next screen after the player selection screen.
    private void PlayerSelectNext()
    {
        player2 = playerSelectDropdown.value; //This sets the ID of the player that the current player wants to trade with.
        if (player1 <= player2) //This accounts for the missing player1.
        {
            player2 += 1; //This increments the ID of player2.
        }
        
        //Getting the information about both players.
        player1Properties = main.board.players[player1].ownedProperties; //This gets all the properties owned by player 1.
        player2Properties = main.board.players[player2].ownedProperties; //This gets all the properties owned by player 2.
        
        //Player 1 buttons
        for (int i = 0; i < player1PropertiesButtons.Count; i++) //This goes through all of the buttons for Player 1.
        {
            player1PropertiesButtons[i].interactable = false; //This makes all of the buttons inactive.

            foreach (Property property in player1Properties) //This goes through all of player 1's owned properties.
            {
                if (player1PropertiesButtons[i].name == property.property_name) //If the property name is the same as the button name then...
                {
                    player1PropertiesButtons[i].interactable = true; //Make the button interactable.
                    break; //Stop searching for the current button name.
                }
            }
        }
        
        //Player 2 buttons
        for (int i = 0; i < player2PropertiesButtons.Count; i++) //This goes through all of the buttons for Player 2.
        {
            player2PropertiesButtons[i].interactable = false; //This makes all of the buttons inactive.
            
            foreach (Property property in player2Properties) //This goes through all of player 2's owned properties.
            {
                if (player2PropertiesButtons[i].name == property.property_name) //If the property name is the same as the button name then...
                {
                    player2PropertiesButtons[i].interactable = true; //Make the button interactable.
                    break; //Stop searching for the current button name.
                }
            }
        }

        //Switching the menus.
        PlayerSelector.SetActive(false); //This hides the player selector.
        TradeMenu.SetActive(true); //This opens the trade menu.
    }

    private void SelectProperty(int button, int player)
    {
        //This switch statement decides between which player the button has been assigned.
        switch (player)
        {
            case 1: //Player 1
                player1SelectedProperties[button] = !player1SelectedProperties[button]; //Switch between selection.
                break;
            case 2: //Player 2
                player2SelectedProperties[button] = !player2SelectedProperties[button]; //Switch between selection.
                break;
        }
    }

    void Awake()
    {
        main = FindObjectOfType<Main>(); //This gets the data from the main file.
        for (int i = 0; i < 28; i++)
        {
            player1SelectedProperties.Add(false);
            player2SelectedProperties.Add(false);
        }
    }

    void Start()
    {
        playerSelectionNext.onClick.AddListener(PlayerSelectNext); //This lets the next button work on the player selector.
        
        for (int i = 0; i < player1PropertiesButtons.Count; i++) //This goes through all of the buttons in the main menu.
        {
            int i1 = i;
            player1PropertiesButtons[i].onClick.AddListener(() => SelectProperty(i1, 1)); //This adds a specific menu for each of the buttons.
            player2PropertiesButtons[i].onClick.AddListener(() => SelectProperty(i1, 2)); //This adds a specific menu for each of the buttons.
        }
    }
}