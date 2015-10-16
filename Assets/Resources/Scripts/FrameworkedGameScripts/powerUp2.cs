using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{
	public class powerUp2 : MonoBehaviour
	{

		private GameObject theBall;
		private GameObject thewhiteP;
		public static bool theSwitch;
	
		// Use this for initialization
		void Start ()
		{
		
			// get the rigidbody
			theBall = GameObject.Find ("Ball");
			thewhiteP = GameObject.FindWithTag ("give direction");
		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		void OnTriggerEnter2D (Collider2D col)
		{
			// turn switch ON
			moveBallToCoin.theSwitch = true;
			if (col.gameObject.name == "Ball") {
				//get the rigidbody og the ball
				Rigidbody2D getBall = theBall.GetComponent<Rigidbody2D> ();
				//find the magnitude of the ball
				float theMag = getBall.velocity.magnitude;
				//0 out the velocity of the ball
				//			getBall.velocity = new Vector2(0, 0);
				//get the position of the coin-up and the ball
				Vector2 whitePos = thewhiteP.transform.position;
				Vector2 ballPos = getBall.transform.position;
				//get the Trajectory of the ball to the coin-up
				Vector2 ballTraj = whitePos - ballPos;
				//normalize it
				Vector2 ballTrajN = ballTraj.normalized;
				//give the ball its new trajectory to the door
				getBall.velocity = ballTrajN * theMag;
				// kill the gold 
				gameObject.transform.localScale = new Vector3 (0.0F, 0.0F, 0);
			}
		}
	
	}//end class
}