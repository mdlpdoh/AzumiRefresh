using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{
    /// <summary>
    /// This script is attached to the Black hole type game object in game play area. Whe the ball gets too close it gets sucked into its center.
    /// </summary>
    public class AttractMover : MonoBehaviour
    {
        // Use this for initialization
        public float attractStrength = 0.1f;
        private float startDistance = 0f;
        public float AttractRadius = 0f;
        public Rigidbody2D particleRotator;

        public float rotatorVelocity = 150;
        void Start()
        {
            particleRotator.angularVelocity = rotatorVelocity;
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name == "Ball")
            {
                EventManager.PostEvent(AzumiEventType.inRepeller, this);
                startDistance = (col.transform.position - transform.position).magnitude;
            }
        }
        void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.name == "Ball")
            {
                EventManager.PostEvent(AzumiEventType.inAttractor, this);
                Vector2 attractVector = (transform.position - col.transform.position);

                float currentDistance = attractVector.magnitude / startDistance;
                Vector2 AttractNormalVector = attractVector.normalized;
                col.attachedRigidbody.AddForce(AttractNormalVector * (attractStrength * (currentDistance)));
            }
        }
    }//end class
}//end namespace