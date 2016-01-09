using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
	public class moveBallToCoin : MonoBehaviour
	{

		private GameObject theBall;
		private GameObject theCoin;
		public static bool theSwitch;
	
		// Use this for initialization
		void Start ()
		{
		
			// get the rigidbody
			theBall = GameObject.Find ("Ball");
			theCoin = GameObject.FindWithTag ("gold coin");
//		theSwitch = false;

		
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
				//get the position of the coin-up and the ball
				Vector2 coinPos = theCoin.transform.position;
				Vector2 ballPos = getBall.transform.position;
				//get the Trajectory of the ball to the coin-up
				Vector2 ballTraj = coinPos - ballPos;
				//normalize it
				Vector2 ballTrajN = ballTraj.normalized;
				//give the ball its new trajectory to the door
				getBall.velocity = ballTrajN * theMag;

				if (theSwitch == true) {
					theBall.gameObject.transform.localScale = new Vector3 (1F, 1F, 0);
					Destroy (gameObject);
				} else {
					//make the ball small so it can get to gold 
					theBall.gameObject.transform.localScale = new Vector3 (0.2F, 0.2F, 0);
				}

			}
		}
	
	}//end class
}