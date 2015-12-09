using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace com.dogonahorse
{
	/**
	 *  Manages all player input. InputManager controls any touch, keyboard or mouse input by the user, and passes it on to the current scene manager for interpretation.
	 */
	public class InputManager : MonoBehaviour
	{
		//public GameManager gameManager;
		public SceneManager sceneManager;
		public bool UIControlIsActive;
		
		private bool InitialMouseDownIsValid = false;
		
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
		

		//swipe version
		void Update ()
		{


			if (Input.GetMouseButtonDown (0) && !UIControlIsActive) {
				InitialMouseDownIsValid = true;
				lastMousePosition =  Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint (Input.mousePosition)));
				//start game if it hasn't already started kejf
				if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Ready) {

					//lastMousePosition = Camera.main.ScreenToViewportPoint (Input.mousePosition);
					sceneManager.StartGamePlay ();
					EventManager.PostEvent (AzumiEventType.GamePress, this,lastMousePosition);
				}
				//prepare Arrow for activation
				if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
					lastMousePosition = Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint (Input.mousePosition)));
					EventManager.PostEvent (AzumiEventType.GamePress, this,  lastMousePosition);
				}
			}

			//Update active arrow
			Vector3 newMousePosition;
			if (Input.GetMouseButton (0) && !UIControlIsActive) {
			
				if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
					newMousePosition = Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint (Input.mousePosition)));
					if (lastMousePosition != newMousePosition) {
						EventManager.PostEvent (AzumiEventType.GameShift, this, newMousePosition);
					}
					
				}
				
			}
			//Release mouse 
			if (GameManager.GetCurrentState () == GameState.GameLevel && sceneManager.GetCurrentState () == SceneState.Playing) {
			
				if (Input.GetMouseButtonUp (0) && !UIControlIsActive && InitialMouseDownIsValid) {
//					print ("GetMouseButtonUp");
					InitialMouseDownIsValid = false;
					newMousePosition = Camera.main.ViewportToWorldPoint(FixCoordinates(Camera.main.ScreenToViewportPoint (Input.mousePosition)));
					EventManager.PostEvent (AzumiEventType.GameRelease, this, newMousePosition);
					if (MainDirectionSelected) {
						EventManager.PostEvent (AzumiEventType.GameSwipe, this, lastMousePosition - newMousePosition);
					} else {
						EventManager.PostEvent (AzumiEventType.GameSwipe, this, newMousePosition - lastMousePosition);
					}
				}
			}
		
		}
	

		Vector3 FixCoordinates (Vector3 screenCoordinates){
			return new Vector3(screenCoordinates.x, screenCoordinates.y, 10);
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
			//print ("ControlActive");
			UIControlIsActive = true;
		}

		public void ControlNotActive ()
		{
			//print ("ControlNotActive");
			UIControlIsActive = false;
		}

		#endregion
	}
}
