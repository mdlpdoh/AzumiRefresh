using UnityEngine;

using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{
    public class UILevelButtonLock : MonoBehaviour
    {
        private Image mainlockImage;
        private Image leftLockImage;
        private Image rightLockImage;
        private ParticleSystem lockParticles;

        private RectTransform rectTransform;

        private LevelButtonBehavior parentButton;
        private float maxAlpha;
        public float BreakDelay = 1f;

        public float shiftDelay = 0.5f;
        public float shakeTime = 1f;


        public float fadeTime = 0.5f;
        public float shakeAmount = 1f;
        public AnimationCurve shakeCurve;

        public AnimationCurve breakFadeCurve;

        public AnimationCurve mainFadeCurve;
        public int numberOfParticles = 50;


        private bool openAnimationInProgress = false;
        void Awake()
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].name == "Lock")
                {
                    mainlockImage = childTransforms[i].GetComponent<Image>();
                }
                else if (childTransforms[i].name == "breakLeft")
                {
                    leftLockImage = childTransforms[i].GetComponent<Image>();
                }
                else if (childTransforms[i].name == "breakRight")
                {
                    rightLockImage = childTransforms[i].GetComponent<Image>();
                }
            }
            lockParticles = GetComponent<ParticleSystem>();
            rectTransform = GetComponent<RectTransform>();
            parentButton = transform.parent.GetComponent<LevelButtonBehavior>();
            leftLockImage.enabled = false;
            rightLockImage.enabled = false;
            maxAlpha = mainlockImage.color.a;
        }


        public void OnOpenModal(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            openAnimationInProgress = false;
            EventManager.Instance.RemoveListener(AzumiEventType.OpenModal, OnOpenModal);
            lockParticles.Clear();
        }

        public void PrepareToBreakOpen()
        {
            openAnimationInProgress = true;
            EventManager.ListenForEvent(AzumiEventType.OpenModal, OnOpenModal);
            Invoke("breakOpen", BreakDelay);
        }
        public void PrepareToBreakOpenAfterShift()
        {
            openAnimationInProgress = true;
            EventManager.ListenForEvent(AzumiEventType.OpenModal, OnOpenModal);
            Invoke("breakOpen", BreakDelay + shiftDelay);
        }

        public void breakOpen()
        {

            if (openAnimationInProgress)
            {

                lockParticles.Emit(numberOfParticles);
                StartCoroutine("Shake");
                EventManager.PostEvent(AzumiEventType.UnLockLevel, this);
            }
            else
            {
                parentButton.Unlock();
            }
        }

        private IEnumerator Shake()
        {
            Color color;
            float currentTime = 0f;

            leftLockImage.enabled = true;
            rightLockImage.enabled = true;

            while (currentTime < shakeTime)
            {
                float normalizedTime = currentTime / shakeTime;
                float curveProgress = shakeCurve.Evaluate(normalizedTime);
                float breakAlphaProgress = breakFadeCurve.Evaluate(normalizedTime);
                float mainAlphaProgress = mainFadeCurve.Evaluate(normalizedTime);
                rectTransform.anchoredPosition = new Vector2(curveProgress * shakeAmount, rectTransform.anchoredPosition.y);
                color = mainlockImage.color;
                color.a = mainAlphaProgress * maxAlpha;
                mainlockImage.color = color;


                color = rightLockImage.color;
                color.a = breakAlphaProgress * maxAlpha;

                leftLockImage.color = color;
                rightLockImage.color = color;
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = new Vector2(0, rectTransform.anchoredPosition.y);
            leftLockImage.enabled = false;
            rightLockImage.enabled = false;
            EventManager.Instance.RemoveListener(AzumiEventType.OpenModal, OnOpenModal);
            openAnimationInProgress = false;
            parentButton.Unlock();
        }


        // Use this for initialization
        public void Show()
        {
            mainlockImage.enabled = true;
        }

        public void Hide()
        {
            mainlockImage.enabled = false;
        }
    }
}