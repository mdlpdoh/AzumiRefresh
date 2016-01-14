using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace com.dogonahorse
{


    public class ScoreManager : MonoBehaviour
    {
        public string ChapterAnimalName = "Drop Bear";


        public Color ChapterMainColor;
        public Color ChapterSecondColor;

        public int ChapterNumber = 0;
        public int LevelNumber = 0;
        public int MaxTaps = 400;
        public int TwoStarLevel = 200;
        public int TwoStarBonus = 10;
        public int ThreeStarLevel = 300;
        public int ThreeStarBonus = 20;
        private int swipesRemaining;
        public int CoinsInLevel = 0;
        private int coinsEarned = 0;
        private bool hitWallsCostsPoints = true;
        private bool playerActionsCostPoints = true;
        private bool exitedDoorSafely = false;

        public bool ExitedDoorSafely
        {
            // return reference to private instance 
            get
            {
                return exitedDoorSafely;
            }
        }


        public int numberOfBounces
        {
            // return reference to private instance 
            get
            {
                return swipesRemaining;
            }
        }

        public int CoinsEarned
        {
            // return reference to private instance 
            get
            {
                return coinsEarned;

            }
        }


        public int NumberOfStars
        // number of stars given based on percentage of ants(coins) cleared in level
        {
            // return reference to private instance 
            get
            {

                float twentyPercent = 0.2f * CoinsInLevel;
                int oneStar = (int)Math.Round(twentyPercent);

                float sixtyPercent = 0.6f * CoinsInLevel;
                int twoStar = (int)Math.Round(sixtyPercent);

                float ninetyPercent = 0.9f * CoinsInLevel;
                int threeStar = (int)Math.Round(ninetyPercent);

                if (exitedDoorSafely == true && coinsEarned >= threeStar)
                {
                    return 3;
                }
                else if (exitedDoorSafely == true && coinsEarned >= twoStar)
                {
                    return 2;
                }
                else if (exitedDoorSafely == true && coinsEarned >= oneStar)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int TotalScore
        // number of stars given based on percentage of ants(coins) cleared in level
        {
            // return reference to private instance 
            get
            {
				if (NumberOfStars == 0) 
				{
					return 0; 
				}
				else
				{
                	return swipesRemaining + coinsEarned;
				}
            } // end get
        } // end



        //private float elapsedTime;
        private Dictionary<PowerUpType, int> availablePowerUps = new Dictionary<PowerUpType, int>();
        private Dictionary<PowerUpType, int> powerUpsUsed = new Dictionary<PowerUpType, int>();

        private SceneManager sceneManager;

        public void ChangeMaxScore(int newScore)
        {
            MaxTaps = newScore;
            swipesRemaining = newScore;
            EventManager.PostEvent(AzumiEventType.SetBounces, this, swipesRemaining);
        }

        void Start()
        {
            LevelManager.InitializateLevelValues(this);
            sceneManager = GameObject.Find("SceneScripts").GetComponent<SceneManager>();
            EventManager.ListenForEvent(AzumiEventType.HitWall, OnHitWallEvent);
            EventManager.ListenForEvent(AzumiEventType.GameSwipe, OnPlayerActionEvent);
            EventManager.ListenForEvent(AzumiEventType.HitCollectible, OnHitCollectibleEvent);
            EventManager.ListenForEvent(AzumiEventType.HitDoor, OnHitDoorEvent);
            EventManager.ListenForEvent(AzumiEventType.OutOfBounces, OnOutOfBounces);
            EventManager.ListenForEvent(AzumiEventType.OutOfTime, OnOutOfTime);
            InitScoreUI();
            swipesRemaining = MaxTaps;
        }

        //#if UNITY_EDITOR
        /*
		void OnEnable (){
			print ("OnEnable Called: " +  Application.isPlaying);
		}
		void OnValidate ()
		{
			print ("OnValidate Called: " +  Application.isPlaying);
			//LevelManager.UpdateLevelValues(this);
		}
*/


        void InitScoreUI()
        {
            exitedDoorSafely = false;
            ScoreCounter scoreCounter = GameObject.Find("ScoreBGpanel").GetComponent<ScoreCounter>();
            CoinCounter coinCounter = GameObject.Find("CoinsNumber").GetComponent<CoinCounter>();
            scoreCounter.SetStartingAmount(MaxTaps);
            coinCounter.SetStartingAmount(0);// <--need to get from account
        }
        public void SetHitWallsCostsPoints(bool newValue)
        {
            hitWallsCostsPoints = newValue;
        }
        public void PlayerActionsCostPoints(bool newValue)
        {
            playerActionsCostPoints = newValue;
        }
        public void OnHitWallEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {

            if (sceneManager.GetCurrentState() == SceneState.Playing)
            {

                try
                {
                    Collision2D wallCollider = (Collision2D)Param;
                    int currentWallValue = wallCollider.gameObject.GetComponent<WallBehavior>().GetWallScoreValue(); ;

                    //don't do anything if ball hit an ordinary wall
                    if (currentWallValue != 0)
                    {
                        if (swipesRemaining + currentWallValue >= 0)
                        {
                            swipesRemaining += currentWallValue;
                        }
                        if (swipesRemaining == 0)
                        {
                            EventManager.PostEvent(AzumiEventType.OutOfBounces, this);
                        }

                        EventManager.PostEvent(AzumiEventType.SetBounces, this, swipesRemaining);
                    }
                }

                catch
                {
                    Debug.Log("Horrible things happened! No WallBehavior script was found!");
                }
            }
        }
        public void OnPlayerActionEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            if (sceneManager.GetCurrentState() == SceneState.Playing)
            {
                //if (playerActionsCostPoints) 
                if (swipesRemaining > 0)
                {
                    swipesRemaining--;

                }
                else
                {
                    exitedDoorSafely = false;
                    EventManager.PostEvent(AzumiEventType.OutOfBounces, this);
                }
                if (swipesRemaining < 5)
                {
                    EventManager.PostEvent(AzumiEventType.SwipesLow, this);
                }
                if (swipesRemaining == 0)
                {
                    EventManager.PostEvent(AzumiEventType.StartTimer, this);
                }
                EventManager.PostEvent(AzumiEventType.SetBounces, this, swipesRemaining);

            }

        }
        public void OnHitCollectibleEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            if (sceneManager.GetCurrentState() == SceneState.Playing)
            {
                coinsEarned++;
                EventManager.PostEvent(AzumiEventType.SetCoins, this, coinsEarned);
            }
        }

        public void OnHitDoorEvent(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            exitedDoorSafely = true;

            if (NumberOfStars > 0)
            {
                print("posting level won event");
                EventManager.PostEvent(AzumiEventType.LevelWon, this);
            }
            else
            {
                EventManager.PostEvent(AzumiEventType.LevelLost, this);
            }

        }

        public void OnOutOfBounces(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            EventManager.PostEvent(AzumiEventType.LevelLost, this);
        }

        public void OnOutOfTime(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            EventManager.PostEvent(AzumiEventType.LevelLost, this);
        }

    }//end class
}//end namespace