using UnityEngine;
using System.Collections;

public class levelBarrier : MonoBehaviour {

	private GameObject theBall;
	private GameObject theLure;
	
	// Use this for initialization
	void Start () {
		
		// get the rigidbody
		theBall = GameObject.Find ("Ball");
		theLure = GameObject.Find ("theLure");
		print(theLure);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.name == "Ball")
		{

			//get the rigidbody og the ball
			Rigidbody2D getBall = theBall.GetComponent<Rigidbody2D>();
			//find the magnitude of the ball
			float theMagnitude = getBall.velocity.magnitude;
			//get the position of the magnet and the ball.The magnet is the empty gameobject you want ball to move towards.
			Vector2 lurePos = theLure.transform.position;
			Vector2 ballPos = getBall.transform.position;
			//get the Trajectory of the ball to the door
			Vector2 ballTraj = lurePos - ballPos;
			//normalize it
			Vector2 ballTrajN = ballTraj.normalized;
			//give the ball its new trajectory to the door
			getBall.velocity = ballTrajN * theMagnitude;
		}
	}
	
}//end class


