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
    
    public AI()
    {
        
    }

    public int NextTask()
    {
        /* TASK DESCRIPTIONS:
         * 0 - Move
         */
        return 0;
    }

    public void RollDice()
    {
        dice1 = random.Next(1, 7);
        dice2 = random.Next(1, 7);

        roll = dice1 + dice2;
    }

}
