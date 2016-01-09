using UnityEngine;
using System.Collections;

public class dragAndSpringBack : MonoBehaviour {

	private Vector2 theObjectPos;
	
	void OnMouseDrag() {
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
		transform.position = new Vector2(transform.position.x, objPos.y);
	}

	// Use this for initialization
	void Start () {
		theObjectPos = GameObject.Find ("Dragger 2").transform.position;
		print (theObjectPos);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)){
			transform.position = new Vector2(theObjectPos.x, theObjectPos.y);
		}
	
	}
}//end class
