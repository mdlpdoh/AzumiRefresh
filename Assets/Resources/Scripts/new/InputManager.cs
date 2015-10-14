using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace com.dogOnaHorse
{
	/**
	 *  Manages all player input. InputManager controls any touch, keyboard or mouse input by the user, and passes it on to the current scene manager for interpretation.
	 */
	public enum ButtonAction
	{
		Unassigned,
		Play,
		AboutPangolinsOpen, 
		AboutPangolinsClose, 
		Store,
		Achievements,
		LeaderBoards,
		Settings,
		PowerUps,
		Connect
	}

	public class InputManager : MonoBehaviour
	{
		//public GameManager gameManager;
		public SceneManager sceneManager;
		//private bool inputLocked = true;
		// Use this for initialization

		void Awake ()
		{
			//sceneManager = GetComponent<SceneManager>();
			//gameManager  = GameObject.Find ("GameScripts").GetComponent<GameManager>();
		}

		public void RegisterCurrentSceneManager (SceneManager newSceneManager)
		{
			sceneManager = newSceneManager;
		}

		#region Button Input
		public void MainButtonClicked (ButtonAction mainButtonID)
		{
			if (sceneManager.GetCurrentState () == SceneState.Ready) {


				if (sceneManager.GetCurrentState () == SceneState.Ready) {
					print ("mainButtonID " + mainButtonID);

					switch (mainButtonID) {
					case ButtonAction.Play:
						break;
					case ButtonAction.AboutPangolinsOpen:
						sceneManager.ModalOpen (mainButtonID);
						break;
					case ButtonAction.Connect:
						break;
					case ButtonAction.Store:
						break;
					case ButtonAction.Achievements:
						break;
					case ButtonAction.LeaderBoards:
						break;
					case ButtonAction.Settings:
						break;
					}
				} 

			}
		}

		public void ModalButtonClicked (ButtonAction modalButtonID)
		{
			if (sceneManager.GetCurrentState () == SceneState.Modal) {
				sceneManager.ModalOpen (modalButtonID);
			}
		}
		#endregion
	}
}
