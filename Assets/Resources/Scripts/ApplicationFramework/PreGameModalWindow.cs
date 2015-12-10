using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.dogonahorse
{


	public class PreGameModallWindow : ModalWindow
	{

		private string beginLevelMessage = "Eat as Many ants as you can before leaving the room.\nYou have @B\nswipes to free the captive\n@A!";


		private ScoreManager scoreManager;



	
		override public void InitWindow() {
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
			int numberOfBounces = scoreManager.numberOfBounces;
			string typeOfAnimal = scoreManager.ChapterAnimalName;;
			string resultsMessage;

			resultsMessage = ParseMessageString(beginLevelMessage,numberOfBounces,typeOfAnimal);

			Text messageText = GetComponentInChildren<Text>();
		//	print (messageText);
			messageText.text = resultsMessage;

		}


		string ParseMessageString (string rawMessage, int numberOfBounces, string typeOfAnimal) {
			rawMessage = rawMessage.Replace("@A", typeOfAnimal); 
			rawMessage = rawMessage.Replace("@B", numberOfBounces.ToString()); 
			return rawMessage ;
		}
		
	}
}
