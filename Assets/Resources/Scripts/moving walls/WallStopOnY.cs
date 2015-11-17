using UnityEngine;
using System.Collections;


namespace com.dogonahorse
{
public class WallStopOnY : MonoBehaviour {

	public float farUp = 4.25f;
	public float farDown = 2.96f;

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() 
	{
		InputManager.Instance.ControlActive();
	}
	void OnMouseUp() 
	{
		InputManager.Instance.ControlNotActive();
	}
		void OnMouseDrag() {
		Vector2 newPos = transform.position;
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
		// decide where to put the block.
		if (objPos.y >= farUp) {
			newPos.y = farUp;
		} else if (objPos.y <= farDown) {
			newPos.y = farDown;
		} else {
			newPos.y = objPos.y;
		}
		//send the block to that position.
		transform.position = new Vector2 (transform.position.x, newPos.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
}
