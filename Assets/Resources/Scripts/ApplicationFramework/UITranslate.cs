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
            
            if (startPosition.x == 777){
                startPosition = transform.localPosition;
                
            }
          if (endPosition.x == 777){
                endPosition = transform.localPosition;
                
            }


        }

        // Update is called once per frame
        public void StartTranslation(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
       // print("Event_Type " + Event_Type + "  " + this.gameObject.name);

            
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


                currentTime += Time.unscaledDeltaTime;


                yield return null;
            }
            print (" endPosition " + endPosition);
            rectTransform.localPosition = endPosition;
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(eventType);
        }
    }
}
