using UnityEngine;
using System.Collections;


//namespace com.dogonahorse
//{
public class MoveXandY: MonoBehaviour {

		public GameObject otherBlock;
//		public GameObject wallBlock;

		// Use this for initialization
		void Start () {

		}
		
		void OnMouseDown() 
		{
//			InputManager.Instance.ControlActive();
			Vector2 oldPosition = transform.position;
//			print (oldPosition);
		}
		
		void OnMouseUp() 
		{
//			InputManager.Instance.ControlNotActive();
		}
		
		void OnMouseDrag() {

			Vector2 newPos = transform.position;
			Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);

			//send the block to that position.
			transform.position = new Vector2 (transform.position.x, objPos.y);

			print (otherBlock.transform.position);

		if (this.GetComponent<Renderer> ().bounds.Intersects (otherBlock.GetComponent<Renderer> ().bounds) ) {
			    //or statement also works:  | this.GetComponent<Renderer> ().bounds.Intersects (wallBlock.GetComponent<Renderer> ().bounds)
				print ("I INTERSECTED *********");
				//move block to previous position
				transform.position = new Vector2 (newPos.x, newPos.y);
			}

		}	
		// Update is called once per frame
		void Update () {
			
		}
		
	}// end class
	
//}//end namespace