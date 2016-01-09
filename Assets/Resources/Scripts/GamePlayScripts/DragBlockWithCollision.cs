using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{
    public class DragBlockWithCollision : MonoBehaviour
    {


        //	public float farUp = 4.25f;
        //	public float farDown = 2.96f;

        //enum
        public enum DragAxis
        {
        	X_Only,
			Y_Only
        }
		
		public DragAxis dragAxis;
        private Vector3 boundsSize;
        private Vector2 touchOffset;
        private Collider2D myCollider;
        //	private GameObject otherBlock;

        // Use this for initialization
        void Start()
        {
            myCollider = GetComponent<Collider2D>();
            boundsSize = myCollider.bounds.size;

            //		otherBlock = GameObject.Find ("Bottom black");
        }


        void OnMouseDown()
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPos = Camera.main.ScreenToWorldPoint(mousePos);
            touchOffset = (Vector2)transform.position - objPos;
        }

        void OnMouseUp()
        {
            //			InputManager.Instance.ControlNotActive();
        }

        void OnMouseDrag()
        {
            //		Vector2 newPos = transform.position;
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D raycast;
            raycast = new RaycastHit2D();
            //Vector2 myDirection = (objPos + touchOffset) - ((Vector2)transform.position);
			Vector2 myDirection;
			if (dragAxis == DragAxis.X_Only){
				if (objPos.x + touchOffset.x >= transform.position.x) {					
					 myDirection = Vector2.right;
				} else {
					myDirection = Vector2.left;
				}
			} else if (dragAxis == DragAxis.Y_Only){
				if (objPos.y + touchOffset.y  >= transform.position.y) {					
					 myDirection = Vector2.up;
				} else {
					myDirection = Vector2.down;
				}
			} else {
				myDirection = Vector2.zero;
			}


            raycast = Physics2D.BoxCast(
               //			Starting point of box
        
               transform.position,

               //Size of the box 

               boundsSize,
               //Angle of box,
               0f,

               //Direction to cast
               myDirection,

               //			Distance to cast
               10f

               );

            float mouseDistance;
			if (dragAxis == DragAxis.X_Only){
				mouseDistance = Mathf.Abs(objPos.x + touchOffset.x - transform.position.x);
			} else if (dragAxis == DragAxis.Y_Only){
				mouseDistance = Mathf.Abs(objPos.y + touchOffset.y - transform.position.y);
			} else {
				mouseDistance = 0;
			}
			
            float hitDistance = (raycast.point - (Vector2)transform.position).magnitude;
            Bounds testBounds = new Bounds();
            testBounds.center = objPos + touchOffset;
            testBounds.size = boundsSize;

            //print ("testBounds " + testBounds);
            //print ("raycast.collider.bounds " + raycast.collider.bounds);
            if ((Vector2)transform.position != objPos + touchOffset && testBounds.Intersects(raycast.collider.bounds))
            {
                //print ("Yes");
                //test to see if there is room o move closer

            }
            else
            {
                //test to see if mouse is on other side of object

                if (hitDistance > mouseDistance)
                {
                    //mouse is safely closer to block than any  outside colliders
					
					
                     MoveBlock(objPos + touchOffset);

                }
                else
                {
					// MoveBlock(objPos + touchOffset);
                    RaycastHit2D newRay = Physics2D.Raycast(transform.position, (objPos - (Vector2)transform.position).normalized);
                    DebugDraw.DrawMarker(newRay.point, 1, Color.white, 0);
                    Debug.DrawLine((Vector2)transform.position, objPos);
                }

            }

            //DebugDraw.DrawMarker(myCollider.bounds.ClosestPoint(objPos + touchOffset),1, Color.red, 0);
            DebugDraw.DrawMarker(raycast.point, 1, Color.red, 0);
            Debug.DrawLine(transform.position, raycast.point, Color.red);
            //test length			


        }

        // Update is called once per frame
        void MoveBlock(Vector2 mousePosition)
        {
			if (dragAxis == DragAxis.X_Only){
				   transform.position = new Vector2(mousePosition.x, transform.position.y);
			} else if (dragAxis == DragAxis.Y_Only){
				   transform.position = new Vector2(transform.position.x, mousePosition.y);
			} else {
				//X and Y--not implmentec
			
			}
			
			
        }
		
		
		
		

    }// end class

    //}//end namespace
}