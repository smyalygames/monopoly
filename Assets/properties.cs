using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public class Property
    {
        public string title; //The name of the property.
        public string group; //The colour, station or utility.
        public int value; //How much it costs to buy.
        public int houseCost; //How much it costs to buy a house on that property.
        public int houses = 0; //How many houses it has. (Can have up to 4)
        public bool hotel = false; //Whether it has a hotel or not. (Can only have 1)
        public bool mortgage = false; //Whether the property has been mortgaged.

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
