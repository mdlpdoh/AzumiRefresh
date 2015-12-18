using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
	public class ScoreCounter : MonoBehaviour
	{
		private Text myText;
		private String flashBlank;

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
			print (startingAmount);
		}

		void OnSetBouncesEvent (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			myText.text = Param.ToString();


//			print (myText.text);
			if (myText.text == "2") {
				print ("START FLASHING!");
				swipeNotifier ();

			}

		}

		void swipeNotifier()
		{
			// TODO
			// MAKE THE THE NUMBER OF SWIPES COUNTDOWN FLASH WHEN THERE ARE 4 SWIPES LEFT
		}
	}
}

