using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;


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
		parsedString = $"Rent: {property_rent}\n" +
		               $"Colour Set: {property_rent*2}\n" +
		               $"1 House: {property_house1}\n" +
		               $"2 Houses: {property_house2}\n" +
		               $"3 Houses: {property_house3}\n" +
		               $"4 Houses: {property_house4}\n" +
		               $"Hotel: {property_hotel}";
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
	

	public Board(List<Player> players, List<Property> properties)
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
	}

	public void MovePlayer(int roll) //This moves the player
	{
		players[currentPlayer].Move(roll); //This is telling the player to move in the local player class.
		textHandler.updateRoll(roll); //This is updating the text on the screen for the roll
		
		//Money UI
		int money = players[currentPlayer].money; //Gets the money the player has
		textHandler.updateMoney(money); //Updates the money on the UI
		
		//Property UI
		string propertyName = existingProperties[players[currentPlayer].position].property_name; //Gets the property name from where the player is at
		int propertyValue = existingProperties[players[currentPlayer].position].property_value; //Gets the value of the property of where the player is at

		if (propertyValue != 0) //This is for if the property can be bought
		{
			textHandler.updateProperty(propertyName, propertyValue); //Updates the UI text for the property info
		}
		else
		{
			textHandler.updateTile(propertyName); //Updates the UI text for the property info without the value of the property
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
		}
		
		textHandler.updateMoney(players[currentPlayer].money); //Updates the UI for the current amount of money the player has.
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
		buttonHandler.EnableRollDice(); //Re-enables the user to roll the dice.
	}

	public bool CheckBuyable(int position) //This checks if the property can be bought by the user.
	{

		Property property = existingProperties[position]; //Gets the property the player is currently at.
		
		for (int i = 0; i < avaliableProperties.Count; i++) //Checks through all of the properties that are buyable using a linear search
		{
			if (property.property_name == avaliableProperties[i].property_name) //Checks if the name exists in the available properties that can be purchased.
			{
				buttonHandler.EnableBuying(); //If it can, it will return true and break from the function
				return true; //Returns true if the property is buyable.
			}
		}
		buttonHandler.DisableBuying(); //If the name is not found, the property has not been found.
		buttonHandler.EnableRollDice(); //Allows the user to roll the dice if the property cannot be bought
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

		return (0, 0); //Default error.
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
				textHandler.updateMoney(players[currentPlayer].money); //This updates the amount of money the player has.
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

	private void PayRent()
	{
		
	}
}

public class Player
{
	public string name; //This is the username of the player
	private int playerNumber; //This is the player number in the queue
	public int money; //Initializes the variable for money.
	public int position; //Positions vary from 0-39 (40 squares on the board) (Go is 0)
	public bool inJail; //This enables specific in jail functions
	public List<Property> ownedProperties; //This is the list of properties that the player owns.
	public GameObject player;
	private Movement movement;
	private TextHandler textHandler;

	public Player(string playerName, int playerNumber, GameObject player)
	{
		name = playerName; //This initialises the username of the player
		position = 0; //This sets to the default position - GO
		inJail = false; //This initialises that the player isn't in jail
		this.playerNumber = playerNumber; //This is the position in the queue that the player is in
		money = 1500; //Set the default starting money.
		this.player = player; //This links the object that the player is linked to in the game
		ownedProperties = new List<Property>();
		movement = GameObject.FindObjectOfType<Movement>(); //This finds the movement script in the game
		textHandler = GameObject.FindObjectOfType<TextHandler>(); //Finds the text handler script
	}

	public void Move(int roll) //This moves the player a certain length (what they got from rolling the dice).
	{

		int previousPosition = position; //This saves the previous position the player was at
		
		position += roll; //Add the position with what was rolled.
		
		if (position >= 40) //If the player has reached or passed go then...
		{
			position -= 40; //As the player has gone round the board once, it removes the fact that it has gone around the board once.
			money += 200; //Collect money as they pass go.
		}

		movement.Move(previousPosition, position, playerNumber); //This moves the player.

		//return position; //Returns where the player needs to move to on the board
	}

