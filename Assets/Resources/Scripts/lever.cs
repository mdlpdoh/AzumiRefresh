using UnityEngine;
using System.Collections;

public class lever : MonoBehaviour {

	private GameObject leverOff;
	private GameObject leverOn;
	private GameObject leverOff2;
	private GameObject leverOn2;
	private GameObject ball;
	private GameObject movingWall1;
	private GameObject movingWall2;
	private bool leverState;


	// Use this for initialization
	void Start () {
		leverState = false;

		leverOff = GameObject.Find ("leverOff");
		leverOn = GameObject.Find ("leverOn");

		leverOff2 = GameObject.Find ("leverOff2");
		leverOn2 = GameObject.Find ("leverOn2");

		ball = GameObject.Find ("Ball");

		movingWall1 = GameObject.Find ("movingWall1");
		movingWall2 = GameObject.Find ("movingWall2");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.transform.tag == "Ball" && leverState == false && (ball.transform.position.x >= leverOn.transform.position.x || ball.transform.position.x >= leverOn2.transform.position.x)) {
			print ("I got hit by ball");
			leverOff.GetComponent<SpriteRenderer> ().enabled = true; 
			leverOff2.GetComponent<SpriteRenderer> ().enabled = true;

			leverOn.GetComponent<SpriteRenderer> ().enabled = false; 
			leverOn2.GetComponent<SpriteRenderer> ().enabled = false; 

			leverState = true;			 

			movingWall1.GetComponent<followPath> ().enabled = true; 
			movingWall2.GetComponent<followPath> ().enabled = true; 


		} else if (coll.transform.tag == "Ball" && leverState == true && (ball.transform.position.x <= leverOn.transform.position.x || ball.transform.position.x <= leverOn2.transform.position.x)){

			leverOff.GetComponent<SpriteRenderer> ().enabled = false;
			leverOff2.GetComponent<SpriteRenderer> ().enabled = false;

			leverOn.GetComponent<SpriteRenderer> ().enabled = true; 
			leverOn2.GetComponent<SpriteRenderer> ().enabled = true; 

			leverState = false;

			movingWall1.GetComponent<followPath> ().enabled = false; 
			movingWall2.GetComponent<followPath> ().enabled = false; 

		} 
	}
	
}//end class
