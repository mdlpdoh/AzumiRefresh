 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace com.dogOnaHorse
{
	/**
	 *  Manages all player input. InputManager controls any touch, keyboard or mouse input by the user, and passes it on to the current scene manager for interpretation.
	 */


	public class InputManager : MonoBehaviour
	{
		//public GameManager gameManager;
		public SceneManager sceneManager;


		//Internal reference to Notifications Manager instance (singleton design pattern)
		private static InputManager instance = null;

		//private  EventManager eventManager;

		public static InputManager Instance { 
			// return public reference to private instance 
			
			get { 
				return instance; 
			}
		}



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
		}

		void Start ()
		{
			sceneManager = GameObject.Find("SceneScripts").GetComponent<SceneManager>();
		}
		//public static void nukeOldSceneManager()
		//{
		//	print ("*************Old Scene Manager is Nuked");
		//	Instance.sceneManager = null;
		//}
		//public void RegisterCurrentSceneManager (SceneManager newSceneManager)
	//	{
		//	sceneManager = newSceneManager;
		//}
		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {

				if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Ready) {

					sceneManager.StartGamePlay();
					EventManager.PostEvent(AzumiEventType.GameTap, this, null);
				} else if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing){
					///communicate with ball
					EventManager.PostEvent(AzumiEventType.GameTap, this, null);
				}
			}	
		}

		#region Button Input
		public void MainButtonClicked (ButtonID buttonID, ButtonAction buttonAction)
		{
			if (sceneManager.GetCurrentState () == SceneState.Ready) {
				sceneManager.ButtonClicked ( buttonID, buttonAction);

			}
		}
		public void ModalButtonClicked (ButtonID buttonID, ButtonAction buttonAction)
		{

			SceneState sceneState = sceneManager.GetCurrentState ();
			if (sceneState == SceneState.Modal || sceneState ==SceneState.GameOver) {

				sceneManager.ButtonClicked ( buttonID, buttonAction);
				
			}
		}

		public void LevelButtonClicked (int LevelNumber, int chapterNumber)
		{
			if (sceneManager.GetCurrentState () == SceneState.Ready) {
				sceneManager.LevelButtonClicked (LevelNumber, chapterNumber);
				
			}
		}
		#endregion
	}
}
