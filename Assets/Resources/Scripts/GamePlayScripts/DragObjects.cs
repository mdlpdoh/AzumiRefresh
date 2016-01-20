using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{
	public class dogonahorse : MonoBehaviour
	{

		void OnMouseDrag ()
		{
			Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
			transform.position = objPos;
		}
		// 1E6153FF color for draggable objects
		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{
		
		}

	}// end class
}