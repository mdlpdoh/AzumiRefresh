using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

namespace com.dogonahorse
{
	public class UISpriteBackGroundFilter : MonoBehaviour
	{

		public void OnMouseDown ()
		{

			InputManager.Instance.ControlActive ();
		
		}
		public void OnMouseUp ()
		{

	
			InputManager.Instance.ControlNotActive ();
		}

	}
}
