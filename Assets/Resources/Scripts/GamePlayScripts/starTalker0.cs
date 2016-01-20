using UnityEngine;
using System.Collections;

public class starTalker0 : MonoBehaviour {

	//***** This script will tell the starListener Script which star got hit (and starListener will destroy the ones that did not get hit).
	private static starListener starListener;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name == "Ball")
		{
			starListener.instance.destroyer(0);
		}
	}
	
}//end class



