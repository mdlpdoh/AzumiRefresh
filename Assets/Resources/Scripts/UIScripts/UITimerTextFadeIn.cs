using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace com.dogonahorse
{
	public class UITimerTextFadeIn : MonoBehaviour
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
		private Text myText;
		public AnimationCurve fadeCurve;

		private bool fadeAlreadyStarted = false;

		void Start()
		{
			myText = GetComponent<Text>();
			EventManager.ListenForEvent(eventType, StartFade);
		}

		public void StartFade(AzumiEventType Event_Type, Component Sender, object Param = null)
		{

			if (fadeAlreadyStarted == false && fadeDirection == FadeDirection.FadeIn)
			{
				StartCoroutine("FadeIn");
				fadeAlreadyStarted = true;
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

				color = myText.color;
				color.a = curveProgress * MaxOpacity;
				myText.color = color;
				currentTime += Time.unscaledDeltaTime;
				yield return null;
			}

			color = myText.color;
			color.a = MaxOpacity;
			myText.color = color;
		}

		void OnDestroy()
		{

			EventManager.Instance.RemoveEvent(eventType);
		}
	}
}