using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
	public class mmRollandShrink : MonoBehaviour
	{

		//	public mmGameManagerController gm;
		private Rigidbody2D myRb;
		private GameObject hits;
	
		// Use this for initialization
		void Start ()
		{
		
			//make sure particle is off to start
			//		gameObject.GetComponent<ParticleSystem> ().enableEmission = false;
		
			// get the particle system child object of the ball
			hits = GameObject.Find ("hitParticles");
		
			// get the rigidbody
			myRb = gameObject.GetComponent<Rigidbody2D> ();
		
			// start ball rolling by adding force started at 500f but much better with a slower ball.
			myRb.AddForce (transform.right * 200f);
		
			// get the game manager script
			//		gm = GameObject.Find("GameManager").GetComponent<mmGameManagerController>();
		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		void FixedUpdate ()
		{
		
			// clamp the velocity or it will go througjh colliders...also notice Y is constrained in the rigidbody so it wont fly up due to physics
			myRb.velocity = Vector2.ClampMagnitude (myRb.velocity, 50f);
		
			if (Input.GetMouseButtonDown (0)) {
				myRb.AddForce (transform.up * 100f);
			}
		}
	
		//	void OnCollisionEnter(Collision coll){
		//		if (coll.collider.tag == "Wall") {
		//			gm.incBounces ();
		//			gm.getBounces ();
		//		} else if (coll.collider.tag == "success") {
		//			gm.getSuccess();
		//			Destroy(this.gameObject);
		//		}
		//	}
	
		void OnCollisionEnter2D (Collision2D coll)
		{
			if (coll.transform.tag == "Block") {
				print ("Yes its a block");
				// below is to give a nice particle effect when ball hits something.
				ContactPoint2D contact = coll.contacts [0];
				Quaternion rot = Quaternion.FromToRotation (Vector3.right, contact.normal);
				Vector2 pos = contact.point;
				print (rot.eulerAngles);
				hits.transform.position = pos;
				hits.transform.rotation = rot;
				hits.GetComponent<ParticleSystem> ().Emit (100);
				//fix for making the particles into a V shape that always rotate on x axis
				//make an empty game object and parent the hitParticles to it and use below 
				//but above particles looks better for other applications.
				//hits.GetComponentInChildren<ParticleSystem> ().Emit(100);
			
				// below is to make ball smaller every time it hits a wall.
				gameObject.transform.localScale -= new Vector3 (0.01F, 0.01F, 0);
				if (gameObject.transform.localScale.y < 0f) {
					print ("I am less than ZERO!");
					Destroy (gameObject);
					print ("GAME OVER");
				}
			}
		}
	
	}// end class
}