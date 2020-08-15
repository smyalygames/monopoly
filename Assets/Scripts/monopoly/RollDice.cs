using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDice : MonoBehaviour
{
    System.Random random = new System.Random();

    private int current;
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

    void TaskOnClick() {
        current = random.Next(1, 7) + random.Next(1, 7);
        main.board.MovePlayer(0, current);
    }
}
