using UnityEngine;
using System.Collections;

public class starListener : MonoBehaviour {
	private GameObject star0;
	private GameObject star1;
	private GameObject star2;
	private GameObject star3;

	private GameObject coin0;
	private GameObject X1;
	private GameObject X2;
	private GameObject X3;


	public static starListener instance;
	void Awake() {
	
		instance = this; 
	}

	// Use this for initialization
	void Start () {

		//All the stars
		star0 = GameObject.Find("star0");
		star1 = GameObject.Find("star1");
		star2 = GameObject.Find("star2");
		star3 = GameObject.Find("star3");

		// All the images under the stars are turned off
		coin0 = GameObject.Find("coin0");
		coin0.GetComponent<SpriteRenderer> ().enabled = false;

		X1 = GameObject.Find("X1");
		X1.GetComponent<SpriteRenderer> ().enabled = false;

		X2 = GameObject.Find("X2");
		X2.GetComponent<SpriteRenderer> ().enabled = false;

		X3 = GameObject.Find("X3");
		X3.GetComponent<SpriteRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void destroyer(int theStar) {

		switch (theStar)
		{
		case 0:
			print ("I am star 0");
		
			// when star0 is destroyed coin image is turned on.
			Destroy(star0);
			coin0.GetComponent<SpriteRenderer> ().enabled = true;
			//Destroy all the rest
			Destroy(star1);
			Destroy(star2);
			Destroy(star3);

			//TODO:
			//** Code here to add coin to players Coin Cache.**

			break;

		case 1:
			print ("I am star 1");
	
			Destroy(star1);
			X1.GetComponent<SpriteRenderer> ().enabled = true;

			//Destroy all the rest
			Destroy(star0);
			Destroy(star2);
			Destroy(star3);

			//TODO:
			//** Code here to add coin to players Coin Cache.**

			break;

		case 2:
			print ("I am star 2");

			Destroy(star2);
			X2.GetComponent<SpriteRenderer> ().enabled = true;

			//Destroy all the rest
			Destroy(star0);
			Destroy(star1);
			Destroy(star3);

			//TODO:
			//** Code here to add coin to players Coin Cache.**
		
			break;
		case 3:
			print ("I am star 3");

			Destroy(star3);
			X3.GetComponent<SpriteRenderer> ().enabled = true;

			//Destroy all the rest
			Destroy(star0);
			Destroy(star1);
			Destroy(star2);

			//TODO:
			//** Code here to add coin to players Coin Cache.**

			break;
		}

	}
}// end class
