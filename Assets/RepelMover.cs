using UnityEngine;
using System.Collections;

public class RepelMover : MonoBehaviour
{

    // Use this for initialization
    public float RepelStrength = 0.1f;

    // Update is called once per frame
    public float startDistance = 0f;
    public Rigidbody2D particleRotator;

    public float rotatorVelocity = 150;
    
    public float rotatorForce = 100;
      public bool rotationIsPositive = true;
      
     public float rotationInterval = 0.5f;
     
     public ConstantForce2D particleRotatorForce;
     
    void Start()
    {
           particleRotatorForce.torque = -rotatorForce* 0.7f;
          InvokeRepeating("switchDirection",rotationInterval, rotationInterval);
          //particleRotatorForce.torque = -rotatorForce/2;
       // particleRotator.angularVelocity = rotatorVelocity;
       
    }
        void Update()
    {
        particleRotatorForce.torque = Mathf.Clamp(particleRotatorForce.torque,-rotatorForce,rotatorForce);
        particleRotator.angularVelocity = Mathf.Clamp(particleRotatorForce.torque,-rotatorVelocity,rotatorVelocity);;
    }
    void switchDirection() {
        if (rotationIsPositive){
            particleRotatorForce.torque = rotatorForce;
           //particleRotator.angularVelocity = 0;
        } else {
             // particleRotator.AddTorque(-rotatorForce);    
              particleRotatorForce.torque = -rotatorForce;
                 //particleRotator.angularVelocity = 0;
        }
        rotationIsPositive = !rotationIsPositive;
    }
    void Example() {
      
    }
    
    
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Ball")
        {
            startDistance = (col.transform.position - transform.position).magnitude;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Ball")
        {
            Vector2 repelVector = (col.transform.position - transform.position);

            float currentDistance = repelVector.magnitude / startDistance;
            Vector2 repelNormalVector = repelVector.normalized;

            col.attachedRigidbody.AddForce(repelNormalVector * (RepelStrength * (currentDistance)));
            // col.attachedRigidbody.AddForce(AttractNormalVector * (attractStrength * (currentDistance * currentDistance)));
        }
    }
}