	public void GoToJail() //If the player needs to go to jail. TODO
	{
		int previousPosition = position;
		Debug.Log("Jailed!!!"); //Prints a message to say that the player is in jail.
		position = 40; //Special position for jail.
		inJail = true; //Enables the in jail functions.
		movement.Move(previousPosition,position, playerNumber); //Moves the player to jail.
	}

	public void GetOutOfJail(int roll) //If the player is going out of jail. TODO
	{
		position = 10; //Moves the player out of jail.
		inJail = false; //Disables the inJail functions for the player.
		Move(roll); //Then moves the player.
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

	public void Pay(float fee) //This function makes the user pay.
	{
		money -= Convert.ToInt32(fee); //This deducts the money from the user's balance.
		textHandler.updateMoney(money); //This updates the text on the screen for the user.
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
			return true; //Says that mortgaging has been done successfully.
		}

		return false; //Says that mortgaging has not been done successfully.
	}
	
}

public static class MergeMethod
{
	public static List<Property> MergeSort(List<Property> unsorted)
	{	
		
		if (unsorted.Count <= 1) //Checks if the list is longer than 1 to do a merge sort
		{
			return unsorted; //Stops the function if the length is 1 or less
		}

		int middle = unsorted.Count / 2; //Does an integer division of 2

		List<Property> left = new List<Property>(); //Creates a list for the left items in the list.
		List<Property> right = new List<Property>(); //Creates a list for the right items in the list.


		for (int i = 0; i < middle; i++) //Adds the left half of the unsorted list to the left list.
		{
			left.Add(unsorted[i]);
		}

		for (int i = middle; i < unsorted.Count; i++) //Adds the rest of the unsorted list to the right list.
		{
			right.Add(unsorted[i]);
		}
		
		//Uses recursion to get to return early.
		left = MergeSort(left);
		right = MergeSort(right);
		
		//Merges the lists.
		return Merge(left, right);
	}

	private static List<Property> Merge(List<Property> left, List<Property> right)
	{
		List<Property> sorted = new List<Property>(); //Creates the list with the sort.

		while (left.Count > 0 || right.Count > 0) //While operates as the left and right lists aren't empty.
		{
			if (left.Count > 0 && right.Count > 0) //Checks if none of the lists are empty.
			{
				if (left.First().property_id <= right.First().property_id) //Checks if the left one is smaller than the right one.
				{
					sorted.Add(left.First());
					left.Remove(left.First());
				}
				else //If the right one is smaller than the left one then...
				{
					sorted.Add(right.First());
					right.Remove(right.First());
				}
			}
			else if (left.Count > 0) //Runs if the only list left is the left one.
			{
				sorted.Add(left.First());
				left.Remove(left.First());
			}
			else if (right.Count > 0) //Runs if the only list left is the right one.
			{
				sorted.Add(right.First());
				right.Remove(right.First());
			}
		}

		return sorted; //Returns the sorted list.
	}
}

public class Main : MonoBehaviour
{
	private List<Property> existingProperties;
	public GameObject[] waypoints; //These are all the predefined waypoints on the board.
	public Board board;

	//Player variables
	public List<Player> players = new List<Player>(); //Creates a list for all the players playing in the game.

	private void Awake()
	{
		//Adds the players to the game
		players.Add(new Player("smyalygames", 0, GameObject.Find("/Players/Player1")));
		players.Add(new Player("coomer", 1, GameObject.Find("/Players/Player2")));
		Debug.Log(players[0].name); //This is just checking if the player has been assigned.
		existingProperties = JsonConvert.DeserializeObject<List<Property>>(FileHandler.LoadProperties()); //This loads via JSON all the properties from a file which was originally downloaded from a server.
		board = new Board(players, existingProperties); //Creates the board class.
	}
	
}
