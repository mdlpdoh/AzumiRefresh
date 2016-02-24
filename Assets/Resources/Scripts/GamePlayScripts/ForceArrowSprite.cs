using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    /// <summary>
    /// This script is on the directionalMoverIntArrow sprite game object that is the child 
    /// of the ForceArrowRotater game object which is a child of the Ball game object.
    /// </summary>
    public class ForceArrowSprite : MonoBehaviour
    {
        public float fadeInTime = 0.3f;
        public AnimationCurve fadeInCurve;
        public float BasefadeOutTime = 0.1f;
        private float adjustedfadeOutTime = 1;
        public AnimationCurve fadeOutCurve;
        public float MaxOpacity = 1.0f;
        private SpriteRenderer mySprite;

        void Start()
        {
            mySprite = GetComponent<SpriteRenderer>();
            Color color = mySprite.color;
            color.a = 0;
            mySprite.color = color;
        }
        public void StartFadeIn()
        {
            StopAllCoroutines();
            StartCoroutine("FadeIn");
        }
        public void AdjustFadeoutSpeed(float fadeOutSpeedAdjustment)
        {
           adjustedfadeOutTime = BasefadeOutTime * (1-fadeOutSpeedAdjustment/2);
        }
        public void StopAllActivity()
        {
            StopAllCoroutines();
            Color color = mySprite.color;
            color.a = 0;
            mySprite.color = color;
        }
        public void StartFadeOut()
        {
            StopAllCoroutines();
            StartCoroutine("FadeOut");
        }
        private IEnumerator FadeIn()
        {
            float currentTime = 0f;
            Color color;
            while (currentTime < fadeInTime)
            {
                float normalizedTime = currentTime / fadeInTime;
                float curveProgress = fadeInCurve.Evaluate(normalizedTime);
                color = mySprite.color;
                color.a = curveProgress * MaxOpacity;

                mySprite.color = color;
                currentTime += Time.deltaTime;
                yield return null;
            }
            
            color = mySprite.color;
            color.a = MaxOpacity;
            mySprite.color = color;
            StartFadeOut();
        }
        private IEnumerator FadeOut()
        {
            float currentTime = 0f;
            Color color;
            while (currentTime < adjustedfadeOutTime)
            {
                float normalizedTime = currentTime / adjustedfadeOutTime;
                float curveProgress = fadeOutCurve.Evaluate(normalizedTime);
                color = mySprite.color;
                color.a = MaxOpacity - curveProgress * MaxOpacity;
                mySprite.color = color;
                currentTime += Time.deltaTime;
                yield return null;
            }
            color = mySprite.color;
            color.a = 0;
            mySprite.color = color;
        }
    }//end class
}//end namespace