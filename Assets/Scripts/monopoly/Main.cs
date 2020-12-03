using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class Property
{
	
	public int property_id; //This is the position/id of the property
	public string property_name;//This is the name of the property
	public string property_group; //This is the group the property is in
	public int property_value; //This is how much it costs to buy the property
	public int? property_cost; //This is how much it costs to buy a house/hotel
	public int? property_rent; //This is the rent if the player that doesn't land on it has to pay
	public int? property_house1; //This is the rent if it has 1 house
	public int? property_house2; //This is the rent if it has 2 houses
	public int? property_house3; //This is the rent if it has 3 houses
	public int? property_house4; //This is the rent if it has 4 houses
	public int? property_hotel; //This is the rent if it has a hotel
	public int houses; //How many houses it has. (Can have up to 4)
	public bool hotel; //Whether it has a hotel or not. (Can only have 1)
	public bool mortgage; //Whether the property has been mortgaged.

	public Property()
	{
		houses = 0;
		hotel = false;
		mortgage = false;
	}

	public void addHouse() //Used to add a house.
	{
		if (!mortgage && (houses < 4) && !hotel) //Checks if there is not a mortgage and there are less than 4 houses and that there isn't a hotel.
		{
			houses++; //Adds a house to the property.
		} 
		else if (!mortgage && (houses == 4) && !hotel) //This is if a hotel is being created.
		{
			houses++; //Removes all the houses.
			hotel = true; //Creates the hotel.
		}
	}

	public void removeHouse() //Used to remove a house.
	{
		if (houses == 5 && hotel) //This is if there is a hotel.
		{
			houses--; //Gives the user back their 4 houses.
			hotel = false; //Removes the hotel.
		} 
		else if (houses > 0) //Checks if there is at least 1 house on the property.
		{
			houses--; //Removes the house from the property.
		}
	}

	public bool mortgageProperty() //Mortgages the property.
	{
		if (!hotel && (houses == 0) && !mortgage) //Checks if there is not a hotel and if there are no houses.
		{
			mortgage = true; //Mortgages the property.
			return true; //This returns true if done successfully.
		}
		
		return false; //This returns false if the property couldn't be mortgaged.
	}

	public bool unmortgageProperty() //Removes the mortgage on the property.
	{
		if (mortgage) //Checks if the property has been mortgaged.
		{
			mortgage = false; //Removes the mortgage.
			return true; //This returns true if done successfully.
		}

		return false; //This returns false if the property couldn't be unmortgaged.
	}

	public bool isBuyable() //This returns a boolean if the property is buyable
	{
		return property_value != 0; //This checks if the value of the property is set and then returns a boolean
	}

	public string ParseRentInformation() //This shows the information of how much rent a player would have to pay on the property for each circumstance.
	{
		string parsedString; //This is the string that will be returned in the end.
		//This plugs in the value of each variable into the string.
		switch (property_group) //This checks the property group
		{
			case "station": //This checks if the property is a station.
				parsedString = "Rent: 25\n" +
				               "If 2 Stations are owned: 50\n" +
				               "If 3 Stations are owned: 100\n" +
				               "If 4 Stations are owned: 200\n";
				break;
			case "utilities": //This checks if the property is a utility.
				parsedString = "If one Utility is owned,\n" +
				               "rent is 4 times amount\n" +
				               "shown on dice.\n\n" +
				               "If both Utilities are owned,\n" +
				               "rent is 10 times amount\n" +
				               "shown on dice.";
				break;
			default: //This is for the rest of the properties.
				parsedString = $"Rent: {property_rent}\n" +
				               $"Colour Set: {property_rent*2}\n" +
				               $"1 House: {property_house1}\n" +
				               $"2 Houses: {property_house2}\n" +
				               $"3 Houses: {property_house3}\n" +
				               $"4 Houses: {property_house4}\n" +
				               $"Hotel: {property_hotel}";
				break;
		}
		return parsedString; //This returns the parsed string.
	}

	public string ParseHouses() //This is used to show how many properties there are on the property tile.
	{
		string parsedString; //This is the string that will be displayed.

		switch (houses) //Checks how many "houses" there are on the property
		{
			case 0: //Checks if there are none.
				parsedString = "Properties: None"; //Writes that there are no properties.
				break;
			case 5:
				parsedString = "Properties: Hotel"; //Writes that there is a hotel.
				break;
			default:
				parsedString = $"Properties: {houses} Houses"; //Writes how many houses there are.
				break;
		}

		return parsedString; //Returns the string parsed.
	}
}

