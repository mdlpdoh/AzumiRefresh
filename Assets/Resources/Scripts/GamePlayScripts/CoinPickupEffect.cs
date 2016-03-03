using UnityEngine;
using System.Collections;



namespace com.dogonahorse
{
    /// <summary>
    /// This script lives on the coin game object and listens for AzumiEventType.HitCollectible to initiate the nice twirling coin animation.
    /// </summary>
    public class CoinPickupEffect : MonoBehaviour
    {
        public float effectTime = 0.3f;
        
        public float MaxScaleAmount = 1.5f;
        
        private Vector3 angleAxis;

        private Vector3 defaultScale;
        
        public AnimationCurve scaleCurve;

        public AnimationCurve rotationCurve;
        
        public float MaxRotationAmount = 10f;
        
        private Quaternion rotationDirection;

        private Collider2D currentCollider;
        
        private Collectible collectible;
        
        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.HitCollectible, OnCoinPickup);
            collectible = GetComponent<Collectible>();
            defaultScale = transform.localScale;
        }

        void OnCoinPickup(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            if (Sender == collectible)
            {
                Collider2D currentCollider = (Collider2D)Param;
                Vector3 currentDirection = currentCollider.attachedRigidbody.velocity.normalized;

                angleAxis = Vector3.Cross(currentDirection, Vector3.forward).normalized;
             
                StartPickupEffect();
            }
        }
        void StartPickupEffect()
        {
            StartCoroutine("RotateEffect");
            StartCoroutine("ScaleEffect");
        }

        private IEnumerator RotateEffect()
        {
            float currentTime = 0f;

            while (currentTime < effectTime)
            {
                float normalizedTime = currentTime / effectTime;
                float curveProgress = rotationCurve.Evaluate(normalizedTime);
                transform.rotation = Quaternion.AngleAxis(curveProgress * MaxRotationAmount, angleAxis);
                // print ("transform.rotation " +  transform.rotation.eulerAngles);
                currentTime += Time.deltaTime;
                
                yield return null;
                
            }// end while
        }//end Ienumerator RotateEffect

        private IEnumerator ScaleEffect()
        {
            float currentTime = 0f;

            while (currentTime < effectTime)
            {
                float normalizedTime = currentTime / effectTime;
                float curveProgress = scaleCurve.Evaluate(normalizedTime);

                transform.localScale = defaultScale + new Vector3(curveProgress * MaxScaleAmount , curveProgress * MaxScaleAmount, curveProgress * MaxScaleAmount);

                currentTime += Time.deltaTime;

                yield return null;
            }

            GameObject.Destroy(gameObject);
 
        }//end Ienumerator ScaleEffect
    }//end class
}//end namespace
