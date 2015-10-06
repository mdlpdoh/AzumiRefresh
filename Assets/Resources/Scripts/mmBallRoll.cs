using UnityEngine;
using System.Collections;

public class mmBallRoll : MonoBehaviour {
	
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

		// clamp the velocity or it will go througjh colliders...also notice Y is constrained in the rigidbody so it wont fly up due to physics
		myRb.velocity = Vector2.ClampMagnitude(myRb.velocity, clampSpeed);

		if (Input.GetMouseButtonDown (0)) {
			myRb.AddForce(transform.up * onTapSpeed);
		}
	}


}// end class
