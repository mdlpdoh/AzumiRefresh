using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
    /// <summary>
    /// This script is attached to the ScoreBGpanel in the LevelCanvas game object. 
    ///  It monitors the amount of swipes left and begins the flashing animation when the swipes are low.
    /// </summary>

    public class ScoreCounter : MonoBehaviour
    {
        private Text numeral01;
        private Text numeral02;
        private Text numeral03;
        public float flashInterval = 0.5f;
        private bool fadingIn = true;
        //private int swipesRemaining; //Don't think this is needed...?
        private bool alreadyFlashing;

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
            EventManager.ListenForEvent(AzumiEventType.SwipesAboveMinimum, StopFlashing);
            EventManager.ListenForEvent(AzumiEventType.SwipesLow, StartFlashing);
            EventManager.ListenForEvent(AzumiEventType.LevelLost, StopFlashing);
            EventManager.ListenForEvent(AzumiEventType.LevelWon, StopFlashing);
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
        void OnDestroy()
        {
            EventManager.Instance.RemoveListener(AzumiEventType.SetBounces, OnSetBouncesEvent);
            EventManager.Instance.RemoveListener(AzumiEventType.SwipesLow, StartFlashing);
            EventManager.Instance.RemoveListener(AzumiEventType.SwipesAboveMinimum, StopFlashing);
            EventManager.Instance.RemoveListener(AzumiEventType.LevelLost, StopFlashing);
            EventManager.Instance.RemoveListener(AzumiEventType.LevelWon, StopFlashing);
        }
        void OnSetBouncesEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            AssignNumeralsToTextBoxes((int)Param);
        }

        void StartFlashing(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            if (alreadyFlashing == false)
            {
                EventManager.PostEvent(AzumiEventType.SwipesLowFadeIn, this);
                StartCoroutine("Flashing");
            }
        }

        void StopFlashing(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            if (alreadyFlashing == true)
            {
                alreadyFlashing = false;
                StopAllCoroutines();
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

            }//end while	

        }//end Ienumerator

    }//end class

}//end namespace
