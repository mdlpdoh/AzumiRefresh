using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{


	public class LevelResultsModalWindow : ModalWindow
	{

		private string victoryMessage1 = "You freed the\n@A\nwith @B\nenergy left!";
		private string victoryMessage2 = "You also got\n@C Ants!";
		private string failureMessage1 = "The ants got you!";
		private string failureMessage2 = "You didn't free the\n@A!\nBut you got\n @C coins!";
		private string failureMessage3 = "Aargh!\nOut of energy!\nThe animals need you!";

		private ScoreManager scoreManager;



	
		override public void InitWindow() {
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
			int numberOfStars = scoreManager.NumberOfStars;
			int numberOfSwipes = scoreManager.numberOfBounces;
			int numberOfCoins  = scoreManager.CoinsEarned;
			string typeOfAnimal = scoreManager.ChapterAnimalName;
			string resultsMessage;


			//Players ran out of swipes - No coins, No nothin', Try again!
			if (numberOfSwipes <= 0 && numberOfStars == 0) {
				resultsMessage = ParseMessageString(failureMessage3,numberOfSwipes,numberOfCoins,typeOfAnimal);
			}
			else if (numberOfStars < 1) {
				//Players lost
				resultsMessage = ParseMessageString(failureMessage1,numberOfSwipes,numberOfCoins,typeOfAnimal);
				if (numberOfCoins > 0) {
					resultsMessage += "\n" + ParseMessageString(failureMessage2,numberOfSwipes,numberOfCoins,typeOfAnimal); 
				}
			} else {
				//Players won
				resultsMessage = ParseMessageString(victoryMessage1,numberOfSwipes,numberOfCoins,typeOfAnimal);
				if (numberOfCoins > 0) {
					resultsMessage += "\n" + ParseMessageString(victoryMessage2,numberOfSwipes,numberOfCoins,typeOfAnimal); 
				}
			}
			Text messageText = GetComponentInChildren<Text>();
		//	print (messageText);
			messageText.text = resultsMessage;
			SetStars(numberOfStars);
		}

		void SetStars (int numberOfStars) {
	
			if (numberOfStars > 2) {
				transform.Find("Star3").GetComponent<Image>().color = Color.red;
				transform.Find("Star2").GetComponent<Image>().color = Color.red;
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
			} else if (numberOfStars > 1){
				transform.Find("Star2").GetComponent<Image>().color = Color.red;
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
			} else if (numberOfStars == 0){
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
			}
		}


		string ParseMessageString (string rawMessage, int numberOfSwipes, int numberOfCoins, string typeOfAnimal) {
			rawMessage = rawMessage.Replace("@A", typeOfAnimal); 
			rawMessage = rawMessage.Replace("@B", numberOfSwipes.ToString()); 
			rawMessage = rawMessage.Replace("@C", numberOfCoins.ToString ()); 
			return rawMessage ;
		}
		
	}
}
