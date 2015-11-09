using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

namespace com.dogOnaHorse
{
	public class UIControlActiveFilter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{

		public void OnPointerDown (PointerEventData eventData)
		{
		
			InputManager.Instance.ControlActive ();
		
		}
		public void OnPointerUp (PointerEventData eventData)
		{

	
			InputManager.Instance.ControlNotActive ();
		}

	}
}
