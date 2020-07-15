using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Property
{
    /*
    public string title; //The name of the property.
    public string group; //The colour, station or utility.
    public int value; //How much it costs to buy.
    public int houseCost; //How much it costs to buy a house on that property.
    */
    public int property_id;
    public string property_name;
    public string property_group;
    public int? property_value;
    public int? property_cost;
    public int? property_rent;
    public int? property_house1;
    public int? property_house2;
    public int? property_house3;
    public int? property_house4;
    public int? property_hotel;
    public int houses = 0; //How many houses it has. (Can have up to 4)
    public bool hotel = false; //Whether it has a hotel or not. (Can only have 1)
    public bool mortgage = false; //Whether the property has been mortgaged.

    /*
    public Property(string name, string importGroup, int importValue, int house) //For houses.
    {
        //Initialising all the variables.
        title = name;
        group = importGroup;
        value = importValue;
        houseCost = house;
    }

    public Property(string name, string importGroup, int importValue) //For stations or utility.
    {
        //Initialising all the variables.
        title = name;
        group = importGroup;
        value = importValue;
    }
    */

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

	public string printTitle() {
		return property_name;
	}
}

public class properties : MonoBehaviour
{
    public List<Property> data;
    int test;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetProperties());
        
    }

    void Update()
    {
        if (data != null && test == 0)
        {
            Debug.Log(data[1].printTitle());
            test++;
        }
    }

    IEnumerator GetProperties()
    {
        UnityWebRequest www = UnityWebRequest.Get(Domain.subDomain("includes/get-properties.inc.php"));
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            string json = www.downloadHandler.text;
            data = JsonConvert.DeserializeObject<List<Property>>(json);
        }
    }


    public void currentProperty(int propertyNum)
    {
        Debug.Log(data[propertyNum].printTitle());
    }

}