public class Board //Creating the class for the board mechanics.
{
	public int houses; //Initialising houses
	public int hotels; //Initialising hotels
	public int totalProperties; //Defining how many properties can exist.
	private TextHandler textHandler;
	private ButtonHandler buttonHandler;
	public List<Player> players;
	public int currentPlayer = 0;
	public List<Property> existingProperties;
	public List<Property> avaliableProperties = new List<Property>(); //Has a list of all the available properties.
	public List<Cards> chance = new List<Cards>();
	private int chancePointer;
	public List<Cards> communityChest = new List<Cards>();
	private int communityPointer;

	public Board(List<Player> players, List<Property> properties, List<Cards> existingCards)
	{
		this.players = players; //Imports all of the players playing
		Debug.Log(this.players.Count); //Prints how many players are playing
		textHandler = GameObject.FindObjectOfType<TextHandler>(); //Finds the text handler script
		buttonHandler = GameObject.FindObjectOfType<ButtonHandler>(); //Finds the button handler script
		existingProperties = properties; //This sets all of the properties that exist
		houses = 32; //Defining the amount of houses - They have a finite amount
		hotels = 12; //Defining the amount of hotels - They have a finite amount
		totalProperties = 28; //Sets the total properties in the game

		//Creates a list for all the buyable properties
		for (int i=0; i < properties.Count; i++)
		{
			if (properties[i].isBuyable())
			{
				avaliableProperties.Add(properties[i]);
			}
		}
		
		//This appends all the cards into their own respective pile.
		foreach (Cards card in existingCards)
		{
			switch (card.card_group) //Checks what group the card is in.
			{
				case 0: //If it is a chance card.
					chance.Add(card);
					break;
				case 1: //If it is a community chest card.
					communityChest.Add(card);
					break;
			}
		}
		
		chancePointer = 0;
		communityPointer = 0;
	}

	public void MovePlayer(int roll1, int roll2) //This moves the player
	{
		int totalRoll = roll1 + roll2;
		
		players[currentPlayer].Move(roll1, roll2); //This is telling the player to move in the local player class.
		textHandler.UpdateRoll(totalRoll); //This is updating the text on the screen for the roll
		
		//Money UI
		int money = players[currentPlayer].money; //Gets the money the player has
		textHandler.UpdateMoney(money); //Updates the money on the UI
		
		//Property UI
		string propertyName = existingProperties[players[currentPlayer].position].property_name; //Gets the property name from where the player is at
		int propertyValue = existingProperties[players[currentPlayer].position].property_value; //Gets the value of the property of where the player is at

		if (propertyValue != 0) //This is for if the property can be bought
		{
			textHandler.UpdateProperty(propertyName, propertyValue); //Updates the UI text for the property info
		}
		else
		{
			textHandler.UpdateTile(propertyName); //Updates the UI text for the property info without the value of the property
		}
		
		buttonHandler.DisableRollDice(); //Disables the user from being able to roll the dice whilst the player is moving
	}

