using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public class Player
    {

        public int money; //Initializes the variable for money.
        public int position = 0; //Positions vary from 0-39 (40 squares on the board) (Go is 0)

        public Player()
        {
            money = 1500;
        }

        public void Move(int length) //This moves the player a certain length (what they got from rolling the dice).
        {
            if (position + length < 40) //Checks if the player will not pass go.
            {
                position += length; //Adds the length that the player needs to move.
            }
            else //If they pass go.
            {
                position = length - (39 - position); //Makes it so that the position is 
                money += 200; //Collect money as they pass go.

            }
        }

        public void goToJail() //If the player needs to go to jail.
        {
            position = 40; //Special position for jail.
        }

        public void getOutOfJail(int length) //If the player is going out of jail.
        {
            position = 10; //Moves the player out of jail.
            Move(length); //Then moves the player.
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
