using UnityEngine;
using System.Collections;

public class WallStop : MonoBehaviour {
		
	public float farLeft = -2.75f;
	public float farRight = 4.32f;

	// Use this for initialization
	void Start () {
 
	}

	void OnMouseDrag() {
		Vector2 newPos = transform.position;
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
		// decide where to put the block.
		if (objPos.x <= farLeft) {
			newPos.x = farLeft;
		} else if (objPos.x >= farRight) {
			newPos.x = farRight;
		} else {
			newPos.x = objPos.x;
		}
		//send the block to that position.
		transform.position = new Vector2 (newPos.x,  transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}//end class
