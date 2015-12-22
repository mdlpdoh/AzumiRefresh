using UnityEngine;
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


        void Awake()
        {
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
/*
                //			print (myText.text);
                if (myText.text == "2")
                {
                    print("START FLASHING!");
                    swipeNotifier();

                }
    */
            }

            void swipeNotifier()
        {
                // TODO
                // MAKE THE THE NUMBER OF SWIPES COUNTDOWN FLASH WHEN THERE ARE 4 SWIPES LEFT
            }
        }
    }

