using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{

    public class Board //Creating the class for the board mechanics.
    {
        public int houses; //Initialising houses
        public int hotels; //Initialising hotels
        // List<properties.Property> cards = new List<properties.Property>;
        
		
		

        public Board()
        {
            houses = 32; //Defining the amount of houses - They have a finite amount
            hotels = 12; //Defining the amount of hotels - They have a finite amount
        }




    }

    // Start is called before the first frame update
    void Start()
    {
		//Property cards = new Property("cunt cunt cunt", "test", 200, 400);
        //Debug.Log(cards.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
