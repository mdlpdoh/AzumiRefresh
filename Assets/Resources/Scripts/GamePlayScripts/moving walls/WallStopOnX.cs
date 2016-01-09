using UnityEngine;
using System.Collections;


namespace com.dogonahorse
{
	public class WallStopOnX : MonoBehaviour {
		
		public float farLeft = -2.75f;
		public float farRight = 2.8f;

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
		public void OnMouseDrag() 
		{
			//		this.GetComponent<Rigidbody2D> ().isKinematic = false;
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
			transform.position = new Vector2 (newPos.x, transform.position.y);
		}

		
		// Update is called once per frame
		void Update () {
		}
		
	}//end class

}// end namespace