using System;
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
    private Player player1Information; //This is just the class of information about player 1.
    private Player player2Information; //This is just the class of information about player 1.
    private List<Property> player1Properties; //This creates a list of all the properties from player 1.
    private List<Property> player2Properties; //This creates a list of all the properties from player 2.
    
    //Displays
    //Name Display
    public TMP_Text player1Name; //This is the text used to show player 1's name on screen.
    public TMP_Text player2Name; //This is the text used to show player 2's name on screen.
    //Money Display
    public TMP_Text player1MoneyText; //This is used to show how much money player 1 has.
    public TMP_Text player2MoneyText; //This is used to show how much money player 2 has.
    
    //Core Trading
    //Properties
    public List<Toggle> player1PropertiesButtons; //This is a list for all the buttons for the properties for player 1.
    public List<Toggle> player2PropertiesButtons; //This is a list for all the buttons for the properties for player 2.
    //Money
    public TMP_InputField player1Money; //This is the amount taken from player 1.
    private int player1MoneyInt; //This is the int version of player1Money text.
    public TMP_InputField player2Money; //This is the amount taken from player 2.
    private int player2MoneyInt; //This is the int version of player1Money text.
    //Complete trade
    public Button tradeButton; //This button is meant to finish the trade.
    
    //Confirmation Box
    public GameObject ConfirmationBox; //This is the confirmation box that will pop up.
    public TMP_Text confirmationText; //This is the text that will ask if the player is sure about the trade.
    public Button confirmationButtonYes; //This is if player 2 decides yes.
    public Button confirmationButtonNo; //This is if player 2 doesn't want to trade.
    
    //Completion
    public GameObject CompletionBox;

    //Function when opening the trade UI
    public void OpenTrade()
    {
        TradeMenu.SetActive(false); //This hides the trade menu (if it was open from before)
        ConfirmationBox.SetActive(false); //This hides the confirmation box (if it was on from before)
        CompletionBox.SetActive(false); //This hides the completion box (if it was on from before)
        tradeButton.interactable = true; //This turns on the trade button (if it was disabled from before)
        
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
        
        PlayerSelector.SetActive(true); //This opens the player selector.
    }

    //This function goes to the next screen after the player selection screen.
    private void PlayerSelectNext()
    {
        player2 = playerSelectDropdown.value; //This sets the ID of the player that the current player wants to trade with.
        if (player1 <= player2) //This accounts for the missing player1.
        {
            player2 += 1; //This increments the ID of player2.
        }
        
        //Player Information (classes)
        player1Information = main.board.players[player1]; //This is the class for player 1.
        player2Information = main.board.players[player2]; //This is the class for player 2.
        
        //Player names
        player1Name.text = player1Information.name; //This sets player 1's name on screen.
        player2Name.text = player2Information.name; //This sets player 2's name on screen.
        
        //Player money
        player1MoneyText.text = $"Total Money: {player1Information.money}"; //This sets player 1's balance on screen.
        player2MoneyText.text = $"Total Money: {player2Information.money}"; //This sets player 2's balance on screen.
        player1Money.text = ""; //This empties what was written into the input box.
        player2Money.text = ""; //This empties what was written into the input box.
        
        //Restricting the maximum value that can be input
        //This is done by converting the int to a string, so the amount of characters can be counted.
        player1Money.characterLimit = player1Information.money.ToString().Length; //This sets a soft limit on how much player 1 can spend.
        player2Money.characterLimit = player2Information.money.ToString().Length; //This sets a soft limit on how much player 2 can spend.

        //Getting the property information about both players.
        player1Properties = main.board.players[player1].ownedProperties; //This gets all the properties owned by player 1.
        player2Properties = main.board.players[player2].ownedProperties; //This gets all the properties owned by player 2.
        
        //Player 1 buttons
        for (int i = 0; i < player1PropertiesButtons.Count; i++) //This goes through all of the buttons for Player 1.
        {
            player1PropertiesButtons[i].interactable = false; //This makes all of the buttons inactive.
            player1PropertiesButtons[i].isOn = false; //This removes all of the ticks on the buttons.

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
            player2PropertiesButtons[i].isOn = false; //This removes all of the ticks on the buttons.
            
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

    //Confirmation Boxes
    private void StartConfirm() //This is when player 1 presses trade.
    {

        //Converting player1Money.text to an int.
        if (player1Money.text != "")
        {
            player1MoneyInt = Convert.ToInt32(player1Money.text);
        }
        else
        {
            player1MoneyInt = 0;
        }
        
        //Converting player2Money.text to an int.
        if (player2Money.text != "")
        {
            player2MoneyInt = Convert.ToInt32(player2Money.text);
        }
        else
        {
            player2MoneyInt = 0;
        }
        
        if (player1MoneyInt > player1Information.money || player2MoneyInt > player2Information.money) //This checks if the player has put too much money to trade.
        {
            return; //If they have, then don't let the player continue.
        }
        
        tradeButton.interactable = false; //This stops player 1 from being able to press trade multiple times.
        confirmationButtonYes.interactable = true; //This allows the yes button to be pressed (if it was disabled from before)
        confirmationText.text = $"{main.board.players[player2].name}\nAre you sure you want to complete the trade?";
        ConfirmationBox.SetActive(true); //This shows the confirmation box.
    }

    private void CancelConfirm() //This is if player 2 decides they don't want to trade.
    {
        ConfirmationBox.SetActive(false); //This hides the confirmation box.
        tradeButton.interactable = true; //This enables player 2 to trade again.
    }

    //Finish the trades between players.
    private void CompleteTrade()
    {
        confirmationButtonYes.interactable = false; //This disables the yes button to be pressed again, to prevent exploits.

        //Money Trades
        if (player1MoneyInt > 0) //This checks if player 1 is trading money. If so then...
        {
            //Take money away from player 1.
            main.board.players[player1].Pay(player1MoneyInt);
            //Give the money to player 2.
            main.board.players[player2].Pay(-player1MoneyInt);
            main.board.players[player1].UpdateMoneyText(); //This updates the amount of money player 1 has on screen.
        }
        if (player2MoneyInt > 0) //This checks if player 2 is trading money. If so then...
        {
            //Take money away from player 2.
            main.board.players[player2].Pay(player2MoneyInt);
            //Give the money to player 1.
            main.board.players[player1].Pay(-player2MoneyInt);
            main.board.players[player1].UpdateMoneyText(); //This updates the amount of money player 1 has on screen.
        }

        //Property Trades
        //for player 1 to give to player 2
        foreach (Toggle propertyButtons in player1PropertiesButtons) //This goes through all of the player buttons.
        {
            if (!propertyButtons.isOn) continue; //If the property is not selected, then skip searching through the owned property list.
            foreach (Property property in player1Information.ownedProperties)
            {
                if (property.property_name == propertyButtons.name) //If the selected properties are matching then...
                {
                    main.board.players[player1].RemoveProperty(property); //Takes away the property from Player 1.
                    main.board.players[player2].AddProperty(property); //Gives the property to Player 2.
                    break; //Stops the property for loop.
                }
            }
        }
        //for player 2 to give to player 1
        foreach (Toggle propertyButtons in player2PropertiesButtons) //This goes through all of the player buttons.
        {
            if (!propertyButtons.isOn) continue; //If the property is not selected, then skip searching through the owned property list.
            foreach (Property property in player2Information.ownedProperties)
            {
                if (property.property_name == propertyButtons.name) //If the selected properties are matching then...
                {
                    main.board.players[player2].RemoveProperty(property); //Takes away the property from Player 2.
                    main.board.players[player1].AddProperty(property); //Gives the property to Player 1.
                    break; //Stops the property for loop.
                }
            }
        }
        
        //Dealing with screens.
        TradeMenu.SetActive(false);
        ConfirmationBox.SetActive(false);
        CompletionBox.SetActive(true);
    }

    void Awake()
    {
        main = FindObjectOfType<Main>(); //This gets the data from the main file.
    }

    void Start()
    {
        playerSelectionNext.onClick.AddListener(PlayerSelectNext); //This lets the next button work on the player selector.
        tradeButton.onClick.AddListener(StartConfirm); //This lets the player 1 ask player 2 if he wants to complete the trade.
        confirmationButtonNo.onClick.AddListener(CancelConfirm); //This button cancels the trade from player 2.
        confirmationButtonYes.onClick.AddListener(CompleteTrade); //This button accepts the trade from player 2.
    }
}