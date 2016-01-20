using UnityEngine;
using System.Collections;

public class powerUp3 : MonoBehaviour {

	//***** This script moves a game object that has this sript on it toward the door when the ball hits it. (currently used on Coin)
	
	
	private GameObject theCoin;
	private GameObject theDoor;
	
	// Use this for initialization
	void Start ()
	{
		
		// get the rigidbody
		theCoin = GameObject.Find ("coin");
		theDoor = GameObject.Find ("Door");
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name == "Ball") {
			//get the rigidbody og the coin
			Rigidbody2D getCoin = theCoin.GetComponent<Rigidbody2D> ();
			//find the magnitude of the coin
			float theMag = getCoin.velocity.magnitude;
			//0 out the velocity of the coin
			//			getCoin.velocity = new Vector2(0, 0);
			//get the position of the door and the coin
			Vector2 doorPos = theDoor.transform.position;
			Vector2 coinPos = getCoin.transform.position;
			//get the Trajectory of the coin to the door
			Vector2 coinTraj = doorPos - coinPos;
			//normalize it
			Vector2 coinTrajN = coinTraj.normalized;
			//give the ball its new trajectory to the door
			getCoin.velocity = coinTrajN * theMag;
			
			
			
			//			getCoin.AddForce(transform.up * 500f);
			print ("Coin has been Moved To Door");

		}
	}
	
}//end class



