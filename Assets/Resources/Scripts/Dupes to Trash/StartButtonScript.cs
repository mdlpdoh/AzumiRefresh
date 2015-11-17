using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour {
		
	public void StartGame() {
		//		Application.LoadLevel (Application.loadedLevel + 1);
		Application.LoadLevel ("sceneOne");
	}

	public void EndGame() {
		//		Application.LoadLevel (Application.loadedLevel + 1);
		Application.LoadLevel ("_start");
	}
		
//		public void LoadMainGameLevel() {
//			Application.LoadLevel ("Main Game Scene");
//		}
	}