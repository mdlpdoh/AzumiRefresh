using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace com.dogonahorse
{

	public class LevelButtonBehavior : ButtonBehavior
	{
	
		public int chapterNumber;
		public int levelNumber;
	   public Text LevelNumberText;
		// Use this for initialization
		override public void Start(){
			base.Start();
			buttonType =  ButtonType.LevelButton;
	        LevelNumberText.text = levelNumber.ToString();
		}
	
		// Update is called once per frame
		override public void DoButtonAction ()
		{	
			if (buttonType== ButtonType.LevelButton) {
				InputManager.Instance.LevelButtonClicked(levelNumber, chapterNumber);
			}else {
				print ("ERROR: Button type is "+ buttonType);
			}	
		}

		 string padWithZeroes(string numberString){
			if (numberString.Length < 2){
				return "0" + numberString;
				
			}
			return  numberString;
		}
	}
}