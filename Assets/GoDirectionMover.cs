using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    public class GoDirectionMover : MonoBehaviour
    {
        public Transform DirectionalArrow;
        public Transform MoverIcon;
        public float transitionAmountPerFrame = 0.001f;

        private float currentAmount = 0f;

        private Vector2 TargetVector;

        void Start()
        {
            DirectionalArrow.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            TargetVector= Vector3.Normalize(DirectionalArrow.position - transform.position);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name == "Ball")
            {
                 //set starting value
                currentAmount = transitionAmountPerFrame;
                AdjustVelocity(col);
            }
        }


        void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.name == "Ball")
            {
                AdjustVelocity(col);
            }
        }
        
        void AdjustVelocity(Collider2D col)
        {
            //get existing ball velocity
            Vector2 CurrentVelocity = col.GetComponent<Rigidbody2D>().velocity ;
            //get desired target velocity (direction of mover * Maximum possible for ball)
            Vector2 TargetVelocity = (col.gameObject.GetComponent<AzumiBallRoll>().MaximumVelocity * TargetVector);
             //increment current amount, clamping to range between 0 and 1)
	        currentAmount = Mathf.Clamp(currentAmount+currentAmount,0,1);
            //set new velocity-reduce current vecror, increase target vector
            col.GetComponent<Rigidbody2D>().velocity = col.GetComponent<Rigidbody2D>().velocity - (CurrentVelocity * currentAmount) + (TargetVelocity * currentAmount);
        }
        void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, DirectionalArrow.position, Color.red);
            Vector2 vectorDirection = Vector3.Normalize(DirectionalArrow.position - transform.position);
            float angle = Vector3.Angle(vectorDirection, Vector2.up);
            if (DirectionalArrow.position.x < transform.position.x)
            {
                DirectionalArrow.rotation = Quaternion.Euler(0, 0, angle + 90);
                MoverIcon.rotation = Quaternion.Euler(0, 0, angle + 90);
            }
            else
            {
                DirectionalArrow.rotation = Quaternion.Euler(0, 0, -angle + 90);
                MoverIcon.rotation = Quaternion.Euler(0, 0, -angle + 90);
            }
            MoverIcon.localPosition = new Vector2(0, 0);
        }
    }
}
