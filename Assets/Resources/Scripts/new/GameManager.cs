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

			return  (GameState)Enum.Parse(typeof(GameState), instance.GetState().ToString());
			
		}
		public static void ChangeScene(GameState newScene) {
			if (newScene != GameState.GameLevel){
				Application.LoadLevel(newScene.ToString());
				Instance.ChangeState(newScene);
			} else {
				//somethimg else
			}
			
		}

		public static void ChangeLevel(string newLevelName) {

			
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