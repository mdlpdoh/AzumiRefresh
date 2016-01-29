using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace com.dogonahorse
{
    public class UILevelResultsStar : MonoBehaviour
    {

        public AnimationCurve winCurve;
        public float showTime = 0.5f;
        public AnimationCurve failCurve;
        public float startScale;
        private float endScale;

        private Image winStar;
        private Image failStar;

        public float delay = 0;

        public bool StarIsWinner = false;

        void Start()
        {
            endScale = transform.localScale.x;
            winStar = transform.Find("WinStar").GetComponent<Image>();
            failStar = transform.Find("FailStar").GetComponent<Image>();
            winStar.enabled = false;
            failStar.enabled = false;
        }

        public void ShowWinStar()
        {
            print("ShowWinStar " + transform.name);
            StarIsWinner = true;
            winStar.enabled = true;
            transform.localScale = new Vector3(startScale, startScale, startScale);
            Invoker.InvokeDelayed(ShowStar, delay);
        }

        public void ShowFailStar()
        {
            print("ShowFailStar " + transform.name);
            StarIsWinner = false;
            failStar.enabled = true;
            transform.localScale = new Vector3(startScale, startScale, startScale);
            Invoker.InvokeDelayed(ShowStar, delay);
        }
        public void ShowStar()
        {
            StartCoroutine("ShowStarAnimation");
        }

        private IEnumerator ShowStarAnimation()
        {
            float currentTime = 0f;

            while (currentTime < showTime)
            {
                float curveProgress;
                float normalizedTime = currentTime / showTime;
                if (StarIsWinner)
                {
                    curveProgress = winCurve.Evaluate(normalizedTime);

                }
                else
                {
                    curveProgress = failCurve.Evaluate(normalizedTime);

                }
                float scaleFactor = startScale + (endScale - startScale) * curveProgress;
                transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
            transform.localScale = new Vector3(endScale, endScale, endScale);
        }


    }
}