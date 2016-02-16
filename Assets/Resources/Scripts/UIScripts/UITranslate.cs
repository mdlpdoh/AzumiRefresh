using UnityEngine;
using System.Collections;



namespace  com.dogonahorse
{
    public class UITranslate : MonoBehaviour
    {

        public float translateTime = 0.5f;
        public Vector3 startPosition;
        public Vector3 endPosition;
        public AzumiEventType eventType;
        public AnimationCurve translationCurve;

        private RectTransform rectTransform;

        private Vector3 difference;
        void OnEnable()
        {

            rectTransform = GetComponent<RectTransform>();
            EventManager.ListenForEvent(eventType, StartTranslation);
            difference = endPosition - startPosition;
            
            if (startPosition.x == 777){
                startPosition = transform.localPosition;
                
            }
          if (endPosition.x == 777){
                endPosition = transform.localPosition;
                
            }
        }

        public void StartTranslation(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {

            if (gameObject.activeSelf == true) {
              StartCoroutine("Translate");
            }
        }

        private IEnumerator Translate()
        {
            float currentTime = 0f;

            while (currentTime < translateTime)
            {
                float normalizedTime = currentTime / translateTime;
                float curveProgress = translationCurve.Evaluate(normalizedTime);

                rectTransform.localPosition = startPosition + (difference * curveProgress);


                currentTime += Time.unscaledDeltaTime;

                yield return null;
            }
          //  print (" endPosition " + endPosition);
            rectTransform.localPosition = endPosition;
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(eventType);
        }
    }
}
