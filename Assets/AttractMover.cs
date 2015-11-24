using UnityEngine;
using System.Collections;

public class AttractMover : MonoBehaviour {

	// Use this for initialization
	public float attractStrength = 0.1f;
	
	// Update is called once per frame
		private float startDistance =0f;
	     public float AttractRadius =0f;
		public Rigidbody2D particleRotator;
            public Rigidbody2D ParticleParent;
            
            void Start(){
                  particleRotator.angularVelocity = 200;
            }
	       void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name == "Ball")
            {
				startDistance =  (col.transform.position- transform.position).magnitude;
            }
        }

	      void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.name == "Ball")
            {
             Vector2 attractVector = (transform.position- col.transform.position);
			 
			 float currentDistance = attractVector.magnitude/startDistance;
			Vector2 AttractNormalVector = attractVector.normalized;
				
			col.attachedRigidbody.AddForce(AttractNormalVector * (attractStrength * (currentDistance)));
			// col.attachedRigidbody.AddForce(AttractNormalVector * (attractStrength * (currentDistance * currentDistance)));
            }
        }
}
