using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    /// <summary>
	/// This script lives on the Door game object and lets the event manager know when it has been hit.
	/// </summary>
	public class Door : MonoBehaviour
	{
		void OnTriggerEnter2D (Collider2D coll)
		{
			if (coll.gameObject.name == "Ball") 
            {
				EventManager.PostEvent(AzumiEventType.HitDoor, this, coll);
			}
		}
	}//end class
}//end namespace