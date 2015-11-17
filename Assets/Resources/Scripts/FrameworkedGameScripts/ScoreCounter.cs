using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
	public class ScoreCounter : MonoBehaviour
	{
		private Text myText;

		void Awake ()
		{
			myText = GetComponent<Text>();
		}
		void Start ()
		{
			EventManager.ListenForEvent (AzumiEventType.SetBounces, OnSetBouncesEvent);

		}
		public void SetStartingAmount(int startingAmount){
			myText.text = startingAmount.ToString();
		}
		void OnSetBouncesEvent (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			myText.text = Param.ToString();
		}
	}
}
