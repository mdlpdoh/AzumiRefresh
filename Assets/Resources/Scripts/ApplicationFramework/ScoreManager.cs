using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace com.dogonahorse
{


    public class ScoreManager : MonoBehaviour
    {
        public string ChapterAnimalName = "Drop Bear";
        public int MaxTaps = 400;
        public int TwoStarLevel = 200;
        public int TwoStarBonus = 10;
        public int ThreeStarLevel = 300;
        public int ThreeStarBonus = 20;
        private int bouncesRemaining;
        public int CoinsInLevel = 0;
        private int coinsEarned = 0;
        private bool hitWallsCostsPoints = true;
        private bool playerActionsCostPoints = true;

        public int numberOfBounces
        {
            // return reference to private instance 
            get
            {
                return bouncesRemaining;
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
            get {

				float twentyPercent = 0.2f * CoinsInLevel;
				int oneStar = (int) Math.Round (twentyPercent);					

				float sixtyPercent = 0.6f * CoinsInLevel;
				int twoStar = (int) Math.Round (sixtyPercent);

				float ninetyPercent = 0.9f * CoinsInLevel;
				int threeStar = (int) Math.Round (ninetyPercent);

				if (coinsEarned >= threeStar && bouncesRemaining > -1) 
				{
					return 3;
				} 
				else if (coinsEarned >= twoStar && bouncesRemaining > -1) 
				{
					return 2;
				} 
				else if (coinsEarned <= oneStar && bouncesRemaining > -1) 
				{
					return 1;
				} 
				else 
				{
					return 0;
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
            bouncesRemaining = newScore;
            EventManager.PostEvent(AzumiEventType.SetBounces, this, bouncesRemaining);
        }

        void Start()
        {
            LevelManager.InitializateLevelValues(this);
            sceneManager = GameObject.Find("SceneScripts").GetComponent<SceneManager>();
            EventManager.ListenForEvent(AzumiEventType.HitWall, OnHitWallEvent);
            EventManager.ListenForEvent(AzumiEventType.GameSwipe, OnPlayerActionEvent);
            EventManager.ListenForEvent(AzumiEventType.HitCollectible, OnHitCollectibleEvent);
            EventManager.ListenForEvent(AzumiEventType.HitDoor, OnHitDoorEvent);
            InitScoreUI();
            bouncesRemaining = MaxTaps;
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
                        if (bouncesRemaining + currentWallValue >= 0)
                        {
                            bouncesRemaining += currentWallValue;
                        }
                        if (bouncesRemaining == 0)
                        {
                            EventManager.PostEvent(AzumiEventType.OutOfBounces, this);
                        }

                        EventManager.PostEvent(AzumiEventType.SetBounces, this, bouncesRemaining);
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
                //if (playerActionsCostPoints) {
                if (bouncesRemaining >= 0)
                {
                    bouncesRemaining--;
                }
				if (bouncesRemaining < 0)
                {
                    EventManager.PostEvent(AzumiEventType.OutOfBounces, this);
                }

                EventManager.PostEvent(AzumiEventType.SetBounces, this, bouncesRemaining);
                //}
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
            if (sceneManager.GetCurrentState() == SceneState.Playing)
            {

            }
        }
    }
}