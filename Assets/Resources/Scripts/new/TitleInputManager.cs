using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace com.dogOnaHorse
{
	/**
	 *  Manages all player input. InputManager controls any touch, keyboard or mouse input by the user, and passes it on to the current scene manager for interpretation.
	 */
	public class TitleInputManager : MonoBehaviour
	{

		public TitleSceneManager sceneManager;



		// Use this for initialization
		void Awake () {
			sceneManager =GetComponent<TitleSceneManager>();
		}


		public void PlayButtonClicked ()
		{
			if (sceneManager.GetCurrentState () == TitleSceneState.Ready) {
				Application.LoadLevel("Progress");
			}
		}

		public void AboutPangolinsButtonClicked ()
		{
			if (sceneManager.GetCurrentState () == TitleSceneState.Ready) {
				sceneManager.AboutPangolinsOpen ();
			}
		}

		public void AboutCloseClicked ()
		{
			if (sceneManager.GetCurrentState () == TitleSceneState.About) {
				sceneManager.AboutPangolinsClose ();
			}
		}
		
	}
}
