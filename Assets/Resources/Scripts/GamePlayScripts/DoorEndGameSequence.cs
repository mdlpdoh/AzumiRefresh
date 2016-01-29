using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    public class DoorEndGameSequence : MonoBehaviour
    {

        public float translateTime = 0.5f;

        private Vector3 startPosition;
        public Vector3 endPosition;

        public AnimationCurve translationCurve;
        private Vector3 difference;
        // Use this for initialization
        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.StartEndGameSequence, onStartEndGameSequence);
            startPosition = transform.position;
            difference = endPosition - startPosition;
        }

        // Update is called once per frame
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
            //  print (" endPosition " + endPosition);
            transform.position = endPosition;
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(AzumiEventType.StartEndGameSequence);
        }
    }
}