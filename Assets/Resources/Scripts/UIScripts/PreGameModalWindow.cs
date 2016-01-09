using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{


	public class PreGameModalWindow : MonoBehaviour
	{

		private string beginLevelMessage = "You have @B swipes \nand @C ants to eat \nto free the captive\n@A!";

		public Text titleText;
		public Text AnimalNameText;
		
		public Text ChapterNumberText;
		public Text LevelNumberText;
		

		void OnEnable(){
			InitWindow();
		}

		public void InitWindow() {
			
			int nextChapter = SceneManager.NextChapter;
			int nextLevel = SceneManager.NextLevel;
			//print(" nextChapter  " + nextChapter + "  nextLevel  " + nextLevel);
			int numberOfBounces = LevelManager.GetMaxTaps(nextChapter, nextLevel);
			int numberOfCoins  = LevelManager.GetCoinsInLevel(nextChapter, nextLevel);
			string typeOfAnimal = LevelManager.GetChapterAnimalName(nextChapter, nextLevel);
			string resultsMessage;

			resultsMessage = ParseMessageString(beginLevelMessage,numberOfBounces,numberOfCoins,typeOfAnimal);

			//Text messageText = titleText;
			AnimalNameText.text = typeOfAnimal;
			ChapterNumberText.text = nextChapter.ToString();
			LevelNumberText.text = nextLevel.ToString();
			titleText.text = resultsMessage;

		}


		string ParseMessageString (string rawMessage, int numberOfBounces, int numberOfCoins, string typeOfAnimal) {
			rawMessage = rawMessage.Replace("@A", typeOfAnimal); 
			rawMessage = rawMessage.Replace("@B", numberOfBounces.ToString());
			rawMessage = rawMessage.Replace("@C", numberOfCoins.ToString ()); 
			return rawMessage ;
		}

	}
}
