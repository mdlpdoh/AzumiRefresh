using UnityEngine;
using System.Collections;

public class azumiBallRoll : MonoBehaviour {

	private GameObject theBall;
//	private GameObject theDoor;
	
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

		theBall = GameObject.Find ("Ball");

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

		// find pos of ball and mouseclick and move ball away from the mouseclick.
		if (Input.GetMouseButtonDown (0)) {
				//get the rigidbody og the ball
				Rigidbody2D getBall = theBall.GetComponent<Rigidbody2D> ();
				//find the magnitude of the ball
				float theMag = getBall.velocity.magnitude;
				//0 out the velocity of the ball
				//			getBall.velocity = new Vector2(0, 0);

				Vector2 tapPos = Input.mousePosition;
				print ("ilugliugsfliugasdilfgalsidugflaisudgfliasdufglaisdufg");
				print(tapPos);
				//get the position of the tap and the ball
				Vector2 ballPos = getBall.transform.position;
				//get the Trajectory of the ball to the tap
				Vector2 ballTraj = ballPos - tapPos;
				//normalize it 
				Vector2 ballTrajN = ballTraj.normalized;
				//give the ball its new trajectory away from tap
//				getBall.velocity = ballTrajN * theMag;
				Vector2 theForce = ballTrajN * theMag;
				getBall.AddForce(theForce * onTapSpeed);
				getBall.velocity = Vector2.ClampMagnitude(getBall.velocity, clampSpeed);
//				myRb.AddForce (transform.up * onTapSpeed);
		}
	}


}// end class
