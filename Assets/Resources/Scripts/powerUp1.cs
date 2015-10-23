using UnityEngine;
using System.Collections;

public class powerUp1 : MonoBehaviour
{
	//***** This script moves the ball toward the door when it hits gameobject that has this script on it (for ex a power-up).


	private GameObject theBall;
	private GameObject theDoor;

	// Use this for initialization
	void Start ()
	{

		// get the rigidbody
		theBall = GameObject.Find ("Ball");
		theDoor = GameObject.Find ("Door");
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name == "Ball") {
			//get the rigidbody og the ball
			Rigidbody2D getBall = theBall.GetComponent<Rigidbody2D> ();
			//find the magnitude of the ball
			float theMag = getBall.velocity.magnitude;
			//0 out the velocity of the ball
//			getBall.velocity = new Vector2(0, 0);
			//get the position of the door and the ball
			Vector2 doorPos = theDoor.transform.position;
			Vector2 ballPos = getBall.transform.position;
			//get the Trajectory of the ball to the door
			Vector2 ballTraj = doorPos - ballPos;
			//normalize it
			Vector2 ballTrajN = ballTraj.normalized;
			//give the ball its new trajectory to the door
			getBall.velocity = ballTrajN * theMag;



//			getBall.AddForce(transform.up * 500f);
			print ("Ball has been Moved To Opening");
			print (theMag);
			print (doorPos);
			print (ballPos);
			print (ballTraj);
			print (ballTrajN);
		}
	}

}//end class