	public void CheckFees() //This checks if the player has to pay for landing on a property
	{
		int playerPosition = players[currentPlayer].position; //It gets the current position of the player on the board
		Property currentProperty = existingProperties[playerPosition]; //It gets the property that it is currently on

		//This checks if the player landed on a tax tile
		if (currentProperty.property_rent >= 100) //Only tax tiles have a rent value of 100 or 200
		{
			players[currentPlayer].Pay(Convert.ToInt32(currentProperty.property_rent)); //Makes the player pay the tax and converts int? to int
			textHandler.UpdateMoney(players[currentPlayer].money); //Updates the UI for the current amount of money the player has.
			return;
		}

		var findProperty = FindOwner(currentProperty.property_name);

		if (findProperty.Item1 != 404)
		{
			int payment = 0;

			switch (currentProperty.property_group) //This checks the specific payment method per property type.
			{
				case "station": //If it is a station.
					switch (players[findProperty.Item1].CountProperties(currentProperty.property_group)) //Checks how many stations the owner owns.
					{
						case 1: //If 1, the player has to pay 25
							payment = 25;
							break;
						case 2: //If 2, the player has to pay 50
							payment = 50;
							break;
						case 3: //If 3, the player has to pay 100
							payment = 100;
							break;
						case 4: //If 4, the player has to pay 200
							payment = 200;
							break;
					}

					break;
				
				case "utilities": //If it is a utility.
					switch (players[findProperty.Item1].CountProperties(currentProperty.property_group)) //Checks how many utilities the owner owns.
					{
						case 1: //If it is 1, then the dice roll gets multiplied by 4 and that's how much they pay.
							payment = players[currentPlayer].diceRoll * 4;
							break;
						case 2: //If it is 2, then the dice roll gets multiplied by 10 and that's how much they pay.
							payment = players[currentPlayer].diceRoll * 10;
							break;
					}

					break;
				
				default: //Else, if it's a normal property.
					switch (currentProperty.houses) //Checks how many houses the owner has on the property.
					{
						case 0:
							if (players[findProperty.Item1].CheckColourSet(currentProperty.property_group)) //This checks if the player owns the whole colour set.
							{
								payment = Convert.ToInt32(currentProperty.property_rent)*2; //If they do own the whole colour set, the rent gets increased by 2.
								break;
							}
							payment = Convert.ToInt32(currentProperty.property_rent); //If they don't own the whole colour set, its the normal rent.
							break;
						case 1: //This is if the player has 1 house.
							payment = Convert.ToInt32(currentProperty.property_house1);
							break;
						case 2: //This is if the player has 2 houses.
							payment = Convert.ToInt32(currentProperty.property_house2);
							break;
						case 3: //This is if the player has 3 houses.
							payment = Convert.ToInt32(currentProperty.property_house3);
							break;
						case 4: //This is if the player has 4 houses.
							payment = Convert.ToInt32(currentProperty.property_house4);
							break;
						case 5: //This is if the player has a hotel.
							payment = Convert.ToInt32(currentProperty.property_hotel);
							break;
					}

					break;
			}

			players[currentPlayer].Pay(payment); //This charges the player that landed on the property.
			players[findProperty.Item1].Pay(-payment); //This gives the property owner the money.
		}
		
		textHandler.UpdateMoney(players[currentPlayer].money); //Updates the UI for the current amount of money the player has.
	}

	public void NextPlayer() //This moves the queue to the next player. TODO
	{
		if (currentPlayer + 1 >= players.Count) //If the counter is about to overflow in the queue, then...
		{
			Debug.Log("Restarted"); //Prints that the queue has started from the start
			currentPlayer = 0; //Starts the queue at 0 again.
		}
		else
		{
			currentPlayer++; //Increments the queue to the next player
		}
		textHandler.UpdateMoney(players[currentPlayer].money); //Changes the amount of money the new player has.
		buttonHandler.EnableRollDice(); //Re-enables the user to roll the dice.
	}

	public bool CheckProperty(int position) //This checks if the property can be bought by the user.
	{

		Property property = existingProperties[position]; //Gets the property the player is currently at.

		if (property.property_group == "chance")
		{
			UseCard(0);
			return false;
		} 
		
		if (property.property_group == "chest")
		{
			UseCard(1);
			return false;
		}
		
		for (int i = 0; i < avaliableProperties.Count; i++) //Checks through all of the properties that are buyable using a linear search
		{
			if (property.property_name == avaliableProperties[i].property_name) //Checks if the name exists in the available properties that can be purchased.
			{
				buttonHandler.EnableBuying(); //If it can, it will return true and break from the function
				return true; //Returns true if the property is buyable.
			}
		}
		buttonHandler.DisableBuying(); //If the name is not found, the property has not been found.
		buttonHandler.EnableNextTurn(); //Allows the user to roll the dice if the property cannot be bought
		return false; //Returns false if the property is not buybale.
	}

