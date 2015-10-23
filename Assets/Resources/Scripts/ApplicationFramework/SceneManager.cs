using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using System;

namespace com.dogOnaHorse
{
	public enum SceneState
	{
		Unassigned,
		DebugMode, 
		Init,
		Ready,
		Playing,
		Modal,
		Locked,
		GameOver,
		Closing
	}

	/**
	 *  Manages all scene assets and behaviors. SceneManager mediates between the GameManager, SceneManager and other high level objects associated with an individual scene.
	 */
	public class SceneManager : StateBehaviour
	{

		
		private static SceneManager instance = null;

		public static SceneManager Instance { 
			// return reference to private instance 
			get { 
				return instance; 
			} 
		}

		public ModalWindow AboutPangolinsWindow;
		public UIMessageBehavior TitleText;
		public bool developMode = false;
		public InputManager inputManager;
		public  Dictionary<ButtonID, ModalWindow> modalWindowDictionary = new Dictionary<ButtonID, ModalWindow> ();
		
		//public Dictionary<int, StudentName> students = new Dictionary<int, StudentName>()
			
		void Awake ()
		{
			if (instance) {
				DestroyImmediate (gameObject); 
				return;
			}
			// Make this active and only instance
			instance = this;
			DontDestroyOnLoad (gameObject);
			Initialize<SceneState> ();
		}

		public void InitScene ()
		{
			print ("InitScene");
				Canvas canvas = GameObject.FindObjectOfType (typeof(Canvas)) as Canvas;

				ModalWindow[] modals = canvas.GetComponentsInChildren<ModalWindow> (true);
				modalWindowDictionary.Clear();
				for (int i=0; i<modals.Length; i++) {
					modalWindowDictionary.Add (modals [i].buttonID, modals [i]);
				}
			inputManager = GameObject.Find ("GameScripts").GetComponent<InputManager> ();
			//	inputManager.RegisterCurrentSceneManager(this);
			if (developMode) {
				ChangeState (SceneState.DebugMode);
			} else {
				ChangeState (SceneState.Init);
			}
			EventManager.ListenForEvent (AzumiEventType.HitDoor, OnHitDoorEvent);
		}

		void OnLevelWasLoaded ()
		{
		
	
		}

		public SceneState GetCurrentState ()
		{
			return  (SceneState)Enum.Parse (typeof(SceneState), GetState ().ToString ());
			
		}

		public void  StartGamePlay ()
		{
			ChangeState (SceneState.Playing);
		}

		public void ButtonClicked (ButtonID buttonID, ButtonAction buttonAction)
		{
			if (buttonAction == ButtonAction.OpenModal) {
				ChangeState (SceneState.Modal);
				modalWindowDictionary [buttonID].DoButtonAction (buttonAction);
			} else if (buttonAction == ButtonAction.CloseModal) {

				ChangeState (SceneState.Ready);
				modalWindowDictionary [buttonID].DoButtonAction (buttonAction);
			} else if (buttonAction == ButtonAction.NextScreen) {
				ChangeState (SceneState.Closing);
				GameManager.ChangeScene (buttonID, buttonAction);
			
			} else if (buttonAction == ButtonAction.ResetLevel) {	
				ChangeState (SceneState.Ready);
				modalWindowDictionary[buttonID].DoButtonAction (ButtonAction.CloseModal);
				GameManager.ReloadScene();
			}
		}

		public void LevelButtonClicked (int levelNumber, int chapterNumber)
		{
			if (GameManager.ChangeScene (levelNumber, chapterNumber)) {
				ChangeState (SceneState.Closing);
			}
		}

		public void OnHitDoorEvent (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			ChangeState(SceneState.GameOver);
			
		}

		#region State methods
		//Enter Actions
		void Init_Enter ()
		{
			Debug.Log ("Scene Manager:  Inited");
			ChangeState (SceneState.Ready);
		}

		void Ready_Enter ()
		{
			Debug.Log ("Scene Manager: Ready");
		}

		void Playing_Enter ()
		{
			Debug.Log ("Scene Manager: Playing");
		
		}

		void GameOver_Enter ()
		{
			Debug.Log ("Scene Manager: GameOver");
			EventManager.ClearGameLevelListeners ();
			GameManager.GameOver();
			modalWindowDictionary [ButtonID.LevelResults].DoButtonAction (ButtonAction.OpenModal);
			//Debug.Log("Game is Over");
			//GameManager.ReturnToProgressScreen ();
		}

		void Modal_Enter ()
		{
			Debug.Log ("Modal open");
		
		}

		void DebugMode_Enter ()
		{
			ChangeState (SceneState.Init);
		}
		#endregion


	}
	
}

