using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{

	public class LevelResultsModalWindow : ModalWindow
	{
		//victory messages
		private string victoryMessage1 = "You freed the @A\nwith @B energy left and\ngot @C Ants!";
		//failure messages
		private string failureMessage1 = "Aargh!\nOut of energy and you didn't eat\nenough ants! Try again!";
//		private string failureMessage2 = "The ants won!\nYou didn't free the\n@A!\nBut you got @C coins!";
//		private string failureMessage3 = "Out of energy!\nTry again!";
		private string failureMessage4 = "The ants got you!\nYou didn't eat enough of them! Try again!";

		private ScoreManager scoreManager;
		public Material failPanda;
	
		override public void InitWindow() {
			
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
			int numberOfStars = scoreManager.NumberOfStars;
			int numberOfSwipes = scoreManager.numberOfBounces;
			int numberOfCoins  = scoreManager.CoinsEarned;

			bool exitedSafely = scoreManager.ExitedDoorSafely;

			int totalScore = scoreManager.TotalScore;
			string typeOfAnimal = scoreManager.ChapterAnimalName;
			string resultsMessage;
/* OLD 
			//Players ran out of swipes - No coins, No nothin', Try again!
			if (numberOfStars == 0) {
				//Player lost.
				resultsMessage = ParseMessageString (failureMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
			} 
//			else if (numberOfStars == 0  && numberOfSwipes >= 0) 
//			{
//				resultsMessage = ParseMessageString (failureMessage4, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
//			}

			else if (numberOfStars > 2) 
			{
				//Players got out with 0 or more energy but only 60% or less ants (2 stars).
				resultsMessage = ParseMessageString(victoryMessage1,numberOfSwipes,numberOfCoins,totalScore,typeOfAnimal);
			} else 
			{
				//Players got out with 90% or more ants (3 stars)and with 0 or more energy.
				resultsMessage = ParseMessageString(victoryMessage1,numberOfSwipes,numberOfCoins,totalScore,typeOfAnimal);
			}

*/ 

			if (exitedSafely == false && numberOfStars == 0) { //ran out of swipes and got no coins
				resultsMessage = ParseMessageString (failureMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
			} else if (exitedSafely == true && numberOfStars == 0) { //had swipes left but did not get min amt of coins
				resultsMessage = ParseMessageString (failureMessage4, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
			} else if (exitedSafely == false && numberOfStars > 0) { //ran out of swipes but got minimum amt of coins
				resultsMessage = ParseMessageString (victoryMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
			} else 
			{
				resultsMessage = ParseMessageString (victoryMessage1, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
			}

			//Sends the various text information about score and swipes to the modal 
			Transform[] childTransforms = GetComponentsInChildren<Transform>();
			for (int i = 0; i < childTransforms.Length; i++) {
				if (childTransforms[i].name == "TitleText")
				{
					Text messageText = childTransforms[i].GetComponent<Text>();
					messageText.text = resultsMessage;
					SetStars(numberOfStars);
				}
				if (childTransforms [i].name == "FinalScoreTextNumeral") 
				{
					Text messageText = childTransforms[i].GetComponent<Text>();
					messageText.text = totalScore.ToString();
				}
			}

//			Text messageText = GetComponentInChildren<Text>();
//			//print (messageText);
//			messageText.text = resultsMessage;
//			SetStars(numberOfStars);
		}

		void SetStars (int numberOfStars) {
			
			if (numberOfStars > 2) {
				transform.Find("Star3").GetComponent<Image>().color = Color.red;
				transform.Find("Star2").GetComponent<Image>().color = Color.red;
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
			} else if (numberOfStars > 1)
			{
				transform.Find("Star2").GetComponent<Image>().color = Color.red;
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
				transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
			} else if (numberOfStars > 0)
			{
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
				transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
			} else if (numberOfStars == 0)
			{
				//It will give 3 broken grey stars
				transform.Find("BrokenStar1").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
				// show the player the filled in red panda				
			
//				Image redpanda = transform.Find ("RedPanda").GetComponent<Image> ();
		//		redpanda.color = Color.red;
		//		redpanda.material = failPanda;
			}
		}
			
		string ParseMessageString (string rawMessage, int numberOfSwipes, int numberOfCoins, int totalScore, string typeOfAnimal) {
			rawMessage = rawMessage.Replace("@A", typeOfAnimal); 
			rawMessage = rawMessage.Replace("@B", numberOfSwipes.ToString()); 
			rawMessage = rawMessage.Replace("@C", numberOfCoins.ToString ()); 
			return rawMessage ;
		}


	}// end class
}//end namespace
