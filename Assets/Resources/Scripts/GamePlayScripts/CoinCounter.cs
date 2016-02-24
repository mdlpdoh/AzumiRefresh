using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
    /// <summary>
	/// This script is attached to the CoinsNumber game object in the CoinBGpanel to show how many ants player has eaten.
	/// </summary>
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
        
	}// end class
}//end namespace
