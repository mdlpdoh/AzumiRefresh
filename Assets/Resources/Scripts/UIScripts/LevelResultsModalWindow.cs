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
		private string failureMessage1 = "Aargh!\nOut of energy and you didn't eat\nenough ants! Try again!"; // You lost, did not eat enuf ants and ran out of swipes.
		private string failureMessage2 = "Oh No!\nYou ran out of energy and\ndid not free the@A!\nTry again!"; // You lost, got the min amt of ants but you ran out of swipes. 
		private string failureMessage3 = "The ants got you!\nYou didn't eat enough of them! Try again!"; // You lost, did not eat min amt of ants but you still had swipes left.

		private ScoreManager scoreManager;
		public Material failPandaMat;
	
		override public void InitWindow() {
			
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
			int correctWinOrLoseMessage = scoreManager.WinAndLoseMessageInfo;
			int numberOfSwipes = scoreManager.numberOfBounces;
			int numberOfCoins  = scoreManager.CoinsEarned;
			int totalScore = scoreManager.TotalScore;
			string typeOfAnimal = scoreManager.ChapterAnimalName;
			string resultsMessage;

			if (correctWinOrLoseMessage == 0) 
			{
				// 0 stars. You lost, did not eat min amt of ants but you still had swipes left.
				resultsMessage = ParseMessageString (failureMessage3, numberOfSwipes, numberOfCoins, totalScore, typeOfAnimal);
			} 
			else if (correctWinOrLoseMessage == 5) 
			{
				// 0 stars. You lost, did not eat enuf ants and ran out of swipes.
				resultsMessage = ParseMessageString(failureMessage1,numberOfSwipes,numberOfCoins,totalScore,typeOfAnimal);
			} 
			else if (correctWinOrLoseMessage == 4) 
			{
				// 0 stars. You lost, got the min amt of ants but you ran out of swipes. 
				resultsMessage = ParseMessageString(failureMessage2,numberOfSwipes,numberOfCoins,totalScore,typeOfAnimal);
			} 

			else if (correctWinOrLoseMessage > 0) 
			{
				// 1 or more stars. You got min amt of ants in amt of swipes given.
				resultsMessage = ParseMessageString(victoryMessage1,numberOfSwipes,numberOfCoins,totalScore,typeOfAnimal);
			} 
			else
			{
				//Players got out with 0 or more energy and the min amt of ants or more - 1 or more stars.
				resultsMessage = ParseMessageString(victoryMessage1,numberOfSwipes,numberOfCoins,totalScore,typeOfAnimal);	
			}


			//Sends the various text information about score and swipes to the modal 
			Transform[] childTransforms = GetComponentsInChildren<Transform>();
			for (int i = 0; i < childTransforms.Length; i++) {
				if (childTransforms[i].name == "TitleText")
				{
					Text messageText = childTransforms[i].GetComponent<Text>();
					messageText.text = resultsMessage;
					SetStars(correctWinOrLoseMessage);
				}
				if (childTransforms [i].name == "FinalScoreTextNumeral") 
				{
					Text messageText = childTransforms[i].GetComponent<Text>();
					messageText.text = totalScore.ToString();
				}
	
			}//end for loop
		}//end method

		void SetStars (int correctWinOrLoseMessage) {
			
			if (correctWinOrLoseMessage == 0 || correctWinOrLoseMessage == 4 || correctWinOrLoseMessage == 5)
			{
				//It will give 3 broken grey stars
				transform.Find("BrokenStar1").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
			}
			else if (correctWinOrLoseMessage == 3) 
			{
				transform.Find("Star3").GetComponent<Image>().color = Color.red;
				transform.Find("Star2").GetComponent<Image>().color = Color.red;
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
			} 
			else if (correctWinOrLoseMessage == 2)
			{
				transform.Find("Star2").GetComponent<Image>().color = Color.red;
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
				transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
			} 
			else if (correctWinOrLoseMessage == 1)
			{
				transform.Find("Star1").GetComponent<Image>().color = Color.red;
				transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;

			} 
			else 
			{
				//It will give 3 broken grey stars
				transform.Find("BrokenStar1").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar2").GetComponent<Image>().enabled = true;
				transform.Find("BrokenStar3").GetComponent<Image>().enabled = true;
				// show the player the filled in red panda				
			
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
