using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{
    public class BallEndGameSequence : MonoBehaviour
    {


        public float translateTime1 = 0.5f;

        private Vector3 startPosition1;
        public Vector3 endPosition1;

        public AnimationCurve translationCurve1;
        private Vector3 difference1;

        public float translateTime2 = 0.5f;

        private Vector3 startPosition2;
        public Vector3 endPosition2;

        public AnimationCurve translationCurve2;
        private Vector3 difference2;

        public float pauseTime = 0.5f;
        private Rigidbody2D rb;
        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            EventManager.ListenForEvent(AzumiEventType.StartEndGameSequence, onStartEndGameSequence);
        }

        void Init()
        {
            startPosition1 = transform.position;
            difference1 = endPosition1 - startPosition1;
            startPosition2 = endPosition1;
            difference2 = endPosition2 - startPosition2;
        }

        // Update is called once per frame
        void onStartEndGameSequence(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            rb.isKinematic = true;
            Init();
            StartCoroutine("Translate1");
        }
        private IEnumerator Translate1()
        {
            float currentTime = 0f;

            while (currentTime < translateTime1)
            {
                float normalizedTime = currentTime / translateTime1;
                float curveProgress = translationCurve1.Evaluate(normalizedTime);
                transform.position = startPosition1 + (difference1 * curveProgress);
                currentTime += Time.deltaTime;
                yield return null;
            }
            //  print (" endPosition " + endPosition);
            transform.position = endPosition1;
            StartCoroutine("Translate2");
        }
        private IEnumerator Translate2()
        {
            float currentTime = 0f;

            while (currentTime < translateTime2)
            {
                float normalizedTime = currentTime / translateTime2;
                float curveProgress = translationCurve2.Evaluate(normalizedTime);
                transform.position = startPosition2 + (difference2 * curveProgress);
                currentTime += Time.deltaTime;
                yield return null;
            }
            //  print (" endPosition " + endPosition);
            transform.position = endPosition2;
            Invoke("EndSequence", pauseTime);
        }
         
        
         void EndSequence()
        {
            EventManager.PostEvent(AzumiEventType.FinishEndGameSequence, this);
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(AzumiEventType.StartEndGameSequence);
        }
    }
}
