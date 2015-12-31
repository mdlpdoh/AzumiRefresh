using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    public class DragBoxMouseDown : MonoBehaviour
    {
		private WallStopOnXY parentMoveScript;
		private DragQuad parentGraphicScript;
		void Start(){
			
			parentMoveScript = transform.root.GetComponent<WallStopOnXY>();
			parentGraphicScript = transform.root.GetComponent<DragQuad>();			
		}
		
		
	
  		void OnMouseDown() 
		{
			InputManager.Instance.ControlActive();
			parentGraphicScript.OnMouseDown();
			parentMoveScript.OnMouseDown();
		}
		void OnMouseUp() 
		{
			
			InputManager.Instance.ControlNotActive();
				parentGraphicScript.OnMouseUp();
		}
		void OnMouseDrag() 
		{
			
			parentMoveScript.OnMouseDrag();
			
			
		}
		
		
		
    }
}