	public (int, int) FindOwner(string propertyName) //This function is used to find the property and the owner of it.
	{
		for (int i = 0; i < players.Count; i++) //This is the loop for the amount of players playing.
		{
			for (int j = 0; j < players[i].ownedProperties.Count; j++) //This is the loop for the amount of properties the currently searched player owns.
			{
				if (players[i].ownedProperties[j].property_name == propertyName) //This checks if the property name parameter matches the one that the player owns.
				{
					return (i, j); //It returns the player then the property position.
				}
			}
		}

		return (404, 404); //Default error.
	}

	public void BuyProperty()
	{
		int position = players[currentPlayer].position; //This is the current position of the player for the property.
		Property property = existingProperties[position]; //This gets the property that the player is buying
		int money = players[currentPlayer].money; //Gets the current amount of money the player has

		if (money - property.property_value < 0) //Checks if the player doesn't have enough money to pay for it
		{
			Debug.Log("The player doesn't have enough money!");
			buttonHandler.DisableBuying(); //Removes the buy button.
			buttonHandler.EnableRollDice(); //Re-enables the user to roll the dice.
			return; //Stops the function
		}

		for (int i = 0; i < avaliableProperties.Count; i++) //Checks through all of the properties that are buyable using a linear search
		{
			if (property.property_name == avaliableProperties[i].property_name) //Checks if the name exists in the available properties that can be purchased.
			{
				avaliableProperties.RemoveAt(i); //Removes the property from the list.
				players[currentPlayer].BuyProperty(property); //This buys the property in the player class
				textHandler.UpdateMoney(players[currentPlayer].money); //This updates the amount of money the player has.
				buttonHandler.DisableBuying(); //Removes the buy button.
				buttonHandler.EnableNextTurn();
				return; //Stops the function
			}
		}
		
		Debug.Log("The property cannot be bought!"); //Prints that theres an error
		
	}

	public void BuyHouseOnProperty(string propertyName) //This function links the UI button and this class to buy properties.
	{
		var location = FindOwner(propertyName); //This function finds the owner and the position of the property in the owner's list of properties.

		Property property = players[location.Item1].ownedProperties[location.Item2]; //This caches the property information that was selected.

		if (!players[location.Item1].CheckColourSet(property.property_group)) //This checks if the player owns all of the properties in the group.
		{
			return; //This stops the function if they don't own all the properties in the group.
		}

		if (property.houses < 4) //This checks if the property has less than 4 houses.
		{
			if (BuyHouse()) //This then buys a house locally.
			{
				players[location.Item1].ownedProperties[location.Item2].addHouse(); //This then buys the house in the Property class.
			}
		}
		else if (property.houses == 4) //This checks if the property has enough houses to buy a hotel.
		{
			if (BuyHotel()) //This buys a hotel locally.
			{
				players[location.Item1].ownedProperties[location.Item2].addHouse(); //This then buys the hotel in the Property class.
			}
		}
		
		players[location.Item1].Pay(Convert.ToInt32(property.property_cost)); //This then makes the player pay for the house that they bought.
		textHandler.UpdateMoney(players[location.Item1].money); //Updates the money on the UI.
	}

	public void SellHouseOnProperty(string propertyName) //This function links the UI button and this class to sell properties.
	{
		var location = FindOwner(propertyName); //This function finds the owner and the position of the property in the owner's list of properties.

		Property property = players[location.Item1].ownedProperties[location.Item2]; //This caches the property information that was selected.

		if (property.houses <= 4 && property.houses > 0) //This checks if the properties has a house(s).
		{
			SellHouse(); //This then locally sells the house(s) locally.
			players[location.Item1].ownedProperties[location.Item2].removeHouse(); //This then removes the house in the Property class.
		}
		else if (property.houses == 5) //This checks if the property has a hotel.
		{
			if (SellHotel()) //This then sells the hotel locally.
			{
				players[location.Item1].ownedProperties[location.Item2].removeHouse(); //This then removes the hotel in the Property class.
			}
		}
		
		players[location.Item1].Pay(Convert.ToInt32(property.property_cost)/-2); //This then gives back the money to the player for half the house/hotel's cost.
		textHandler.UpdateMoney(players[location.Item1].money); //Updates the money on the UI.
	}

