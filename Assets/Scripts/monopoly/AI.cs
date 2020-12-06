using System.Collections;
using System.Collections.Generic;

public class AI
{
    /* TODO
     * Write out a way to roll a dice.
     * Buy properties
     * Buy houses
     * Trade
     * Etc...
     */
    
    System.Random random = new System.Random(); //Random Function.

    //Dice Rolling Mechanism
    public int dice1; //First dice.
    public int dice2; //Second dice.
    private int roll; //Total of both dice.
    
    //AI Info
    public int money; //This tracks how much money the AI can deal with.
    public int position; //This is the current position of the AI on the board.
    private bool startedRound; //This checks when the player has started the round.
    
    //AI Abilities
    public bool buyProperty; //This tells the AI if it is allowed to buy it's own property.
    
    //Tasks
    private int currentTask; //This is the current task that the player is performing
    
    /* TASK DESCRIPTIONS:
     * 0 - Move
     * 1 - Buy
     */
     
    public AI(int money)
    {
        //AI Info
        this.money = money;
        position = 0;
        startedRound = false;
        
        //AI Abilities
        buyProperty = false;
    }

    public int NextTask()
    {
        if (!startedRound) //This checks if the player has not started the round.
        {
            //The AI will decide to move.
            RollDice(); //The AI will roll the dice.
            currentTask = 0; //It will set the current task to 0 to represent movement.
        }
        else if (buyProperty)
        {
            currentTask = 1; //This will buy a property.
            buyProperty = false;
        }
        return currentTask;
    }

    public void RollDice()
    {
        dice1 = random.Next(1, 7);
        dice2 = random.Next(1, 7);

        roll = dice1 + dice2;
    }

}
