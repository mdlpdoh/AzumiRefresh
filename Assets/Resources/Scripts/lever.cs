using UnityEngine;
using System.Collections;

public class lever : MonoBehaviour {

	private GameObject leverOff;
	private GameObject leverOn;
	private GameObject ball;
	private bool leverState;


	// Use this for initialization
	void Start () {
		leverState = false;
		leverOff = GameObject.Find ("leverOff");
		leverOn = GameObject.Find ("leverOn");	
		ball = GameObject.Find ("Ball");	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.transform.tag == "Ball" && leverState == false && ball.transform.position.x > -.66) {
			print ("I got hit by ball");
			leverOff.GetComponent<SpriteRenderer> ().enabled = true; 
			leverOn.GetComponent<SpriteRenderer> ().enabled = false; 
			leverState = true;			 

		} else if (coll.transform.tag == "Ball" && leverState == true && ball.transform.position.x  < -.89){
			leverOff.GetComponent<SpriteRenderer> ().enabled = false; 
			leverOn.GetComponent<SpriteRenderer> ().enabled = true; 
			leverState = false;
		} 
	}
	
}//end class
