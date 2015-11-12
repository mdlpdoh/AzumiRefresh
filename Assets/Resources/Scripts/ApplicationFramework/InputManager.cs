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
		public bool UIControlIsActive;
		private Vector3 lastMousePosition;

		//Internal reference to Notifications Manager instance (singleton design pattern)
		private static InputManager instance = null;

		//private  EventManager eventManager;

		public static bool MainDirectionSelected = true;

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
			sceneManager = GameObject.Find ("SceneScripts").GetComponent<SceneManager> ();
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

	
		/*  tap version
		void Update ()
		{
			if (Input.GetMouseButtonDown (0)  && !UIControlIsActive) {
			
				if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Ready) {

					sceneManager.StartGamePlay();
					EventManager.PostEvent(AzumiEventType.GameTap, this, null);
				} else if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing){
					///communicate with ball
					EventManager.PostEvent(AzumiEventType.GameTap, this, null);
				}
			}	
		}
	*/

		/*smush version
		void Update ()
		{
			if (Input.GetMouseButtonDown (0)  && !UIControlIsActive) {
				lastMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Ready) {

					sceneManager.StartGamePlay();
				}
			}

			if (Input.GetMouseButton(0) && !UIControlIsActive) {
			
				if (GameManager.GetCurrentState() == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
					Vector3 newMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
					if (lastMousePosition != newMousePosition){
						EventManager.PostEvent(AzumiEventType.GameSwipe, this, newMousePosition-lastMousePosition);

						//print (newMousePosition);
						lastMousePosition = newMousePosition;
					}
	
				}

			}
		}
*/

		//swipe version
		void Update ()
		{

		
			if (Input.GetMouseButtonDown (0) && !UIControlIsActive) {

				//start game if it hasn't already started kejf
				if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Ready) {
					sceneManager.StartGamePlay ();
				}
				//prepare Arrow for activation
				lastMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
				EventManager.PostEvent (AzumiEventType.GamePress, this, lastMousePosition);
			}

			//Update active arrow
			Vector3 newMousePosition;
			if (Input.GetMouseButton (0) && !UIControlIsActive) {
			
				if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
					newMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
					if (lastMousePosition != newMousePosition) {
						EventManager.PostEvent (AzumiEventType.GameShift, this, newMousePosition);
					}
					
				}
				
			}
			//Release mouse 
			if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
			
				if (Input.GetMouseButtonUp (0) && !UIControlIsActive) {
					newMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
					EventManager.PostEvent (AzumiEventType.GameRelease, this, newMousePosition);
					if (MainDirectionSelected) {
						EventManager.PostEvent (AzumiEventType.GameSwipe, this, lastMousePosition - newMousePosition);
					} else {
						EventManager.PostEvent (AzumiEventType.GameSwipe, this, newMousePosition - lastMousePosition);
					}
				}
			}

			/*
			if (MainDirectionSelected) {

				if (Input.GetMouseButtonDown (0) && !UIControlIsActive) {
					lastMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
					if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Ready) {
						
						sceneManager.StartGamePlay ();
					}
				}
				
				if (Input.GetMouseButton (0) && !UIControlIsActive) {
					
					if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
						Vector3 newMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
						if (lastMousePosition != newMousePosition) {
							EventManager.PostEvent (AzumiEventType.GameSwipe, this, newMousePosition - lastMousePosition);
							lastMousePosition = newMousePosition;
						}
						
					}
					
				}

			} else {
				if (Input.GetMouseButtonDown (0) && !UIControlIsActive) {
	
					lastMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
					if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Ready) {
						sceneManager.StartGamePlay ();
					}
				}

				if (Input.GetMouseButtonUp (0) && !UIControlIsActive) {
			
					if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
						Vector3 newMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
						if (lastMousePosition != newMousePosition) {
							EventManager.PostEvent (AzumiEventType.GameSwipe, this, newMousePosition - lastMousePosition);

							//print (newMousePosition);
							lastMousePosition = newMousePosition;
						}
	
					}

				}

			}*/
		}
		#region Button Input
		public void MainButtonClicked (ButtonID buttonID, ButtonAction buttonAction)
		{
			if (sceneManager.GetCurrentState () == SceneState.Ready || sceneManager.GetCurrentState () == SceneState.Playing) {
				sceneManager.ButtonClicked (buttonID, buttonAction);

			}
		}

		public void ModalButtonClicked (ButtonID buttonID, ButtonAction buttonAction)
		{

			SceneState sceneState = sceneManager.GetCurrentState ();
			if (sceneState == SceneState.Modal || sceneState == SceneState.GameOver) {

				sceneManager.ButtonClicked (buttonID, buttonAction);
				
			}
		}

		public void LevelButtonClicked (int LevelNumber, int chapterNumber)
		{
			if (sceneManager.GetCurrentState () == SceneState.Ready) {
				sceneManager.LevelButtonClicked (LevelNumber, chapterNumber);
				
			}
		}

		public void ControlActive ()
		{
			print ("ControlActive");
			UIControlIsActive = true;
		}

		public void ControlNotActive ()
		{
			print ("ControlNotActive");
			UIControlIsActive = false;
		}

		#endregion
	}
}
