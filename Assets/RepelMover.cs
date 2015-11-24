using UnityEngine;
using System.Collections;

public class RepelMover : MonoBehaviour {

	// Use this for initialization
	public float RepelStrength = 0.1f;
	
	// Update is called once per frame
		public float startDistance =0f;
	
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
             Vector2 repelVector = (col.transform.position-transform.position);
			 
			 float currentDistance = repelVector.magnitude/startDistance;
			Vector2 repelNormalVector = repelVector.normalized;
				
			col.attachedRigidbody.AddForce(repelNormalVector * (RepelStrength * (currentDistance)));
			// col.attachedRigidbody.AddForce(AttractNormalVector * (attractStrength * (currentDistance * currentDistance)));
            }
        }
}
