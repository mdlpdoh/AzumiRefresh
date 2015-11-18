using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
	public class ForceArrowRotater : MonoBehaviour
	{
		


		public SpriteRenderer ArrowSpriteRenderer;

		public SpriteRenderer[] ArrowLights;


		private bool MainDirectionSelected = true;

		public float MaxFlickerInterval;
		public float MinFlickerInterval;
		private float currentFlickerInterval;
		public float MaxPullDistance;

		private Vector3 startLocation;
		private Vector3 currentLocation;

		private float lastFlickerTime = 0;
		private int lastFlickerSpriteNum = 0;
		private int maxFlickerSpriteNum = 0;
		
		// Use this for initialization
		void Start ()
		{
			
			EventManager.ListenForEvent (AzumiEventType.GamePress, OnGamePress);
			EventManager.ListenForEvent (AzumiEventType.GameShift, OnGameShift);
			EventManager.ListenForEvent (AzumiEventType.GameRelease, OnGameRelease);

			ArrowSpriteRenderer.enabled = false;
			maxFlickerSpriteNum = ArrowLights.Length-1;
			print("StartArrowSpriteRenderer " +  ArrowSpriteRenderer );
		}
		
		// Update is called once per frame
		//public void OnGamePress (AzumiEventType Event_Type, Component Sender, object Param = null)
		//{
			//print ("ArrowPivot == " +;
			//if (InputManager.MainDirectionSelected){
			//	ArrowSpriteRenderer.sprite = MainSprite;
		//	} else {
			//	ArrowSpriteRenderer.sprite = AltSprite;
			//}
		//	startLocation =  Camera.main.ViewportToWorldPoint( FixCoordinates((Vector3)Param));
			
			//transform.position = startLocation;
			
		//}
		void Update ()
		{
			if (ArrowSpriteRenderer.enabled) {
				Flicker();
			}
		}
		void Flicker () {

			if (Time.time > lastFlickerTime + currentFlickerInterval){

				ArrowLights[lastFlickerSpriteNum].enabled = false;
				if (lastFlickerSpriteNum >= maxFlickerSpriteNum){
					lastFlickerSpriteNum = 0;
				} else {
					lastFlickerSpriteNum++;
				}
				ArrowLights[lastFlickerSpriteNum].enabled = true;

				lastFlickerTime = lastFlickerTime + currentFlickerInterval;
			}

			}
		// Update is called once per frame
		public void OnGamePress (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			//print ("ArrowPivot == " +;
	
			startLocation =  (Vector3)Param;
			lastFlickerTime = Time.time;

			
		}
		public void OnGameShift (AzumiEventType Event_Type, Component Sender, object Param = null)
		{
			//print("ArrowSpriteRenderer " +  ArrowSpriteRenderer );
			if (!ArrowSpriteRenderer.enabled) {
				ArrowSpriteRenderer.enabled = true;
			}
			SetMarkerRotation((Vector3)Param);
			CalculateFlickerInterval();
				/*
			if (InputManager.MainDirectionSelected){
				transform.localScale = new Vector2 ((newLocation - startLocation).magnitude * fudge, 1);
			} else {
				transform.localScale = new Vector2 ((newLocation - startLocation).magnitude * -fudge, 1);
			}
*/
			
		}
		void SetMarkerRotation(Vector3 shiftLocation){
			var newLocation = shiftLocation;
			Vector2 vectorDirection = Vector3.Normalize (newLocation - startLocation);
			float angle = Vector3.Angle (vectorDirection, Vector2.up);
			
			if (InputManager.MainDirectionSelected){
				if (newLocation.x < startLocation.x) {
					transform.rotation = Quaternion.Euler (0, 0, angle - 90);
				} else {
					transform.rotation = Quaternion.Euler (0, 0, -angle - 90);
				}
			} else {
				
				if (newLocation.x < startLocation.x) {
					transform.rotation = Quaternion.Euler (0, 0, angle + 90);
				} else {
					transform.rotation = Quaternion.Euler (0, 0, -angle + 90);
				}
				
			}
			
			currentLocation = newLocation;
		}
			
		void CalculateFlickerInterval(){
			float forceNormal;
			float currentMagnitude = (currentLocation - startLocation).magnitude;
			if (currentMagnitude >= MaxPullDistance){
				forceNormal = 1f;
			} else {
				forceNormal = currentMagnitude/MaxPullDistance;
			}
			currentFlickerInterval = MaxFlickerInterval - ((MaxFlickerInterval - MinFlickerInterval) * forceNormal);
			//print ("currentLocation " + currentLocation );
			//print ("    startLocation " +startLocation );
		}



		public void OnGameRelease (AzumiEventType Event_Type, Component Sender, object Param = null)
		{	
			ArrowSpriteRenderer.enabled = false;

			for (int i = 0; i <= maxFlickerSpriteNum; i++){
				ArrowLights[i].enabled = false;
			}
			
		}
		

	}
}
