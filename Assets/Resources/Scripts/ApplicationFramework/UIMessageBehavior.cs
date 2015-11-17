using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace com.dogonahorse
{
	public class UIMessageBehavior : MonoBehaviour
	{
		public enum TransitionStyle
		{
			Fade, 
			Countdown
		}

		public TransitionStyle entryStyle;
		public float entryTime;
		public AnimationCurve entryCurve;
		public TransitionStyle exitStyle;
		public float exitTime;
		public AnimationCurve exitCurve;
		public float holdTime;
		public bool autoPlay = true;
		public float startCount = 5;
		public int CountTime = 5;

		public delegate void UIMessageCallback ();
		public event  UIMessageCallback onEntryActionFinished;
		public event  UIMessageCallback onExitActionFinished;

		private Text myText;


		// Use this for initialization
		void Start ()
		{
			myText = GetComponent<Text> ();
			myText.enabled = false;
		}
	
		// Update is called once per frame
		public void StartEntry ()
		{	
			switch (entryStyle) {
			case TransitionStyle.Fade:
				setTextAlpha(0f);
				myText.enabled = true;
				onEntryActionFinished = finishedEntryTransiton;
				StartCoroutine(FadeIn(0f, 1f, entryTime, entryCurve));
				break;
			case TransitionStyle.Countdown:
			
				StartCoroutine(Countdown());
				myText.enabled = true;
				break;
			default:
				break;
			}
		}
		private void setTextAlpha (float newAlpha) {
			Color myTextColor = myText.color;
			myTextColor.a = newAlpha;
			myText.color = myTextColor;
		}

		private void finishedEntryTransiton () {
			if (autoPlay){
				Invoke("StartExit", holdTime);
			}
		}

		public void StartExit ()
		{
			//print ("onExitActionFinished" + onExitActionFinished);
			switch (exitStyle) {
			case TransitionStyle.Fade:
				StartCoroutine(FadeOut(1f, 0f, exitTime, exitCurve));
				break;
			case TransitionStyle.Countdown:

				StartCoroutine(Countdown());
				myText.enabled = true;
				break;
			default:
				break;
			}
		}


		private IEnumerator Countdown(){
	
			float currentTime = 0f;
			int currentCount;

			while(currentTime < CountTime -1)
			{
				float normalizedTime = currentTime/CountTime;

				currentCount = (int)Mathf.Floor(startCount -(normalizedTime * startCount));
				myText.text = currentCount.ToString();
				currentTime += Time.deltaTime;
				yield return null;
			}
			if(onEntryActionFinished!=null)
				onEntryActionFinished();  
		}
	

		private IEnumerator FadeIn(float startAlpha, float EndAlpha, float fadeTime, AnimationCurve fadeCurve){
			float currentTime = 0f;
			while(currentTime < fadeTime)
			{
				float normalizedTime = currentTime/fadeTime;
				float curveProgress = fadeCurve.Evaluate(normalizedTime);

				setTextAlpha (curveProgress);
				currentTime += Time.deltaTime;
				yield return null;
			}
			if(onEntryActionFinished!=null)
				onEntryActionFinished();  
		}
		private IEnumerator FadeOut(float startAlpha, float EndAlpha, float fadeTime, AnimationCurve fadeCurve){
			float currentTime = 0f;
			while(currentTime < fadeTime)
			{
				float normalizedTime = currentTime/fadeTime;
				float curveProgress = fadeCurve.Evaluate(normalizedTime);
				
				setTextAlpha (1-curveProgress);
				currentTime += Time.deltaTime;
				yield return null;
			}
			setTextAlpha (EndAlpha);
			if(onExitActionFinished!=null)
				onExitActionFinished();
		}
	}
}