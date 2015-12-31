using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{
    public enum DragAxis
    {
        X_Only,
        Y_Only
    }
    public class DragBlockWithCollision2 : MonoBehaviour
    {
		public DragAxis dragAxis;
        private Vector3 boundsSize;
        private Vector2 touchOffset;
        private Collider2D myCollider;
        // Use this for initialization
		 private Rigidbody2D rigidBody;
		
        void Start()
        {
            myCollider = GetComponent<Collider2D>();
			rigidBody = GetComponent<Rigidbody2D>();
           // boundsSize = myCollider.bounds.size;
        }

       void OnMouseDown()
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPos = Camera.main.ScreenToWorldPoint(mousePos);
            touchOffset = (Vector2)transform.position - objPos;
        }
		     void OnMouseUp()
        {
           rigidBody.velocity = Vector2.zero;
        }
		
		 void OnMouseDrag()
        	{
			Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPos = Camera.main.ScreenToWorldPoint(mousePos);
			if (dragAxis == DragAxis.X_Only){
				float xVelocity  = (objPos.x + touchOffset.x - transform.position.x) *10; 
				rigidBody.velocity = new Vector2(xVelocity, 0);
			} else if (dragAxis == DragAxis.Y_Only){
			float yVelocity  = (objPos.y + touchOffset.y - transform.position.y) *10; 
						rigidBody.velocity = new Vector2(0, yVelocity);
			} else {
				//X and Y--not implmentec
			
			}
		}
    }
}
