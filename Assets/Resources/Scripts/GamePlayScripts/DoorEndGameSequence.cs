using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    /// <summary>
    /// This script lives on the left and right doors game objects. It listens for the AzumiEventType.StartEndGameSequence
    /// to start the doors opening animation sequence.
    /// </summary>
    public class DoorEndGameSequence : MonoBehaviour
    {

        public float translateTime = 0.5f;
        private Vector3 startPosition;
        public Vector3 endPosition;
        public AnimationCurve translationCurve;
        private Vector3 difference;
        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.StartEndGameSequence, onStartEndGameSequence);
            startPosition = transform.position;
            difference = endPosition - startPosition;
        }

        void onStartEndGameSequence(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            StartCoroutine("Translate");
        }

        private IEnumerator Translate()
        {
            float currentTime = 0f;

            while (currentTime < translateTime)
            {
                float normalizedTime = currentTime / translateTime;
                float curveProgress = translationCurve.Evaluate(normalizedTime);
                transform.position = startPosition + (difference * curveProgress);
                currentTime += Time.deltaTime;
                yield return null;
            }
            // print (" endPosition " + endPosition);
            transform.position = endPosition;
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(AzumiEventType.StartEndGameSequence);
        }
    }//end class
}//end namespace