using UnityEngine;
using System.Collections;

public class experimentBlockMoveOnXY : MonoBehaviour {
	

	public float farUp = 4.25f;
	public float farDown = 2.96f;
	
	
	private RaycastHit2D thisBlock;
	//		private GameObject otherBlock;
	
	private GameObject otherBlock;
	
	// Use this for initialization
	void Start () {
		
		otherBlock = GameObject.Find ("Bottom black");
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
//		// decide where to put the block.
//		if (objPos.y >= farUp) {
//			newPos.y = farUp;
//		}  else if (objPos.y <= farDown) {
//			newPos.y = farDown;
//		}  else {
//			newPos.y = objPos.y;
//		}
		//send the block to that position.
		transform.position = new Vector2 (transform.position.x, objPos.y);
		
		print (otherBlock.transform.position);
		
		if (this.GetComponent<Renderer> ().bounds.Intersects (otherBlock.GetComponent<Renderer> ().bounds)) {
			print ("I INTERSECTED GADDAMMMITTTTTT");
		}
		
		RaycastHit2D raycast = Physics2D.BoxCast (
			
			//Starting point of box
			Vector2.zero,
			
			//Size of the box could also be
			//new Vector2 (1,1),
			GetComponent<Renderer> ().bounds.size,
			//Angle of box,
			0f,
			
			//Direction to cast
			Vector2.up,
			
			//Distance to cast
			5f
			
			);
		
			print (GetComponent<Renderer> ().bounds.size);
			if(raycast.collider.tag == "Player"){
				print("I Hit with my ray.");
				Debug.DrawRay(transform.position, raycast.point);
			}else{
				print("I dDI NOT HOT IT");
			}
		
		
		
		
		//			RaycastHit2D hit;
		//			Ray2D blockRay = new Ray2D (transform.position, Vector2.up);
		//			Vector2 newPos = transform.position;
		//			Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		//			Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
		
		//get direction for boxcasting
		//			Vector2 thedir = (newPos - objPos).normalized;
		
		// decide where to put the block.
		//			if (objPos.y >= farUp) {
		//				newPos.y = farUp;
		//			}  else if (objPos.y <= farDown) {
		//				newPos.y = farDown;
		//			}  else {
		//				newPos.y = objPos.y;
		//			}
		//send the block to that position.
		//			transform.position = new Vector2 (transform.position.x, objPos.y);
		
		//boxcasting
		
		//			float distance = Mathf.Infinity; 
		//			float angle = 0f;
		//			Vector2.Angle (thedir, transform.forward); 
		//			int layerMask = 1;
		//			float minDepth = -Mathf.Infinity; 
		//			float maxDepth = Mathf.Infinity;
		//size	    GetComponent<Renderer> ().bounds.size
		
		//			Debug.DrawLine(transform.position, blockRay.point);
		//			thisBlock = Physics2D.BoxCast(transform.position, angle, thedir, distance);
		//			if (Physics2D.Raycast(blockRay, out hit, 3f)) {
		//				print ("YES THIS IS A HIT!!!!!!");
		//			}	else{
		//				print ("NOOOOOOOOOO not a hit!!!!!!!!!!");
		//			}
		//			 
		
		//			print (GetComponent<Renderer> ().bounds.size);
		//			print (thedir);
		//			print (thisBlock.transform);
		//		    print ("THIS IS THE OBJECT POSITIOPN!!!!!!" + objPos);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}// end class

//}//end namespace
