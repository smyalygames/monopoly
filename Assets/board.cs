using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using static properties;
=======
>>>>>>> cf686d8cd1e8be24b600a6d66e72755a749da53b

public class board : MonoBehaviour
{

    public class Board //Creating the class for the board mechanics.
    {
        public int houses; //Initialising houses
        public int hotels; //Initialising hotels
<<<<<<< HEAD
        // List<properties.Property> cards = new List<properties.Property>;
        Property cards = new Property("test", "test", 200, 400);
=======
        List<Property> cards = new List<Property>();
>>>>>>> cf686d8cd1e8be24b600a6d66e72755a749da53b

        public Board()
        {
            houses = 32; //Defining the amount of houses - They have a finite amount
            hotels = 12; //Defining the amount of hotels - They have a finite amount
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
