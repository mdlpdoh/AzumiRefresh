using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{
    public class RepelMover : MonoBehaviour
    {

        // Use this for initialization
        public float RepelStrength = 0.1f;

        // Update is called once per frame
        public float startDistance = 0f;
        public Transform particleWiggler;
        public Transform rotator;
        float rotIndex = 0;

        float amplitudeX = 5.0f;

        float omegaX = 20.0f;

        float index;

        public void Update()
        {
            index += Time.deltaTime;
            float z = amplitudeX * Mathf.Cos(omegaX * index);
            // float y = Mathf.Abs (amplitudeY*Mathf.Sin (omegaY*index));
            particleWiggler.localRotation = Quaternion.Euler(0, 0, z);
            rotIndex -= 0.3f;
            rotator.rotation = Quaternion.Euler(0, 0, rotIndex);
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
                EventManager.PostEvent(AzumiEventType.inRepeller, this);
                Vector2 repelVector = (col.transform.position - transform.position);

                float currentDistance = repelVector.magnitude / startDistance;
                Vector2 repelNormalVector = repelVector.normalized;

                col.attachedRigidbody.AddForce(repelNormalVector * (RepelStrength * (currentDistance)));

            }
        }
    }
}