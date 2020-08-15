using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
	public int roll;
	public bool movement = false;
	private int position = 0;
	public GameObject[] waypoints;
	public GameObject[] players;
	public int currentPlayer;
	float rotSpeed;
	public float speed;
	double WPradius = 0.001;
	private Main main;


	public void Move(int _roll, int playerNumber)
	{
		roll = _roll;
		movement = true;
		currentPlayer = playerNumber;
	}

	void Awake()
	{
		main = FindObjectOfType<Main>();
	}

	void Update()
	{
		if (!movement) return;
		if ((Vector3.Distance(waypoints[position].transform.position, players[currentPlayer].transform.position) < WPradius) && position != roll)
		{
			//Debug.Log(waypoints[current]);
			position++;
			if (position >= waypoints.Length)
			{
				position = 0;
			}
		} else if ((Vector3.Distance(waypoints[position].transform.position, players[currentPlayer].transform.position) < WPradius) && position == roll)
		{
			main.board.CheckBuyable(roll);
			movement = false;
		}

		players[currentPlayer].transform.position = Vector3.MoveTowards(players[currentPlayer].transform.position,
			waypoints[position].transform.position, Time.deltaTime * speed);
	}
}
