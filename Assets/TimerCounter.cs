using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
	public class TimerCounter : MonoBehaviour 
	{
		private Text myText;
		public float timer = 7.0f;


		void Awake ()
		{
			myText = GetComponent<Text>();
		}

		void Start () 
		{
			EventManager.ListenForEvent(AzumiEventType.StartTimer, EndGameStartTimer);
		}

		void EndGameStartTimer(AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			StartCoroutine("StartEndTimer");

		}
		private IEnumerator StartEndTimer ()
		{
			while (true) 
			{
				if (timer > 0) 
				{
					timer -= Time.deltaTime;
					myText.text = timer.ToString ();
				} 
				else 
				{
					if (timer <= 0) 
					{
						EventManager.PostEvent (AzumiEventType.OutOfBounces, this);
					}
					timer = 0;
				}
				yield return null;
			}
		}// end ienumerator 

	}//end class
}// end namespace

