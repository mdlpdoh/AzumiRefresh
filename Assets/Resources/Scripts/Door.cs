using UnityEngine;
using System.Collections;





public class Door : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.name == "Ball")
		{
			Destroy(col.gameObject);
			print("Ball has been destroyed");
			//GameManager.ReturnToProgressScreen();
		}
	}
	}
