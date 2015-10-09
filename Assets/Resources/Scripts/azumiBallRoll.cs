using UnityEngine;
using System.Collections;

public class azumiBallRoll : MonoBehaviour {

	private GameObject theBall;
	private GameObject theDoor;
	
//	public mmGameManagerController gm;
	private Rigidbody2D myRb;
//	private GameObject hits;

	// speed at which ball starts rolling
	public float startSpeed = 200f;

	// speed that is added upon a click/tap
	public float onTapSpeed = 300f;

	// velocity clamp
	public float clampSpeed = 10f;

	// Use this for initialization
	void Start () {

		// find the ball and door to move from door on click in fixedUpdate.
		theBall = GameObject.Find ("Ball");
		theDoor = GameObject.Find ("Door");

		//make sure particle is off to start
//		gameObject.GetComponent<ParticleSystem> ().enableEmission = false;

		// get the particle system child object of the ball
//		hits = GameObject.Find ("hitParticles");

		// get the rigidbody
		myRb = gameObject.GetComponent<Rigidbody2D> ();

		// start ball rolling by adding force started at 500f but much better with a slower ball.
		myRb.AddForce(transform.right * startSpeed);

		// get the game manager script
//		gm = GameObject.Find("GameManager").GetComponent<mmGameManagerController>();

	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {

		// clamp the velocity or it will go througjh colliders...also notice Y is constrained in the rigidbody so it wont fly up due to physics.
		myRb.velocity = Vector2.ClampMagnitude(myRb.velocity, clampSpeed);

		if (Input.GetMouseButtonDown (0)) {
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
				Vector2 ballTraj = ballPos - doorPos;
				//normalize it
				Vector2 ballTrajN = ballTraj.normalized;
				//give the ball its new trajectory to the door
				getBall.velocity = ballTrajN * theMag;

//				myRb.AddForce (transform.up * onTapSpeed);
		}
	}


}// end class
