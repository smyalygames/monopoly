using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public int roll;
	public bool movement = false;
	private int position = 0;
	public GameObject[] waypoints;
	public List<GameObject> players;
	public int currentPlayer;
	float rotSpeed;
	public float speed;
	double WPradius = 0.001;
	private Main main;
	private bool forward;


	public void Move(int previousPosition, int _roll, int playerNumber)
	{
		position = previousPosition;
		roll = _roll;
		forward = true;
		currentPlayer = playerNumber;
		movement = true;
	}

	public void MoveBack(int previousPosition, int back, int playerNumber)
	{
		forward = false;
		position = previousPosition;
		roll = back;
		currentPlayer = playerNumber;
		movement = true;
	}

	void Awake()
	{
		main = FindObjectOfType<Main>();
		players = main.playersGameObjects;
	}

	void Update()
	{
		if (!movement) return;

		if (!forward)
		{
			if ((Vector3.Distance(waypoints[position].transform.position, players[currentPlayer].transform.position) < WPradius) && position != roll)
			{
				position--;
				if (position < 0)
				{
					position = 39;
				}
			} else if ((Vector3.Distance(waypoints[position].transform.position, players[currentPlayer].transform.position) < WPradius) && position == roll)
			{
				movement = false;
				if (position == 30) //This checks if the player has landed on go to jail.
				{
					main.board.players[currentPlayer].GoToJail();
					return;
				}

				if (!main.board.CheckProperty(roll))
				{
					main.board.CheckFees();
				}
			}
		} else if ((Vector3.Distance(waypoints[position].transform.position, players[currentPlayer].transform.position) < WPradius) && roll == 40) //This checks if the player has to go to jail
		{
			position = 40;
		} 
		else if ((Vector3.Distance(waypoints[position].transform.position, players[currentPlayer].transform.position) < WPradius) && position != roll)
		{
			//Debug.Log(waypoints[current]);
			position++;
			if (position >= waypoints.Length-1)
			{
				position = 0;
			}
		} 
		else if ((Vector3.Distance(waypoints[position].transform.position, players[currentPlayer].transform.position) < WPradius) && position == roll)
		{
			movement = false;
			if (position == 30) //This checks if the player has landed on go to jail.
			{
				main.board.players[currentPlayer].GoToJail();
				return;
			}

			if (!main.board.CheckProperty(roll))
			{
				main.board.CheckFees();
			}
		}

		players[currentPlayer].transform.position = Vector3.MoveTowards(players[currentPlayer].transform.position, waypoints[position].transform.position, Time.deltaTime * speed);
	}
}