	private bool BuyHouse() //This function is used to buy houses locally.
	{
		if (houses > 0) //This checks if there are enough houses to buy.
		{
			houses--; //This removes a house from the board as the house can be bought.
			return true; //This says that the house can be bought.
		}

		return false; //This says that the house cannot be bought.
	}

	private void SellHouse() //This function is used to sell a house locally.
	{
		houses++; //This adds a house to the board as the house has been sold.
	}

	private bool BuyHotel() //This function is used to buy a hotel locally.
	{
		if (hotels > 0) //This checks if there are enough hotels on the board.
		{
			houses += 4; //This adds 4 houses to the board as the player only has a hotel now.
			hotels--; //This removes a hotel from the board.
			return true; //This says that the hotel can be bought.
		}

		return false; //This says that the hotel cannot be bought.
	}

	private bool SellHotel() //This function is used to sell a hotel locally.
	{
		if (houses >= 4) //This first checks if there are enough houses to sell the hotel.
		{
			houses -= 4; //This removes 4 houses from the board and gives it to the property.
			hotels++; //This then adds a hotel to the board.
			return true; //This returns true to say that the hotel can be sold.
		}

		return false; //This returns false to say that the hotel cannot be sold.
	}

	public void UseCard(int group) //TODO
	{
		Cards card = new Cards(); //This initializes the variable card.
		
		switch (group)//This opens the menu to say what card the player has got.
		{
			case 0: //This is for Chance.
				card = SelectCard(group); //This selects a card from the deck.
				textHandler.ShowCard(group, card.card_text); //This displays the card details on the UI.
				break;
			case 1: //This is for Community Chest.
				card = SelectCard(group); //This selects a card from the deck.
				textHandler.ShowCard(group, card.card_text); //This displays the card details on the UI.
				break;
		}

		card = chance[10];

		(int, int) properties; //This is initialised to count the properties.
		int houses; //This is initialised to calculate the total cost of each house.
		int hotels; //This is initialised to calculate the total cost of each hotel.
		int extra; //This is used for converting extra to an int.
		int payment; //This is used to calculate the total payment required.
		
		switch (card.card_function) //This performs the function of the card.
		{
			case 1:
				//1 - move | extra - move to x (position)
				players[currentPlayer].CardMove(1, Convert.ToInt32(card.extra));
				break;
			case 2:
				//2 - bank gives money | extra - give x money (money)
				players[currentPlayer].Pay(-Convert.ToInt32(card.extra)); //This gives the player with what's predefined in extra converted to an int.
				break;
			case 3:
				//3 - make repairs 25 per house and 100 per hotel.
				properties = players[currentPlayer].CountProperties(); //This counts how many houses and hotels the player owns.
				houses = properties.Item1 * 25; //This charges 25 per house the player owns.
				hotels = properties.Item2 * 100; //This charges 100 per hotel the player owns.
				players[currentPlayer].Pay(houses + hotels); //The player gets fined here.
				break;
			case 4:
				//4 - advance to the nearest station - requires calculation pay the owner 2x the rent
				//TODO
				
				//Finds the player's current position.

				int playerPosition = players[currentPlayer].position;
				
				/* Station Positions:
				 * 
				 * Kings Cross - 5
				 * Marylebone Station - 15
				 * Fenchurch St. Station - 25
				 * Liverpool St. Station - 35
				 */

				int station;

				if (playerPosition < 5) //If it is before Kings Cross.
				{
					station = 5; //Go to Kings Cross.
				} 
				else if (playerPosition < 15) //If it is before Marylebone.
				{
					station = 15; //Go to Marylebone.
				}
				else if (playerPosition < 25) //If it is before Fenchurch St.
				{
					station = 25; //Go to Fenchurch St.
				}
				else if (playerPosition < 35) //If it is before Liverpool St.
				{
					station = 35; //Go to Liverpool St.
				}
				else //If it is past Liverpool St.
				{
					station = 5; //Go to Kings Cross.
				}

				players[currentPlayer].CardMove(4, station);
				break;
			case 5:
				//5 - advance to the nearest utility - make player roll dice, then pay owner 10x entitled pay.
				//TODO
				break;
			case 6:
				//6 - pay each player a sum of money | extra - money x
				extra = Convert.ToInt32(card.extra); //This converts extra to an int.
				payment = extra * (players.Count - 1); //This defines how much the player has to pay to each player.
				players[currentPlayer].Pay(payment); //This charges the current player.

				for (int i = 0; i < players.Count; i++) //This for loop will give every player the extra money.
				{
					if (i != currentPlayer) //This checks if the current player is not himself.
					{
						players[i].Pay(-extra); //This gives the player the money.
					}
				}

				break;
			case 7:
				//7 - pay bank | extra - money x
				players[currentPlayer].Pay(Convert.ToInt32(card.extra)); //This charges the player with what's predefined in extra converted to an int.
				break;
			case 8:
				//8 - go back 3 steps.
				players[currentPlayer].CardMove(8, 3);
				break;
			case 9:
				//9 - get out of jail free card
				players[currentPlayer].getOutOfJailCards += 1;
				break;
			case 10:
				//10 - street repairs, 40 per house and 115 per hotel.
				properties = players[currentPlayer].CountProperties(); //This counts how many houses and hotels the player owns.
				houses = properties.Item1 * 40; //This charges the player 40 per house. 
				hotels = properties.Item2 * 115; //This charges the player 115 per hotel/
				players[currentPlayer].Pay(houses + hotels); //The player gets fined here.
				break;
			case 11:
				//11 - collect money from every player | extra - money x
				extra = Convert.ToInt32(card.extra); //Converts extra into an int.

				for (int i = 0; i < players.Count; i++) //This goes through all the players in the game.
				{
					if (i != currentPlayer) //This checks if the current player is not selected.
					{
						players[i].Pay(extra); //This charges the players the fee from extra.
					}
				}

				payment = -extra * (players.Count - 1); //This calculates how much give to the current player.
				players[currentPlayer].Pay(payment); //This gives the player money from every player gave.
				break;
		}
		textHandler.UpdateMoney(players[currentPlayer].money); //Updates the money on the UI.
		buttonHandler.EnableNextTurn(); //This lets the player end the turn.
	}

