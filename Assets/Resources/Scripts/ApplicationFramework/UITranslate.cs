using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace com.dogonahorse
{
    public class UITranslate : MonoBehaviour
    {

        public float translateTime = 0.5f;
        public Vector3 startPosition;
        public Vector3 endPosition;
        public AzumiEventType eventType;
        public AnimationCurve translationCurve;
        // Use this for initialization
        private RectTransform rectTransform;

        private Vector3 difference;
        void OnEnable()
        {

            rectTransform = GetComponent<RectTransform>();

            EventManager.ListenForEvent(eventType, StartTranslation);
            difference = endPosition - startPosition;

        }

        // Update is called once per frame
        public void StartTranslation(AzumiEventType Event_Type, Component Sender, object Param = null)
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

                rectTransform.localPosition = startPosition + (difference * curveProgress);


                currentTime += Time.deltaTime;


                yield return null;
            }

            rectTransform.localPosition = endPosition;
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(eventType);
        }
    }
}
