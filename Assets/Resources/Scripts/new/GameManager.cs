using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

namespace com.dogOnaHorse
{
	public class GameManager : StateBehaviour
	{

		private static GameManager instance = null;
	
		public static GameManager Instance { 
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


	}
}