	public Cards SelectCard(int group) //This function uses a queue to select a card.
	{
		Cards selectedCard = new Cards(); //This initialises the card that will be selected.
		
		switch (group) //This checks which group has been called for.
		{
			case 0: //Chance
				selectedCard = chance[chancePointer]; //This selects the card in the queue by it's pointer.
				if (chancePointer++ >= chance.Count) //This checks if the pointer will cause an overflow.
				{
					chancePointer = 0; //If so, it moves back to position 0.
					break;
				}
				
				chancePointer++; //If it won't overflow, it will increment the pointer.
				break;
			case 1: //Community Chest
				selectedCard = communityChest[communityPointer]; //This selects the card in the queue by it's pointer.
				if (communityPointer++ >= communityChest.Count) //This checks if the pointer will cause an overflow.
				{
					communityPointer = 0; //If so, it moves back to position 0.
					break;
				}

				communityPointer++; //If it won't overflow, it will increment the pointer.
				break;
		}

		return selectedCard; //This returns the card that was selected from the queue.
	}
}

public class Player
{
	public string name; //This is the username of the player
	private bool isAI; //This defines if the player is an AI. false = not an AI. true = is an AI.
	private int playerNumber; //This is the player number in the queue
	public int money; //Initializes the variable for money.
	public int position; //Positions vary from 0-39 (40 squares on the board) (Go is 0)
	public bool inJail; //This enables specific in jail functions
	public int getOutOfJailCards; //This counts the amount of get out of jail cards the user has.
	public int diceRoll;
	public List<Property> ownedProperties; //This is the list of properties that the player owns.
	public GameObject player;
	private Movement movement;
	private TextHandler textHandler;

