using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
	public class CoinCounter : MonoBehaviour
	{
		private Text myText;
	

		void Awake ()
		{
			myText = GetComponent<Text>();
		}


		void Start ()
		{
			EventManager.ListenForEvent (AzumiEventType.SetCoins, OnSetCoinsEvent);
		}

		public void SetStartingAmount(int startingAmount){

			myText.text = startingAmount.ToString();
		}
		void OnSetCoinsEvent (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			myText.text = Param.ToString();
		}


	}
}
