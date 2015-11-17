using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
	public class SimpleWall : MonoBehaviour
	{


		public Transform handle01;
		public Transform handle02;
		public Transform wallSegment;
		public float fudge = 2;
		public bool Snap = true;
		public float SnapValue = 0.25f;

		void OnDrawGizmos ()
		{
			
			if (Snap) {
				snapHandle (handle01);
				snapHandle (handle02);
			}
			Vector2 handle01pos = handle01.transform.localPosition;
			Vector2 handle02pos = handle02.transform.localPosition;
			Vector2 wallSegmentPos = handle02.transform.localPosition;

			drawMarker (handle01.transform.position);
			drawMarker (handle02.transform.position);

			Vector2 vectorDirection = Vector3.Normalize (handle01pos - handle02pos);
			float angle = Vector3.Angle (vectorDirection, Vector2.up);
			wallSegment.localPosition = new Vector2 (handle01pos.x + (handle02pos.x - handle01pos.x) / 2, handle01pos.y + (handle02pos.y - handle01pos.y) / 2);
			wallSegment.localScale = new Vector2 ((handle01pos - handle02pos).magnitude * fudge, wallSegment.localScale.y);

			if (handle01pos.x < handle02pos.x) {
				wallSegment.rotation = Quaternion.Euler (0, 0, angle + 90);
			} else {
				wallSegment.rotation = Quaternion.Euler (0, 0, -angle + 90);
			}

			Vector3 difference = wallSegment.transform.position - transform.position;
			transform.position = wallSegment.transform.position;

			handle01.transform.localPosition -= difference;
			handle02.transform.localPosition -= difference;
			wallSegment.transform.localPosition -= difference;

		}

		void drawMarker (Vector2 markerLocation)
		{
			DebugDraw.DrawMarker (markerLocation, 3, Color.red, 1);
		}

		void snapHandle (Transform handle)
		{

			
			float x = handle.transform.position.x;
			float y = handle.transform.position.y;

			float remainderX = Mathf.Abs (x) % SnapValue;
			float remainderY = Mathf.Abs (y) % SnapValue;

	 

			if (remainderX < SnapValue / 2) {
				handle.position = new Vector2 ((Mathf.Abs (handle.position.x) - remainderX)* Mathf.Sign (handle.position.x), handle.position.y);
			} else {
				handle.position = new Vector2 ((Mathf.Abs (handle.position.x) + (SnapValue - remainderX)) * Mathf.Sign (handle.position.x), handle.position.y);
			}

			float newY;
			if (remainderY < SnapValue / 2) {

				 newY = (Mathf.Abs(handle.transform.position.y) - remainderY) * Mathf.Sign (handle.position.y);
				handle.position = new Vector2 (handle.position.x, newY);
			} else {

				newY =  (Mathf.Abs (handle.position.y) + (SnapValue - remainderY)) * Mathf.Sign (handle.position.y);
				handle.position = new Vector2 (handle.position.x,newY);
			}
		
		}
	}
}