	public Player(string playerName, bool isAI, int playerNumber, GameObject player)
	{
		name = playerName; //This initialises the username of the player
		this.isAI = isAI;
		position = 0; //This sets to the default position - GO
		inJail = false; //This initialises that the player isn't in jail
		getOutOfJailCards = 0; //This initialises the player to have 0 get out of jail free cards.
		this.playerNumber = playerNumber; //This is the position in the queue that the player is in
		money = 1500; //Set the default starting money.
		this.player = player; //This links the object that the player is linked to in the game
		ownedProperties = new List<Property>();
		movement = GameObject.FindObjectOfType<Movement>(); //This finds the movement script in the game
		textHandler = GameObject.FindObjectOfType<TextHandler>(); //Finds the text handler script
	}

	public void Move(int roll1, int roll2) //This moves the player a certain length (what they got from rolling the dice).
	{

		diceRoll = roll1 + roll2;
		int previousPosition = position; //This saves the previous position the player was at
		
		position += diceRoll; //Add the position with what was rolled.
		
		if (position >= 40) //If the player has reached or passed go then...
		{
			position -= 40; //As the player has gone round the board once, it removes the fact that it has gone around the board once.
			money += 200; //Collect money as they pass go.
			textHandler.UpdateMoney(money); //Updates the money on the UI.
		}

		movement.Move(previousPosition, position, playerNumber); //This moves the player.

		//return position; //Returns where the player needs to move to on the board
	}

	private void MoveToPosition(int position)
	{
		Debug.Log("Moving!");
		int previousPosition = this.position;
		this.position = position;
		
		movement.Move(previousPosition, this.position, playerNumber);
	}

	private void MoveBack(int roll)
	{
		int previousPosition = position; //This saves the previous position the player was at
		
		position -= roll; //Subtracts the position with what was rolled.
		
		if (position < 0) //If the player has reached or passed go then...
		{
			position += 40; //As the player has gone round the board once, it removes the fact that it has gone around the board once.
		}
		
		movement.MoveBack(previousPosition, position, playerNumber);
	}

	public void GoToJail() //If the player needs to go to jail. TODO
	{
		int previousPosition = position;
		Debug.Log("Jailed!!!"); //Prints a message to say that the player is in jail.
		position = 40; //Special position for jail.
		inJail = true; //Enables the in jail functions.
		movement.Move(previousPosition,position, playerNumber); //Moves the player to jail.
	}

	public void GetOutOfJail(int roll1, int roll2) //If the player is going out of jail. TODO
	{
		position = 10; //Moves the player out of jail.
		inJail = false; //Disables the inJail functions for the player.
		Move(roll1, roll2); //Then moves the player.
	}

	public void BuyProperty(Property property) //This function allows the player to own a property.
	{
		int price = property.property_value;
		
		if (money - price >= 0)
		{
			ownedProperties.Add(property); //Adds the property to the list of the player owned properties.
			ownedProperties = MergeMethod.MergeSort(ownedProperties);
			money -= price;
		}
		else
		{
			Debug.Log("Error: You do not have enough money to pay for the property!");
		}
		
	}
	
	public bool CheckColourSet(string colour) //Checks if the player has a whole colour set.
	{
		int required = 3; //This is the number of properties needed to own to buy houses.
		int counter = 0; //This checks how many times the property is found.

		if (colour == "brown" || colour == "dark blue") //Only brown and dark blue has 2 properties.
		{
			required = 2;
		}

		for (int i = 0; i < ownedProperties.Count && counter != required; i++)
		{
			if (ownedProperties[i].property_group == colour) //Checks if the owned property is in the same colour group.
			{
				counter++; //Increments the counter if a property was found to be owned.
			}
		}

		return (counter == required);
	}

	public int CountProperties(string colour) //This counts how many properties the player owns in a property group.
	{
		int required = 3; //This is the number of properties needed to own to buy houses.
		int counter = 0; //This checks how many times the property is found.
		
		if (colour == "brown" || colour == "dark blue" || colour == "utilities") //Only brown, dark blue and utilities has 2 properties.
		{
			required = 2;
		}
		else if (colour == "station") //This is if it is a station.
		{
			required = 4;
		}
		
		for (int i = 0; i < ownedProperties.Count && counter != required; i++)
		{
			if (ownedProperties[i].property_group == colour) //Checks if the owned property is in the same colour group.
			{
				counter++; //Increments the counter if a property was found to be owned.
			}
		}

		return counter; //Returns how many of the colour that the player owns.
	}

