﻿using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class wallMovePingPong : MonoBehaviour 
{
	//list of transforms called Points
	public Transform[] Points;
	//IEnumerators let you examine a thing and then move to the next thing (sequence).
	public IEnumerator<Transform> GetPathEnumerator() {
		//the iterator block
		//in order to follow a path we need one point so we check to make sure its smaller than 1 yield break.
		if (Points == null || Points.Length < 1)
			yield break;
		var direction = 1;
		var index = 0;
		// this loop will cause movement to PING-PONG.
		while (true) {
			//yield excecution back to whoever is invoking the enumerator.
			yield return Points[index];
			//this if statement catches if there is only One point, so it won't throw an exception.
			if (Points.Length == 1)
				continue;
			//execution will then continue.
			//because we are yielding inside this loop, wherever it is called has the contol over its start and end so no endless loop.
			if (index <= 0)
				direction = 1;
			else if (index >= Points.Length - 1)

				direction = -1;
			// if index + a -1 then it will be moving backwards.
			index = index + direction;
		}
	}

	public void OnDrawGizmos()
	{
		//ensure that there are atleast 2 points to ensure there are enough points to make a line.points object will be null until list is created so use Points == null.
		if (Points == null || Points.Length < 2)
			return;
		//loop over all points and start index on 1. we are looking at 2nd point in array to draw from prev point to current point.
		for (var i = 1; i < Points.Length; i++) {
			//from Points[i - 1].position TO Points[i].position
			Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
		}
	}
}
