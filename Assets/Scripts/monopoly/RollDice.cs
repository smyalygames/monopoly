using UnityEngine;
using UnityEngine.UI;

public class RollDice : MonoBehaviour
{
    System.Random random = new System.Random();

    private int dice1;
    private int dice2;
    private int current;
    private int totalRoll;
    private Main main;

    public Button m_RollDice;

    void Awake()
    {
        main = FindObjectOfType<Main>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_RollDice.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {

        current = main.board.currentPlayer;
        bool inJail = main.board.players[current].inJail;

        dice1 = random.Next(1, 7);
        dice2 = random.Next(1, 7);
        totalRoll = dice1 + dice2;
        
        if (inJail)
        {
            if (dice1 != dice2)
            {
                Debug.Log($"You rolled {dice1} and {dice2}");
                return;
            }
            else
            {
                Debug.Log($"You got out of jail with {dice1} and {dice2}!");
                main.board.players[current].GetOutOfJail(dice1, dice2);
                return;
            }
        }
    }
}