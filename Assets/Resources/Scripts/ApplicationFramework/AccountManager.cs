using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{
	public class AccountManager : MonoBehaviour
	{

		private string userName;
		private string password;

		private int numberOfCoins;



		//private IList LevelProgress;

		private static AccountManager instance = null;
		
		public static AccountManager Instance { 
			// return reference to private instance 
			get { 
				return instance; 
			} 
		}

		void Awake ()
		{
			if (instance) {
				DestroyImmediate (gameObject); 
				return;
			}
			// Make this active and only instance
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	}
}
