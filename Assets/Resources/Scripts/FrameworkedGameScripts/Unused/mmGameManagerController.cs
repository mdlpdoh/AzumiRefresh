using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace com.dogOnaHorse
{
	public class mmGameManagerController : MonoBehaviour
	{

		public Text bouncesText;
		public Text successText;
		public Button tryagain;
		public Button endgame;
		public Button endgame2;
		public Button bump;
		public Button startover;
		private int bounces;

		// Use this for initialization
		void Start ()
		{
			bounces = 0;
		}

		// Update is called once per frame
		void Update ()
		{
	
		}

		public void incBounces ()
		{
			bounces ++;
		}

		public void getBounces ()
		{
			bouncesText.text = "Bounces: " + bounces;

		}

		public void getSuccess ()
		{
			successText.text = "Success!     " + "Your score is: " + bounces;
			tryagain.gameObject.SetActive (true);
			endgame.gameObject.SetActive (true);
			bump.gameObject.SetActive (false);
			startover.gameObject.SetActive (false);
			endgame2.gameObject.SetActive (false);

		
		}
	}
}