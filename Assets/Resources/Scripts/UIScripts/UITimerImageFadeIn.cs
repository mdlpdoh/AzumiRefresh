using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace com.dogonahorse
{
    public class UITimerImageFadeIn : MonoBehaviour
    {
        public enum FadeDirection
        {
            FadeIn,
            FadeOut
        }
        public float fadeTime = 0.5f;

        public float MaxOpacity = 1.0f;
        public FadeDirection fadeDirection;
        public AzumiEventType eventType;
        private Image myImage;
        public AnimationCurve fadeCurve;

        private bool fadeAlreadyStarted = false;

        // Use this for initialization
        void Start()
        {
            myImage = GetComponent<Image>();
            EventManager.ListenForEvent(eventType, StartFade);
            EventManager.ListenForEvent(AzumiEventType.CancelTimer, OnResetTimer);
        }

        public void StartFade(AzumiEventType Event_Type, Component Sender, object Param = null)
        {

            if (fadeAlreadyStarted == false && fadeDirection == FadeDirection.FadeIn)
            {
                StartCoroutine("FadeIn");
                fadeAlreadyStarted = true;
            }

        }
        public void OnResetTimer(AzumiEventType Event_Type, Component Sender, object Param = null)
        {

            StopAllCoroutines();
            Color color = myImage.color;
            color.a = 0;
            myImage.color = color;
            fadeAlreadyStarted = false;
        }
        private IEnumerator FadeIn()
        {
            float currentTime = 0f;
            Color color;

            while (currentTime < fadeTime)
            {
                float normalizedTime = currentTime / fadeTime;
                float curveProgress = fadeCurve.Evaluate(normalizedTime);

                color = myImage.color;
                color.a = curveProgress * MaxOpacity;
                myImage.color = color;
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            color = myImage.color;
            color.a = MaxOpacity;
            myImage.color = color;
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(eventType);
            EventManager.Instance.RemoveEvent(eventType);
        }
    }
}