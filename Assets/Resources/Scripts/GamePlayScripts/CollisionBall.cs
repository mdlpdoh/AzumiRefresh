using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    /// <summary>
	/// This script is on the ball and activates the particles animation that happens when ball collides with another gameobject.
	/// </summary>
	public class CollisionBall : MonoBehaviour
	{
		public bool particlesEnabled;
		private GameObject hits;
	
		void Start ()
		{
			// get the particle system gameobject called "hitparticles" in the scene.
			hits = GameObject.Find ("hitParticles");
		}
	
		void emitParticleEffect (Collision2D coll)
		{
			// below is to give a nice particle effect when ball hits something.

			ContactPoint2D contact = coll.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.forward, contact.normal);
			Vector2 pos = contact.point;
			//print (rot.eulerAngles);
			hits.transform.position = pos;
			hits.transform.rotation = rot;
			hits.GetComponent<ParticleSystem> ().Emit (200);
		
			//fix for making the particles into a V shape that always rotate on x axis
			//make an empty game object and parent the hitParticles to it and use below 
			//but above particles looks better for other applications.
			//hits.GetComponentInChildren<ParticleSystem> ().Emit(100);
		}

		// Particle Effect on Collision
		void OnCollisionEnter2D (Collision2D coll)
		{
			if (coll.transform.tag == "Wall") 
            {
				if (particlesEnabled)
                {
					emitParticleEffect (coll);
				}

				EventManager.PostEvent(AzumiEventType.HitWall, this, coll);
			}
		}
	
	}//end class
}// end namespace