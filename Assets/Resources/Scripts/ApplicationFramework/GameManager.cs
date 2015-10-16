using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using System;
namespace com.dogOnaHorse
{

	public enum GameState
	{
		Unassigned,
		DebugMode, 
		Init,
		Title,
		Progress,
		GameLevel,
		Win,
		Lose
	}
	public class GameManager : StateBehaviour
	{

		private static GameManager instance = null;
	

		public static GameManager Instance { 
		// return reference to private instance 
		
			get { 
				return instance; 
			} 
		}

		public  GameState defaultState;

		void Awake ()
		{
			// Check if existing instance of class exists in scene 35 
			// If so, then destroy this instance
			if (instance) {
				DestroyImmediate (gameObject); 
				return;
			}
			// Make this active and only instance
			instance = this;
			DontDestroyOnLoad (gameObject);
			Initialize<GameState>();
		}

		void Start () {
			ChangeState(GameState.Init);
		}

		public static GameState GetCurrentState() {
			return (GameState)Enum.Parse(typeof(GameState), instance.GetState().ToString());		
		}
	
		public static void ChangeScene(ButtonID buttonID, ButtonAction buttonAction) {
			GameState currentState = GetCurrentState();
			GameState newState;
			switch (currentState ) {
			case GameState.Title:
				newState = GameState.Progress;
				Application.LoadLevel(newState.ToString());
				Instance.ChangeState(newState);
				break;
				/*
			case GameState.Progress:
				newState = GameState.Progress;
				Application.LoadLevel(newState.ToString());
				Instance.ChangeState(newState);
				break;
			*/
			case GameState.GameLevel:
				newState = GameState.Progress;
				Application.LoadLevel(newState.ToString());
				Instance.ChangeState(newState);
				break;

			default:
				break;
				
			}

		}
		public static void ReturnToProgressScreen(){
			Instance.ChangeState (GameState.Progress );
			Application.LoadLevel("Progress");
			
		}


		public static bool ChangeScene(int levelNumber, int chapterNumber) {
			GameState currentState = GetCurrentState();
			if (currentState == GameState.Progress){
				string levelName = "Level_"+padWithZeroes(chapterNumber.ToString()) + padWithZeroes(levelNumber.ToString());;
			
				if (Application.CanStreamedLevelBeLoaded(levelName)) {

					Application.LoadLevel(levelName);
					Instance.ChangeState(GameState.GameLevel);
					return true;
				} else {
					print ("ERROR: Can't find level " + levelName);
			
				}
			}
			return false;
		}
		  static string padWithZeroes(string numberString){
			if (numberString.Length < 2){
				return "0" + numberString;
					
			}
			return  numberString;
		}




		#region State methods
		//Enter Actions
		void Init_Enter()
		{
			Debug.Log("Game Manager is now Inited");
			ChangeState(defaultState);
		}
		
		void Title_Enter()
		{
			Debug.Log("Game Manager is ready for Title Screen");
		}
		
		void Progress_Enter()
		{
			Debug.Log("Game Manager is ready for Progress Screen");
			
		}
		#endregion
	}
}