﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
	
    public class ScoreCounter : MonoBehaviour
    {
        private Text numeral01;
        private Text numeral02;
        private Text numeral03;
		private Image panLeft;
		private Image panCenter;
		private Image panRight;
		public float flashInterval = 0.5f;
		private bool fadingIn = true;
		private int swipesRemaining;
		private bool alreadyFlashing;
		private float timeRemaining = 2f;


        void Awake()
        {
			Image[] myImages = GetComponentsInChildren<Image>();
			for (int i = 0; i < myImages.Length; i++) 
			{
				if (myImages [i].name == "Redpanel_L") {
					panLeft = myImages [i];
				} else if (myImages [i].name == "Redpanel_C") {
					panCenter = myImages [i];
				} 
				else  
				{
					panRight = myImages [i];
				} 
				
			}

            Text[] myTexts = GetComponentsInChildren<Text>();
            for (int i = 0; i < myTexts.Length; i++)
            {
                if (myTexts[i].name == "ScoreNumber01")
                {
                    numeral01 = myTexts[i];
                }
                else if (myTexts[i].name == "ScoreNumber02")
                {
                    numeral02 = myTexts[i];
                }
                else
                {
                    numeral03 = myTexts[i];
                }
            }
        }
        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.SetBounces, OnSetBouncesEvent);
			EventManager.ListenForEvent(AzumiEventType.SwipesLow, StartFlashing);
			EventManager.ListenForEvent(AzumiEventType.StartTimer, EndGameStartTimer);

        }

        public void SetStartingAmount(int startingAmount)
        {
            //  myText.text = startingAmount.ToString();
            AssignNumeralsToTextBoxes(startingAmount);
        }

        public void AssignNumeralsToTextBoxes(int currentAmount)
        {
            String numberString = currentAmount.ToString();
            if (numberString.Length > 2)
            {
                numeral01.text = numberString[0].ToString();
                numeral02.text = numberString[1].ToString();
                numeral03.text = numberString[2].ToString();
            }
            else if (numberString.Length > 1)
            {
                numeral01.text = "0";
                numeral02.text = numberString[0].ToString();
                numeral03.text = numberString[1].ToString();
            }
            else
            {
				numeral01.text = "0";
                numeral02.text = "0";
                numeral03.text = numberString[0].ToString();
            }
		}

        void OnSetBouncesEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
    	{
            
             AssignNumeralsToTextBoxes((int)Param);
        }

		public int numberOfBounces
		{
			// return reference to private instance 
			get
			{
				return swipesRemaining;
			}
		}


		void StartFlashing(AzumiEventType Event_Type, Component Sender, object Param = null)
		{
            
            
            print ("-----------------StartFlashing " + alreadyFlashing);
			if (alreadyFlashing == false) 
			{
				EventManager.PostEvent (AzumiEventType.SwipesLowFadeIn, this);
				StartCoroutine ("Flashing");
			}
		}
			
		private IEnumerator Flashing()

		{
			alreadyFlashing = true;
			float currentTime = 0f;


			while (true)
			{
				
				if (currentTime < flashInterval) 
				{
					currentTime += Time.unscaledDeltaTime;
				}
			else
			{
				if (fadingIn == true) 
				{
					EventManager.PostEvent(AzumiEventType.SwipesLowFadeOut, this);
					fadingIn = false;
				}
				else 
				{
					EventManager.PostEvent(AzumiEventType.SwipesLowFadeIn, this);
					fadingIn = true;

				}
				currentTime = 0;
				
			}
				yield return null;
		
			}	

        }

		void EndGameStartTimer(AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			StartCoroutine(TheEndGameTimer(7.0F));
		}
		private IEnumerator TheEndGameTimer(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			EventManager.PostEvent(AzumiEventType.OutOfBounces, this);
		}
    }

}