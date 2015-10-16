using UnityEngine;
using System.Collections;
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


		
		private static InputManager instance = null;
		// Use this for initialization
		public static InputManager Instance { 
			// return reference to private instance 
			
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

		public void RegisterCurrentSceneManager (SceneManager newSceneManager)
		{
			sceneManager = newSceneManager;
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
			if (sceneManager.GetCurrentState () == SceneState.Modal) {
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