	public void Pay(float fee) //This function makes the user pay.
	{
		money -= Convert.ToInt32(fee); //This deducts the money from the user's balance.
	}

	public bool Mortgage(int currentProperty) //This is used for mortgaging a property.
	{
		if (currentProperty == 50) //Checks if there was an error - 50 is an error code.
		{
			return false; //Breaks the function and says that it wasn't completed.
		}

		if (ownedProperties[currentProperty].mortgageProperty()) //Mortgages the property and if done successfully..
		{
			Pay(ownedProperties[currentProperty].property_value / -2); //Gives the user 50% of what the property is worth. (/-2 makes it positive in the Pay function)
			textHandler.UpdateMoney(money); //Updates the money on the UI.
			return true; //Says that mortgaging has been done successfully.
		}

		return false; //Says that mortgaging has not been done successfully.
	}

	public bool Unmortgage(int currentProperty) //This is used for unmortgaging a property.
	{
		if (currentProperty == 50) //Checks if there was an error - 50 is an error code.
		{
			return false; //Breaks the function and says that it wasn't completed.
		}

		if (ownedProperties[currentProperty].unmortgageProperty()) //Unmortgages the property and if done successfully..
		{
			Pay((ownedProperties[currentProperty].property_value / 2) * 1.1f); //Makes the user pay what they got in mortgage plus a 10% interest
			textHandler.UpdateMoney(money); //Updates the money on the UI.
			return true; //Says that mortgaging has been done successfully.
		}

		return false; //Says that mortgaging has not been done successfully.
	}

	public (int, int) CountProperties()
	{
		int houses = 0;
		int hotels = 0;
		foreach (Property property in ownedProperties)
		{
			if (!property.hotel)
			{
				houses += property.houses;
			}
			else
			{
				hotels++;
			}
		}
		
		return (houses, hotels);
	}

	public void CardMove(int function, int position)
	{
		switch (function)
		{
			case 1:
				MoveToPosition(position);
				break;
			case 4:
				MoveToPosition(position);
				break;
			case 8:
				MoveBack(position);
				break;
		}
	}
}

public class Main : MonoBehaviour
{
	private List<Property> existingProperties;
	private List<Cards> existingCards;
	public Board board;

	//Player variables
	public List<Player> players = new List<Player>(); //Creates a list for all the players playing in the game.

	public GameObject playerParentGameObject; //This is where the parent for the player GameObjects goes.
	public List<GameObject> playersGameObjects; //This is the list of player GameObjects
	public GameObject playerTemplate; //This is the template for each new player created.

	private void Awake()
	{
		//Adds the players to the game

		for (int i = 0; i < GameSettings.players; i++)
		{
			//Duplicates the player template
			//Moves the new player into a parent for players
			//Names the game object player and a unique number
			Instantiate(playerTemplate, playerTemplate.transform.position, Quaternion.identity, playerParentGameObject.transform).name = $"Player{i}";
			playersGameObjects.Add(GameObject.Find($"/Players/Player{i}")); //Adds to a list of GameObjects by searching for the GameObject.
			players.Add(new Player($"Player {i}", false, i, playersGameObjects[i])); //Creates a unique player class for that specific GameObject
		}
		
		Destroy(playerTemplate); //Deletes the player template GameObject.

		Debug.Log(players[1].name); //This is just checking if the player has been assigned.
		existingProperties = JsonConvert.DeserializeObject<List<Property>>(FileHandler.LoadProperties()); //This loads via JSON all the properties from a file which was originally downloaded from a server.
		existingCards = JsonConvert.DeserializeObject<List<Cards>>(FileHandler.LoadCards()); //This loads via JSON all the cards from a file which was originally downloaded from a server.
		board = new Board(players, existingProperties, existingCards); //Creates the board class.
	}
	
}
