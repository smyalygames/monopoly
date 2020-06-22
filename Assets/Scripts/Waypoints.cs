using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Waypoints : MonoBehaviour
{
	public int roll;
	int position = 0;
	public GameObject[] waypoints;
	float rotSpeed;
	public float speed;
	double WPradius = 0.001;

	public void UpdateRoll(int passedRoll)
    {
		roll += passedRoll;
		if (roll >= 40)
        {
			roll -= 40;
        }
    }
	
    void Update()
    {
		if ((Vector3.Distance(waypoints[position].transform.position, transform.position) < WPradius) && position != roll)
		{
			//Debug.Log(waypoints[current]);
			position++;
			if (position >= waypoints.Length)
			{
				position = 0;
			}
		}
        transform.position = Vector3.MoveTowards(transform.position, waypoints[position].transform.position, Time.deltaTime * speed);
    }
}
