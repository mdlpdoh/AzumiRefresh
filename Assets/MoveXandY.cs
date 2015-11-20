using UnityEngine;
using System.Collections;


//namespace com.dogonahorse
//{
public class MoveXandY: MonoBehaviour {
		
//		public float farUp = 4.25f;
//		public float farDown = 2.96f;
		
		private RaycastHit2D thisBlock;
//		private GameObject otherBlock;
		
		
		// Use this for initialization
		void Start () {

//			otherBlock = GameObject.Find ("Movable Top Lef");
			
		}
		
		
		void OnMouseDown() 
		{
//			InputManager.Instance.ControlActive();
		Vector2 oldPosition = transform.position;
		print (oldPosition);
		}
		
		void OnMouseUp() 
		{
//			InputManager.Instance.ControlNotActive();
		}
		
		void OnMouseDrag() {

			Vector2 newPos = transform.position;
			Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);

			//get direction for boxcasting
			Vector2 thedir = (newPos - objPos).normalized;

			// decide where to put the block.
//			if (objPos.y >= farUp) {
//				newPos.y = farUp;
//			} else if (objPos.y <= farDown) {
//				newPos.y = farDown;
//			} else {
//				newPos.y = objPos.y;
//			}
			//send the block to that position.
//			transform.position = new Vector2 (transform.position.x, objPos.y);
			
			//boxcasting
			float distance = Mathf.Infinity; 
			float angle = 0f;
//			Vector2.Angle (thedir, transform.forward); 
			int layerMask = 1;
			float minDepth = -Mathf.Infinity; 
			float maxDepth = Mathf.Infinity;

			thisBlock = Physics2D.BoxCast(transform.position, GetComponent<Renderer> ().bounds.size, angle, thedir, distance);
			
			print (GetComponent<Renderer> ().bounds.size);
			print (thedir);
			print (thisBlock.transform);
//		    print ("THIS IS THE OBJECT POSITIOPN!!!!!!" + objPos);

		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
	}// end class
	
//}//end namespace