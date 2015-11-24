using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    public class GoDirectionMover2 : MonoBehaviour
    {
        public Transform DirectionalArrow;
        public Transform MoverIcon;
        public float TransitionAmountPerFrame = 0.001f;

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
                currentAmount = TransitionAmountPerFrame;
           
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
            Vector2 CurrentVelocity = col.GetComponent<Rigidbody2D>().velocity ;
            Vector2 TargetVelocity = (col.gameObject.GetComponent<AzumiBallRoll>().MaximumVelocity * TargetVector);
	        currentAmount = Mathf.Clamp(currentAmount+currentAmount,0,1);
			//Vector2 differenceVector = col.transform.position - transform.position;
            
			//float influence = Mathf.Clamp(Mathf.Pow(differenceVector.magnitude * multiplier, -multiplier)+ Mathf.Pow(differenceVector.magnitude * multiplier, -multiplier), 0,1);
          //  Vector2 differenceVector = col.transform.position - transform.position;
            //float currentDistanceValue = 1 - (differenceVector.magnitude/ContactDistance);
         

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
