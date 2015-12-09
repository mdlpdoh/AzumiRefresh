using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using System;

namespace com.dogonahorse
{
	public enum SceneState
	{
		Unassigned,
		DebugMode, 
		Init,
		PreGame,
		Ready,
		Playing,
		Modal,
		Locked,
		GameOver,
		Closing,
		Resetting

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
		public GameObject devSettingsPanel;
	
		private SceneState nextState;

			
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

			Canvas canvas = GameObject.FindObjectOfType (typeof(Canvas)) as Canvas;
			devSettingsPanel = GameObject.Find("DevelopmentSettings");
			//print ("a " + devSettingsPanel);
			//print (" b "+ devSettingsPanel.GetComponent<DevelopmentPanelManager>() );
			//if (GameManager.GetCurrentState() == GameState.GameLevel){
				//devSettingsPanel.GetComponent<DevelopmentPanelManager>().Init();
			//}
			ModalWindow[] modals = canvas.GetComponentsInChildren<ModalWindow> (true);
			modalWindowDictionary.Clear ();
			for (int i=0; i<modals.Length; i++) {
				modalWindowDictionary.Add (modals [i].buttonID, modals [i]);
			}
			inputManager = GameObject.Find ("GameScripts").GetComponent<InputManager> ();
			ChangeState (SceneState.Init);
			/*
			if (developMode) {
				ChangeState (SceneState.DebugMode);
			} else {
				ChangeState (SceneState.Init);
			}
			*/
			EventManager.ListenForEvent (AzumiEventType.HitDoor, OnHitDoorEvent);
			EventManager.ListenForEvent (AzumiEventType.OutOfBounces, OnOutOfBouncesEvent);
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

				
				modalWindowDictionary [buttonID].DoButtonAction (buttonAction);
				//pause Game if opening modal while game is running re ready to start
				if (GameManager.GetCurrentState () == GameState.GameLevel && (GetCurrentState () ==  SceneState.Ready ||  GetCurrentState () == SceneState.Playing)) {
						print ("opening other modal");
					Time.timeScale = 0;
					nextState = GetCurrentState ();
				} 
				ChangeState (SceneState.Modal);

			} else if (buttonAction == ButtonAction.CloseModal) {
				if (GameManager.GetCurrentState () == GameState.GameLevel ) {
					Time.timeScale = 1;
					print("nextState " + nextState);
					ChangeState (nextState);
			
				} else {
					ChangeState (SceneState.Ready);

				}
				modalWindowDictionary [buttonID].DoButtonAction (buttonAction);
			} else if (buttonAction == ButtonAction.NextScreen) {
				Time.timeScale = 1;
				ChangeState (SceneState.Closing);
			

				GameManager.ChangeScene (buttonID, buttonAction);

			
			} else if (buttonAction == ButtonAction.ResetLevel) {	
				Time.timeScale = 1;
				ChangeState (SceneState.Ready);
				modalWindowDictionary [buttonID].DoButtonAction (ButtonAction.CloseModal);
				GameManager.ReloadScene ();
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
			ChangeState (SceneState.GameOver);
		}

		public void OnOutOfBouncesEvent (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			ChangeState (SceneState.GameOver);
		}

		#region State methods
		//Enter Actions
		void Init_Enter ()
		{
			Debug.Log ("Scene Manager:  Inited");
			ChangeState (SceneState.PreGame);
			
		}
		void PreGame_Enter ()
		{
			Debug.Log ("SceneState: PreGame");
			if (GameManager.GetCurrentState() == GameState.GameLevel) {
				ChangeState (SceneState.Modal);
				modalWindowDictionary [ButtonID.PreGameModal].DoButtonAction (ButtonAction.OpenModal);
				nextState = SceneState.Ready;
			} else{
				ChangeState (SceneState.Ready);
			}
			
			

		}
		void Ready_Enter ()
		{
			Debug.Log ("Scene Manager: Ready");
	
		}

		void Playing_Enter ()
		{
			Debug.Log ("Scene Manager: Playing");
		
		}



	void LevelReset(){
		EventManager.ClearGameLevelListeners ();
	}
		void GameOver_Enter ()
		{
			Debug.Log ("Scene Manager: GameOver");
			//LevelReset();
			GameManager.GameOver ();
	
				//devSettingsPanel.SetActive(false);
			modalWindowDictionary [ButtonID.LevelResults].DoButtonAction (ButtonAction.OpenModal);

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

