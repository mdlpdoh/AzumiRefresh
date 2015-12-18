using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace com.dogonahorse
{
    public class UIFadeInOut : MonoBehaviour
    {
        public enum FadeDirection
        {
            FadeIn,
            FadeOut
        }
        public float fadeTime = 0.5f;
        public FadeDirection fadeDirection;
        public AzumiEventType eventType;
        private Image myImage;
        public AnimationCurve fadeCurve;
        // Use this for initialization
        void Start()
        {
            myImage = GetComponent<Image>();
            EventManager.ListenForEvent(eventType, StartFade);
        }

        // Update is called once per frame
        public void StartFade(AzumiEventType Event_Type, Component Sender, object Param = null)
        {

                if (fadeDirection == FadeDirection.FadeIn)
                {
                    StartCoroutine("FadeIn");
                }
                else
                {
                    StartCoroutine("FadeOut");
                }
      
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
                color.a = curveProgress;
                print("currentTime " + currentTime);
                myImage.color = color;
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            color = myImage.color;
            color.a = 1;
            myImage.color = color;
        }
        private IEnumerator FadeOut()
        {
            float currentTime = 0f;
            while (currentTime < fadeTime)
            {
                float normalizedTime = currentTime / fadeTime;
                float curveProgress = fadeCurve.Evaluate(normalizedTime);
                Color color = myImage.color;
                color.a = 1 - curveProgress;
                myImage.color = color;
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            EventManager.PostEvent(AzumiEventType.BlurFadeOutComplete, this);
        }
        void OnDestroy()
        {

            EventManager.Instance.RemoveEvent(eventType);
        }
    }
}