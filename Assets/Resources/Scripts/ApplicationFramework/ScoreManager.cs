using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using System;
namespace com.dogOnaHorse
{


	public class ScoreManager : MonoBehaviour
	{

		private static ScoreManager instance = null;
	

		public static ScoreManager Instance { 
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
			//Initialize<GameState>();
		}


	}
}