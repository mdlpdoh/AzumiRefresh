using UnityEngine;
using System.Collections;

public class coin : MonoBehaviour {

	//***** This script will destroy object on TRIGGER that has this script on it, when the ball hits it. (for ex. a Coin)

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
			print("Coin has been pocketed");
		}
	}

}//end class
