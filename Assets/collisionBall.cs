using UnityEngine;
using System.Collections;

public class collisionBall : MonoBehaviour {

	private GameObject hits;

	// Use this for initialization
	void Start () {

		// get the particle system child object of the ball
		hits = GameObject.Find ("hitParticles");
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
	
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.transform.tag == "Block") {
			print ("Yes its a block");
			// below is to give a nice particle effect when ball hits something.
			ContactPoint2D contact = coll.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.right, contact.normal);
			Vector2 pos = contact.point;
			print(rot.eulerAngles);
			hits.transform.position = pos;
			hits.transform.rotation = rot;
			hits.GetComponent<ParticleSystem> ().Emit(100);
			//fix for making the particles into a V shape that always rotate on x axis
			//make an empty game object and parent the hitParticles to it and use below 
			//but above particles looks better for other applications.
			//hits.GetComponentInChildren<ParticleSystem> ().Emit(100);
			
		}
	}
	
}
