using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogOnaHorse
{


	public class LevelResultsModalWindow : ModalWindow
	{

		public string victoryMessage1 = "You freed the\n@A\nwith @B\nbounces left!";
		public string victoryMessage2 = "You also got\n@C Coins!";
		public string failureMessage1 = "Oh No\n You weren't able to free the @A!";
		public string failureMessage2 = "\nBut at least you got @C coins!";

		private ScoreManager scoreManager;



	
		override public void InitWindow() {
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
			int numberOfStars = scoreManager.NumberOfStars;
			int numberOfBounces = scoreManager.numberOfBounces;
			int numberOfCoins  = scoreManager.numberOfCoins;
			string typeOfAnimal = scoreManager.TypeOfAnimal;;
			string resultsMessage;

			if (numberOfStars < 1) {
				//Players lost
				resultsMessage = ParseMessageString(failureMessage1,numberOfBounces,numberOfCoins,typeOfAnimal);
				if (numberOfCoins > 0) {
					resultsMessage += "\n" + ParseMessageString(failureMessage2,numberOfBounces,numberOfCoins,typeOfAnimal); 
				}
			} else {
				//Players won
				resultsMessage = ParseMessageString(victoryMessage1,numberOfBounces,numberOfCoins,typeOfAnimal);
				if (numberOfCoins > 0) {
					resultsMessage += "\n" + ParseMessageString(victoryMessage2,numberOfBounces,numberOfCoins,typeOfAnimal); 
				}
			}
			Text messageText = GetComponentInChildren<Text>();
		//	print (messageText);
			messageText.text = resultsMessage;
			
		}

		string ParseMessageString (string rawMessage, int numberOfBounces, int numberOfCoins, string typeOfAnimal) {
			rawMessage = rawMessage.Replace("@A", typeOfAnimal); 
			rawMessage = rawMessage.Replace("@B", numberOfBounces.ToString()); 
			rawMessage = rawMessage.Replace("@C", numberOfCoins.ToString ()); 
			return rawMessage ;
		}
		/*
		public  WindowState myWindowState = WindowState.closed;
		public ButtonID buttonID;

		// Use this for initialization
		void Awake ()
		{
			implementCurrentWindowState ();
		}

		void implementCurrentWindowState ()
		{
			if (myWindowState == WindowState.closed) {
				gameObject.SetActive (false);
			} else {
				gameObject.SetActive (true);
			}
		}


		public void DoButtonAction (ButtonAction buttonAction)
		{
			switch (buttonAction) {
			case ButtonAction.OpenModal:
				Open ();
				break;
			case ButtonAction.CloseModal:
				Close ();
				break;
			 default:
				break;
			
			}
		}


		public void Open ()
		{
			myWindowState = WindowState.open;
			implementCurrentWindowState ();
		}

		public void Close ()
		{

			myWindowState = WindowState.closed;
			implementCurrentWindowState ();
		}
			*/
	}
}
