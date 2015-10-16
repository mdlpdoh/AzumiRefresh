using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using System;
namespace com.dogOnaHorse
{


	public class LevelManager : MonoBehaviour
	{

		private static LevelManager instance = null;
	

		public static LevelManager Instance { 
		// return reference to private instance 
		
			get { 
				return instance; 
			} 
		}
		//public  GameState defaultState;

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

		void Start () {

			//ChangeState(GameState.Init);

		}
	
	
	}
}