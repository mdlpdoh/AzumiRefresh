using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace com.dogOnaHorse
{
	/**
	 *  Manages all player input. InputManager controls any touch, keyboard or mouse input by the user, and passes it on to the current scene manager for interpretation.
	 */
	public class ProgressInputManager : MonoBehaviour
	{
		//private GameManager gameManager;
		private ProgressSceneManager sceneManager;
		//private bool inputLocked = true;


		// Use this for initialization
		void Awake () {
			sceneManager = GetComponent<ProgressSceneManager>();
			//gameManager  = GameObject.Find ("GameScripts").GetComponent<GameManager>();
		}


		public void LevelButtonClicked (String levelName)
		{
			if (sceneManager.GetCurrentState () == ProgressSceneState.Ready) {
				GameManager.ChangeLevel(levelName);
			}
		}


		
	}
}
