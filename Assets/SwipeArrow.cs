using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{
	public class SwipeArrow : MonoBehaviour
	{

		private Vector3 startLocation;
		public float fudge = 2;
		private SpriteRenderer ArrowSpriteRenderer;
		private bool MainDirectionSelected = true;

		public Sprite MainSprite;
		public Sprite AltSprite;

		void Awake ()
		{


			ArrowSpriteRenderer = GetComponent<SpriteRenderer>();
		}
			// Use this for initialization
		void Start ()
		{

			EventManager.ListenForEvent (AzumiEventType.GamePress, OnGamePress);
			EventManager.ListenForEvent (AzumiEventType.GameShift, OnGameShift);
			EventManager.ListenForEvent (AzumiEventType.GameRelease, OnGameRelease);
			ArrowSpriteRenderer.enabled = false;
		}
	
		// Update is called once per frame
		public void OnGamePress (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			//print ("ArrowPivot == " +;
			if (InputManager.MainDirectionSelected){
				ArrowSpriteRenderer.sprite = MainSprite;
			} else {
				ArrowSpriteRenderer.sprite = AltSprite;
			}
			startLocation =  Camera.main.ViewportToWorldPoint( FixCoordinates((Vector3)Param));
		
			transform.position = startLocation;
		
		}
		public void OnGameShift (AzumiEventType Event_Type, Component Sender, object Param = null)
		{

			if (!ArrowSpriteRenderer.enabled) {
				ArrowSpriteRenderer.enabled = true;
			}
			var newLocation =  Camera.main.ViewportToWorldPoint( FixCoordinates((Vector3)Param));
			Vector2 vectorDirection = Vector3.Normalize (newLocation - startLocation);
			float angle = Vector3.Angle (vectorDirection, Vector2.up);
		
			if (newLocation.x < startLocation.x) {
				transform.rotation = Quaternion.Euler (0, 0, angle - 90);
			} else {
				transform.rotation = Quaternion.Euler (0, 0, -angle - 90);
			}

			if (InputManager.MainDirectionSelected){
				transform.localScale = new Vector2 ((newLocation - startLocation).magnitude * fudge, 1);
			} else {
				transform.localScale = new Vector2 ((newLocation - startLocation).magnitude * -fudge, 1);
			}
	
		
		}

		public void OnGameRelease (AzumiEventType Event_Type, Component Sender, object Param = null)
		{	
			ArrowSpriteRenderer.enabled = false;
			
		}

		//have to modify vector to include distance from camera. or 
		Vector3 FixCoordinates (Vector3 screenCoordinates){
			return new Vector3(screenCoordinates.x, screenCoordinates.y, 10);
		}
	}
}
