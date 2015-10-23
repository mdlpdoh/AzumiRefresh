using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{

	public enum CollectibleType 	{
		//Level events
		Unassigned,
		Coin,
		BagOCoins,
		BoxOCoins,
		MysteryBox,
		BoostBox
	}
	public class Collectible : MonoBehaviour
	{
		public CollectibleType collectibleType;
		// Use this for initialization
		void Start ()
		{
			
		}
		
		// Update is called once per frame
		
		void OnTriggerEnter2D (Collider2D col)
		{
			if (col.gameObject.name == "Ball") {
				Destroy (this.gameObject);
				//print ("Coin has been pocketed");
				EventManager.PostEvent(AzumiEventType.HitCollectible, this, collectibleType);
			}
		}
		
	}//end class
}