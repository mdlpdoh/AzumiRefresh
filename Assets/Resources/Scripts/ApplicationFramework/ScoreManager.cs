using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace com.dogOnaHorse
{


	public class ScoreManager : MonoBehaviour
	{
		public string TypeOfAnimal = "Drop Bear";
		public int maxBounces = 400;
		public int TwoStarBouncelevel = 200;
		public int TwoStarCoinBonus = 10;
		public int ThreeStarBouncelevel = 300;
		public int ThreeStarCoinBonus = 20;
		private int bouncesRemaining;
		private int coinsEarned = 0;
		//private int numberOfStars = 0;

		public  int numberOfBounces  { 
			// return reference to private instance 
			get { 
				return bouncesRemaining; 
			} 
		}

		public int numberOfCoins  { 
			// return reference to private instance 
			get { 
				return coinsEarned; 
			} 
		}

		public int NumberOfStars  { 
			// return reference to private instance 
			get { 
				if (bouncesRemaining > ThreeStarBouncelevel){
					return 3;
				} else if (bouncesRemaining > TwoStarBouncelevel){
					return 2;
				} else if (bouncesRemaining > 0){
					return 1;
				} else {
					return 0;
				}
			} 
		}

		//private float elapsedTime;
		private Dictionary<PowerUpType, int> availablePowerUps = new Dictionary<PowerUpType, int>();
		private Dictionary<PowerUpType, int> powerUpsUsed = new Dictionary<PowerUpType, int>();

		private SceneManager sceneManager;

		void Awake ()
		{
			//initialize tracking variables
			bouncesRemaining = maxBounces;
		}

		void Start ()
		{
			sceneManager = GameObject.Find ("SceneScripts").GetComponent<SceneManager>();
			EventManager.ListenForEvent(AzumiEventType.HitWall, OnHitWallEvent);
			EventManager.ListenForEvent(AzumiEventType.GameTap, OnHitWallEvent);
			EventManager.ListenForEvent(AzumiEventType.HitCollectible, OnHitCollectibleEvent);
			EventManager.ListenForEvent(AzumiEventType.HitDoor, OnHitDoorEvent);
			InitScoreParameters();
		}

		void InitScoreParameters ()
		{
			ScoreCounter scoreCounter = GameObject.Find("ScoreNumber").GetComponent<ScoreCounter>();
			CoinCounter coinCounter = GameObject.Find("CoinsNumber").GetComponent<CoinCounter>();
			scoreCounter.SetStartingAmount(maxBounces);
			coinCounter.SetStartingAmount(0);// <--need to get from account
		}
		public void getLevelResults ()
		{

		}
		public void OnHitWallEvent(AzumiEventType Event_Type, Component Sender, object Param = null){

			if (sceneManager.GetCurrentState() == SceneState.Playing){
				//print ("Event_Type: "+Event_Type + ", Sender: "+Sender + ", Param: "+Param);
				if (bouncesRemaining >= 0 ) { 
					bouncesRemaining--;
				}
				if (bouncesRemaining == 0 ) { 
					EventManager.PostEvent(AzumiEventType.OutOfBounces, this);
				}
				
				EventManager.PostEvent(AzumiEventType.SetBounces, this, bouncesRemaining);
			}
		}

		public void OnHitCollectibleEvent(AzumiEventType Event_Type, Component Sender, object Param = null){
			if (sceneManager.GetCurrentState() == SceneState.Playing){
				coinsEarned++;
				EventManager.PostEvent(AzumiEventType.SetCoins, this, coinsEarned);
			}
		}

		public void OnHitDoorEvent(AzumiEventType Event_Type, Component Sender, object Param = null){
			if (sceneManager.GetCurrentState() == SceneState.Playing){
				print ("Event_Type: "+Event_Type + ", Sender: "+Sender + ", Param: "+Param);
			}
		}
	}
}