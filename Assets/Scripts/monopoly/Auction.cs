using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Auction : MonoBehaviour
{
    
    //Essentials
    private Main main; //This is the main class in the game.
    
    //Screens
    public GameObject AuctionScreen; //This is the main auction screen.
    public GameObject AuctionWon; //This displays what player won the auction.

    //Text Displays
    public TMP_Text propertyDetails; //This is meant to give the name and value of the property.
    public TMP_Text biddingInfo; //This is meant to show who has the highest bid and what that highest bid is.
    public TMP_Text currentBidders; //This is meant to show who is meant to go next and how many are left.
    
    //Buttons
    //Preset amounts
    public Button bid1; //This is the button to bid 1.
    public Button bid10; //This is the button to bid 10.
    public Button bid100; //This is the button to bid 100.
    //Custom amounts
    public TMP_InputField customBidInput; //This is where the user can input however much they want.
    public Button customBidButton; //This is where the user can confirm how much they want to bid.
    //Quit auction
    public Button quitAuction; //This is where the user stops bidding and removes themselves from the auction.
    
    //Information
    private int highestBidValue; //This is the highest value for the property in the bid.
    private int highestBidPlayer; //This is the player with the highest bid.
    private List<Player> auctionPlayerQueue; //This will be the queue for the players that are bidding.
    private int currentAuctionPlayer; //This is the current player in the queue for the auction.
    private Property property; //This is the property that will be auctioned

    //Starting function for the auction
    public void OpenTrade()
    {
        AuctionWon.SetActive(false); //This turns off the auction won screen from before.

        highestBidValue = 0; //This is the starting value for the highest bid.
        
        //Assigning players to the queue
        List<Player> auctionPlayerQueue = main.board.players; //This gets a temporary list of all of the players in the game.
        currentAuctionPlayer = 0; //This sets the auction queue to 0.
        int currentPlayer = main.board.currentPlayer - 1; //This gets the current player from the board.

        int playerPosition = main.board.players[currentPlayer].position; //This gets the position of the player that started the auction.

        property = main.board.existingProperties[playerPosition]; //This gets the property that is being auctioned.

        UpdatePropertyDetails(); //This shows the details of the property on the screen.
        UpdateBiddingInfo(true); //This updates the highest bidder information.
        NextPlayer(); //This increments the player.
        AuctionScreen.SetActive(true); //This shows the auction screen.
    }

    private void UpdatePropertyDetails()
    {
        propertyDetails.text = $"Property Name: {property.property_name}\n" +
                               $"Property Value: {property.property_value}";
    }
    
    private void UpdateBiddingInfo(bool start)
    {
        //This checks if it wants to show the information if its the start of the auction.
        if (start)
        {
            biddingInfo.text = $"Highest Bidder: -\n" +
                               $"Highest Bid: {highestBidValue}";
            return;
        }

        biddingInfo.text = $"Highest Bidder: {auctionPlayerQueue[highestBidPlayer].name}\n" +
                           $"Highest Bid: {highestBidValue}";
    }
    
    private void UpdateCurrentBidders()
    {
        currentBidders.text = $"Current Bidder: {auctionPlayerQueue[currentAuctionPlayer]}\n" +
                              $"Bidders Left: {auctionPlayerQueue.Count}";
    }

    //This moves on to the next player in the auction queue.
    private void NextPlayer()
    {
        
        //Incrementing to the next player
        if (currentAuctionPlayer >= auctionPlayerQueue.Count)
        {
            currentAuctionPlayer = 0;
        }
        else
        {
            currentAuctionPlayer++;
        }

        int money = auctionPlayerQueue[currentAuctionPlayer].money; //Gets the money of the current player in the queue
        int biddableMoney = money - highestBidValue;
        
        //Adds a maximum value to the custom bid.
        customBidInput.text = "";
        customBidInput.characterLimit = money.ToString().Length;

        //This disables each amounts of money if the player cannot afford to bid that amount.
        if (biddableMoney >= 100)
        {
            bid1.interactable = true;
            bid10.interactable = true;
            bid100.interactable = true;
        }
        else if (biddableMoney < 1)
        {
            bid1.interactable = false;
            bid10.interactable = false;
            bid100.interactable = false;
        }
        else if (biddableMoney < 10)
        {
            bid1.interactable = true;
            bid10.interactable = false;
            bid100.interactable = false;
        }
        else
        {
            bid1.interactable = true;
            bid10.interactable = true;
            bid100.interactable = false;
        }
        
        UpdateCurrentBidders(); //Updates the displayed text.
    }
    
    //This increases the bid by 1.
    private void Bid1()
    {
        highestBidValue += 1; //Increase the highest bid by 1.
        highestBidPlayer = currentAuctionPlayer; //Sets the highest bidding player to the current person who auctioned.
        UpdateBiddingInfo(false); //Updates the highest bidding information.
        
        //Moves on to the next player
        NextPlayer();
    }
    
    //This increases the bid by 10.
    private void Bid10()
    {
        highestBidValue += 10; //Increase the highest bid by 10.
        highestBidPlayer = currentAuctionPlayer; //Sets the highest bidding player to the current person who auctioned.
        UpdateBiddingInfo(false); //Updates the highest bidding information.
        
        //Moves on to the next player
        NextPlayer();
    }
    
    //This increases the bid by 100.
    private void Bid100()
    {
        highestBidValue += 100; //Increase the highest bid by 100.
        highestBidPlayer = currentAuctionPlayer; //Sets the highest bidding player to the current person who auctioned.
        UpdateBiddingInfo(false); //Updates the highest bidding information.
        
        //Moves on to the next player
        NextPlayer();
    }

    //This is for custom bids.
    private void BidCustom()
    {
        int money = Convert.ToInt32(customBidInput.text); //This sets the custom input value into a temporary variable, money converted to an integer.

        if (money == 0 || money + highestBidValue > auctionPlayerQueue[currentAuctionPlayer].money) //This checks if the player hasn't bid 0 or more than how much they own
        {
            return;
        }

        highestBidValue += money; //This increases the highest bid value by the custom amount.
        highestBidPlayer = currentAuctionPlayer; //Sets the highest bidding player to the current person who auctioned.
        UpdateBiddingInfo(false); //Updates the highest bidding information.
        
        //Moves on to the next player
        NextPlayer();
    }

    void Awake()
    {
        main = FindObjectOfType<Main>(); //This gets the data from the main file.
    }

    void Start()
    {
        //Add predefined bid buttons with its own function
        bid1.onClick.AddListener(Bid1); //For bids of 1.
        bid10.onClick.AddListener(Bid10); //For bids of 10.
        bid100.onClick.AddListener(Bid100); //For bids of 10.
        
        //Custom bid
        customBidButton.onClick.AddListener(BidCustom); //This allows the player to bid a custom amount.
    }
}
