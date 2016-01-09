using UnityEngine;
using System.Collections;


namespace com.dogonahorse
{
    public class WallStopOnXY : MonoBehaviour
    {



        public float farUp = 4.25f;
        public float farDown = 2.96f;
        public float farLeft = -2.75f;
        public float farRight = 2.8f;
        private Vector2 touchOffset;
        public DragAxis dragAxis;
        // Use this for initialization


        public void OnMouseDown()
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPos = Camera.main.ScreenToWorldPoint(mousePos);
            touchOffset = (Vector2)transform.position - objPos;
        }
        public void OnMouseDrag()
        {
            //		this.GetComponent<Rigidbody2D> ().isKinematic = false;
            Vector2 newPos = transform.position;
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPos = Camera.main.ScreenToWorldPoint(mousePos);

            if (dragAxis == DragAxis.X_Only)
            {
                // decide where to put the block.
                if (objPos.x + touchOffset.x <= farLeft)
                {
                    newPos.x = farLeft;
                }
                else if (objPos.x + touchOffset.x >= farRight)
                {
                    newPos.x = farRight;
                }
                else
                {
                    newPos.x = objPos.x + touchOffset.x;
                }
                //send the block to that position.
                transform.position = new Vector2(newPos.x , transform.position.y);
            }
            else
            {
                if (objPos.y + touchOffset.y >= farUp)
                {
                    newPos.y = farUp;
                }
                else if (objPos.y  + touchOffset.y <= farDown)
                {
                    newPos.y = farDown;
                }
                else
                {
                    newPos.y = objPos.y + touchOffset.y ;
                }
                //send the block to that position.
                transform.position = new Vector2(transform.position.x, newPos.y);

            }

        }
    }//end class

}// end namespace