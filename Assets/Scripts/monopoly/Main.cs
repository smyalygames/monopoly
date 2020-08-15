using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine.UI;


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
	public int houses = 0; //How many houses it has. (Can have up to 4)
	public bool hotel = false; //Whether it has a hotel or not. (Can only have 1)
	public bool mortgage = false; //Whether the property has been mortgaged.

	public void addHouse() //Used to add a house.
	{
		if (!mortgage && (houses < 4) && !hotel) //Checks if there is not a mortgage and there are less than 4 houses and that there isn't a hotel.
		{
			houses += 1; //Adds a house to the property.
		}
	}

	public void removeHouse() //Used to remove a house.
	{
		if (houses > 0) //Checks if there is at least 1 house on the property.
		{
			houses -= 1; //Removes the house from the property.
		}
	}

	public void addHotel() //Used to add a hotel.
	{
		if (houses == 4) //Checks if the user has enough houses.
		{
			houses = 0; //Removes all the houses.
			hotel = true; //Creates the hotel.
		}
	}

	public void removeHotel() //Removes the hotel.
	{
		if (hotel) //Checks if they have a hotel already.
		{
			houses = 4; //Gives the user back their 4 houses.
			hotel = false; //Removes the hotel.

		}
	}

	public void mortgageProperty() //Mortgages the property.
	{
		if (!hotel && (houses == 0)) //Checks if there is not a hotel and if there are no houses.
		{
			mortgage = true; //Mortgages the property.
		}
	}

	public void unmortgageProperty() //Removes the mortgage on the property.
	{
		if (mortgage) //Checks if the property has been mortgaged.
		{
			mortgage = false; //Removes the mortgage.
		}
	}

	public bool isBuyable() //This returns a boolean if the property is buyable
	{
		return property_value != 0; //This checks if the value of the property is set and then returns a boolean
	}
}

public class Board //Creating the class for the board mechanics.
{
	public int houses; //Initialising houses
	public int hotels; //Initialising hotels
	public int totalProperties; //Defining how many properties can exist.
	private TextHandler textHandler;
	private ButtonHandler buttonHandler;
	private List<Player> players;
	private int currentPlayer = 0;
	public List<Property> existingProperties;
	public List<Property> avaliableProperties = new List<Property>(); //Has a list of all the available properties.
	

	public Board(List<Player> players, List<Property> properties)
	{
		this.players = players;
		Debug.Log(this.players.Count);
		textHandler = GameObject.FindObjectOfType<TextHandler>();
		buttonHandler = GameObject.FindObjectOfType<ButtonHandler>();
		existingProperties = properties; //This sets all of the properties that exist
		houses = 32; //Defining the amount of houses - They have a finite amount
		hotels = 12; //Defining the amount of hotels - They have a finite amount
		totalProperties = 28;
		for (int i=0; i < properties.Count; i++)
		{
			if (properties[i].isBuyable())
			{
				avaliableProperties.Add(properties[i]);
			}
		}
	}

	public void MovePlayer(int player, int roll)
	{
		players[player].Move(roll);
		textHandler.updateRoll(roll);
		
		int money = players[player].money;
		textHandler.updateMoney(money);
		
		string propertyName = existingProperties[players[player].position].property_name;
		int propertyValue = existingProperties[players[player].position].property_value;

		if (propertyValue != 0)
		{
			textHandler.updateProperty(propertyName, propertyValue);
		}
		else
		{
			textHandler.updateTile(propertyName); //property_value
		}
		
		buttonHandler.disableRollDice();
	}

	public bool CheckBuyable(int position) //This checks if the property can be bought by the user.
	{

		Property property = existingProperties[position]; //Gets the property the player is currently at.
		
		for (int i = 0; i < avaliableProperties.Count; i++) //Checks through all of the properties that are buyable using a linear search
		{
			if (property.property_name == avaliableProperties[i].property_name) //Checks if the name exists in the available properties that can be purchased.
			{
				buttonHandler.enableBuying(); //If it can, it will return true and break from the function
				return true; //Returns true if the player can buy the property.
			}
		}
		buttonHandler.disableBuying(); //If the name is not found, the property has not been found.
		buttonHandler.enableRollDice(); //Lets the player continue moving if they can't buy the property.
		return false; //Returns false if the player can't buy the property.
	}

	public void BuyProperty()
	{
		int position = players[currentPlayer].position; //This is the current position of the player for the property.
		Property property = existingProperties[position]; //This gets the property that the player is buying

		for (int i = 0; i < avaliableProperties.Count; i++) //Checks through all of the properties that are buyable using a linear search
		{
			if (property.property_name == avaliableProperties[i].property_name) //Checks if the name exists in the available properties that can be purchased.
			{
				avaliableProperties.RemoveAt(i); //Removes the property from the list.
				players[currentPlayer].BuyProperty(property); //This buys the property in the player class
				textHandler.updateMoney(players[currentPlayer].money); //This updates the amount of money the player has.
				buttonHandler.disableBuying();
				buttonHandler.enableRollDice();
				return;
			}
		}
		
		Debug.Log("The property cannot be bought!");
		
	}
}

public class Player
{
	private string name;
	private int playerNumber;
	public int money; //Initializes the variable for money.
	public int position = 0; //Positions vary from 0-39 (40 squares on the board) (Go is 0)
	public List<Property> ownedProperties = new List<Property>();
	public GameObject player;
	private Movement movement;

	public Player(string playerName, int playerNumber, GameObject player)
	{
		name = playerName;
		this.playerNumber = playerNumber;
		money = 1500; //Set the default starting money.
		this.player = player;
		movement = GameObject.FindObjectOfType<Movement>();
	}

	public void Move(int roll) //This moves the player a certain length (what they got from rolling the dice).
	{

		position += roll; //Add the position with what was rolled.
		if (position >= 40) //If the player has reached or passed go then...
		{
			position -= 40; //As the player has gone round the board once, it removes the fact that it has gone around the board once.
			money += 200; //Collect money as they pass go.
		}

		movement.Move(position, playerNumber);

		//return position; //Returns where the player needs to move to on the board
	}

	public void GoToJail() //If the player needs to go to jail.
	{
		position = 40; //Special position for jail.
	}

	public void GetOutOfJail(int length) //If the player is going out of jail.
	{
		position = 10; //Moves the player out of jail.
		//Move(length); //Then moves the player.
	}

	public void BuyProperty(Property property) //This function allows the player to own a property.
	{

		int price = property.property_value;
		
		if (money - price >= 0)
		{
			ownedProperties.Add(property); //Adds the property to the list of the player owned properties.
			money -= price;
		}
		else
		{
			Debug.Log("Error: You do not have enough money to pay for the property!");
		}
	}
	
}

public class Main : MonoBehaviour
{
	private List<Property> existingProperties;
	public GameObject[] waypoints;
	public Board board;

	//Player variables
	public List<Player> players = new List<Player>();

	private void Awake()
	{
		players.Add(new Player("smyalygames", 0, GameObject.Find("/Players/Player1")));
		players.Add(new Player("coomer", 1, GameObject.Find("/Players/Player2")));
		Debug.Log(players[0].player.name);
		
		existingProperties = JsonConvert.DeserializeObject<List<Property>>(FileHandler.LoadProperties());
		board = new Board(players, existingProperties);
	}
	
}
