using UnityEngine;
using System.Collections;

public class destroyer : MonoBehaviour {

	//***** This script will destroy whatever object it is on when the ball hits it.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.name == "Ball")
		{
			Destroy(this.gameObject);
		}
	}
	
}//end class



