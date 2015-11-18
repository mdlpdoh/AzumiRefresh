using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
	public class AzumiBallRoll : MonoBehaviour
	{

		//private GameObject theBall;
//	private GameObject theDoor;
	
//	public mmGameManagerController gm;
		private Rigidbody2D myRb;
//	private GameObject hits;

		// speed at which ball starts rolling
		public float startSpeed = 200f;

		// speed that is added upon a click/tap
		//public float onTapSpeed = 100f;

		// velocity clamp
		//public float clampSpeed = 6f;

		// Use this for initialization

		private bool gamePointerDown = false;
		public float MinimumVelocity  = 0f;
		public float MaximumVelocity = 3f;

		public float MinimumMagnitude = 0.5f;
		public float MaximumMagnitude = 5f;


		void Start ()
		{
			print ("AzumiBallRoll start ");
			//theBall = GameObject.Find ("Ball");

			//make sure particle is off to start
//		gameObject.GetComponent<ParticleSystem> ().enableEmission = false;

			// get the particle system child object of the ball
//		hits = GameObject.Find ("hitParticles");

			// get the rigidbody
			myRb = gameObject.GetComponent<Rigidbody2D> ();

			// start ball rolling by adding force started at 500f but much better with a slower ball.
			myRb.AddForce (transform.right * startSpeed);

			// Listen For Input
//
			//EventManager.ListenForEvent (AzumiEventType.GameTap, OnGameTap);
			EventManager.ListenForEvent (AzumiEventType.GameSwipe, OnGameSwipe);
			EventManager.ListenForEvent (AzumiEventType.GamePress, OnGamePress);
		}
	
		// Update is called once per frame
		void Update ()
		{
			
			//myRb.velocity = Vector2.ClampMagnitude (myRb.velocity, clampSpeed);
		}

		void FixedUpdate ()
		{
			// clamp the velocity or it will go througjh colliders...also notice Y is constrained in the rigidbody so it wont fly up due to physics.
			//myRb.velocity = Vector2.ClampMagnitude (myRb.velocity, clampSpeed);
		}

		public void OnGameSwipe (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			Vector3 directionVector = (Vector3)Param;
		
			if (gamePointerDown && directionVector.magnitude != 0f){
				//print ("OnGameSwipe " + directionVector);
				float currentMagnitude = Mathf.Clamp( directionVector.magnitude,  MinimumMagnitude,  MaximumMagnitude); 
				float normalMagnitude = currentMagnitude/MaximumMagnitude;
				Vector3 normalVector = directionVector.normalized;

				//print ("vel " + Mathf.Clamp((normalMagnitude * MaximumVelocity), MinimumVelocity, MaximumVelocity));

				myRb.velocity = normalVector *  Mathf.Clamp((normalMagnitude * MaximumVelocity), MinimumVelocity, MaximumVelocity);
		

				/*
				myRb.AddForce((Vector3)Param * onTapSpeed );
				myRb.velocity =Vector2.ClampMagnitude(myRb.velocity, clampSpeed);
				gamePointerDown = false;
				*/
			}
		}
		public void OnGamePress (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			print ("OnGamePress");
			gamePointerDown = true;
		}





		/*
		public void OnGameTap (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			// find pos of ball and mouseclick and move ball away from the mouseclick.
		
			//get the rigidbody og the ball
			//Rigidbody2D getBall = theBall.GetComponent<Rigidbody2D> ();
			//find the magnitude of the ball
			float theMag = myRb.velocity.magnitude;
			//0 out the velocity of the ball
			//			getBall.velocity = new Vector2(0, 0);

			Vector2 tapPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				
			//print (tapPos);
			//get the position of the tap and the ball
			Vector2 ballPos = myRb.transform.position;
			//get the Trajectory of the ball to the tap
			Vector2 ballTraj = ballPos - tapPos;
			//normalize it 
			Vector2 ballTrajN = ballTraj.normalized;
			//give the ball its new trajectory away from tap
//				getBall.velocity = ballTrajN * theMag;
			Vector2 theForce = ballTrajN * theMag;
			myRb.AddForce (theForce * onTapSpeed);

//				getBall.velocity = Vector2.ClampMagnitude(getBall.velocity, clampSpeed);
//				myRb.AddForce (transform.up * onTapSpeed);
				
		}*/

	}// end class
}
