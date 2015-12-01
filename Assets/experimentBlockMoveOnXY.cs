using UnityEngine;
using System.Collections;

public class experimentBlockMoveOnXY : MonoBehaviour {
	

//	public float farUp = 4.25f;
//	public float farDown = 2.96f;
	
	//enum
	
	private Vector3 boundsSize;
	private Vector2 touchOffset; 
	private Collider2D myCollider ; 	
//	private GameObject otherBlock;
	
	// Use this for initialization
	void Start () {
		myCollider  = GetComponent<Collider2D>();
	 boundsSize = 	 myCollider.bounds.size;
		
//		otherBlock = GameObject.Find ("Bottom black");
	}
	
	
	void OnMouseDown() 
	{
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
		touchOffset = (Vector2)transform.position - objPos;
	}
	
	void OnMouseUp() 
	{
		//			InputManager.Instance.ControlNotActive();
	}
	
	void OnMouseDrag() {
		
//		Vector2 newPos = transform.position;
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
		RaycastHit2D raycast;
		raycast =  new RaycastHit2D();
		Vector2 myDirection = ( objPos + touchOffset) - ((Vector2)transform.position );
		
		 raycast = Physics2D.BoxCast (
//						transform.position, GetComponent<Renderer> ().bounds.size, 0f, -Vector2.right, 5f
//					);			
//			Starting point of box
			//Vector2.zero,
			transform.position,
			
//			Size of the box could also be
//			new Vector2 (1,1),
			boundsSize,
//			Angle of box,
			0f,
			
//			Direction to cast
			myDirection.normalized,
			
//			Distance to cast
			10f
			
			);

			float mouseDistance = myDirection.magnitude;			
			float hitDistance = (raycast.point - (Vector2)transform.position).magnitude;
			Bounds testBounds = new Bounds();
			testBounds.center = objPos  + touchOffset;
			testBounds.size = boundsSize;
			
			//print ("testBounds " + testBounds);
			//print ("raycast.collider.bounds " + raycast.collider.bounds);
			if ((Vector2)transform.position != objPos + touchOffset && testBounds.Intersects(raycast.collider.bounds)){
				//print ("Yes");
				//test to see if there is room o move closer
				
			} else{
				//test to see if mouse is on other side of object
				
				if (hitDistance > mouseDistance) {
					//mouse is safely closer to block than any  outside colliders
						transform.position = objPos + touchOffset;
					
				} else {
					 RaycastHit2D newRay = Physics2D.Raycast(transform.position, (objPos - (Vector2)transform.position).normalized );
				  		DebugDraw.DrawMarker(newRay.point,1, Color.white, 0);
					 Debug.DrawLine((Vector2)transform.position, objPos);	
				}
			
			}
			
			//DebugDraw.DrawMarker(myCollider.bounds.ClosestPoint(objPos + touchOffset),1, Color.red, 0);
			  		DebugDraw.DrawMarker(raycast.point,1, Color.red, 0);
			Debug.DrawLine(transform.position, raycast.point, Color.red);		
			//test length			
			
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}// end class

//}//end namespace
