using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{
	public class Door : MonoBehaviour
	{
		void OnTriggerEnter2D (Collider2D coll)
		{
			if (coll.gameObject.name == "Ball") {
				EventManager.PostEvent(AzumiEventType.HitDoor, this, coll);
				//Destroy (coll.gameObject);
				//print ("Ball has been destroyed");
				//GameManager.ReturnToProgressScreen ();
			}
		}

	}//end class
}