using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDice : MonoBehaviour
{
    System.Random random = new System.Random();

    private int current;
    private Waypoints waypoints;
    

    public Button m_RollDice;

    void Awake()
    {
        waypoints = GameObject.FindObjectOfType<Waypoints>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_RollDice.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void TaskOnClick() {
        current = random.Next(1, 7) + random.Next(1, 7);
		Debug.Log("Rolled: " + current);
        waypoints.UpdateRoll(current);
	}
}
