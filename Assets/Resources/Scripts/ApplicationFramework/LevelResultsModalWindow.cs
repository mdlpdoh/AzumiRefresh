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
		private string failureMessage2 = "You weren't able\nto free the\n@A! But at least\nyou got @C coins!";
		private string failureMessage3 = "Aargh!\nOut of energy!\nThe animals need you!";

		private ScoreManager scoreManager;



	
		override public void InitWindow() {
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
			int numberOfStars = scoreManager.NumberOfStars;
			int numberOfBounces = scoreManager.numberOfBounces;
			int numberOfCoins  = scoreManager.CoinsEarned;
			string typeOfAnimal = scoreManager.ChapterAnimalName;
			string resultsMessage;
				//Players ran out of swipes - No coins, No nothin', Try again!
			if (numberOfBounces < 0) {
				resultsMessage = ParseMessageString(failureMessage3,numberOfBounces,numberOfCoins,typeOfAnimal);
			}
			else if (numberOfStars < 1) {
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
			} else if (numberOfStars > 0){
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
			}
		}


		string ParseMessageString (string rawMessage, int numberOfBounces, int numberOfCoins, string typeOfAnimal) {
			rawMessage = rawMessage.Replace("@A", typeOfAnimal); 
			rawMessage = rawMessage.Replace("@B", numberOfBounces.ToString()); 
			rawMessage = rawMessage.Replace("@C", numberOfCoins.ToString ()); 
			return rawMessage ;
		}
		
	}
}
