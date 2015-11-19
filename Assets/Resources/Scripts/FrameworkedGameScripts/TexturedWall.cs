﻿using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
	public class TexturedWall: MonoBehaviour
	{


		public Transform handle01;
		public Transform handle02;
		public Transform WallSegment;
		public MeshRenderer WallSegmentRenderer;
		public float fudge = 2;
		public bool Snap = true;
		public float SnapValue = 0.25f;

		public  bool DrawLines = false;
		public  float LineLength = 1f;
		public  Color LineColor = Color.red;
		public  float TextureUnitScale = 0.7f;
		public  float TextureScaleFudge = 3f;

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
			WallSegment.localPosition = new Vector2 (handle01pos.x + (handle02pos.x - handle01pos.x) / 2, handle01pos.y + (handle02pos.y - handle01pos.y) / 2);
			WallSegment.localScale = new Vector2 ((handle01pos - handle02pos).magnitude * fudge, WallSegment.localScale.y);
			WallSegmentRenderer.material.mainTextureScale= new Vector2((TextureUnitScale*TextureScaleFudge) * ((handle01pos - handle02pos).magnitude * fudge),TextureUnitScale);

		//	TextureUnitScale
			if (handle01pos.x < handle02pos.x) {
				WallSegment.rotation = Quaternion.Euler (0, 0, angle + 90);
			} else {
				WallSegment.rotation = Quaternion.Euler (0, 0, -angle + 90);
			}

			Vector3 difference = WallSegment.transform.position - transform.position;
			transform.position = WallSegment.transform.position;

			handle01.transform.localPosition -= difference;
			handle02.transform.localPosition -= difference;
			WallSegment.transform.localPosition -= difference;

		}

		void drawMarker (Vector2 markerLocation)
		{
			if (DrawLines) { 
				DebugDraw.DrawMarker (markerLocation, LineLength, LineColor, 1);
			
			}
		}

		void snapHandle (Transform handle)
		{

			
			float x = handle.transform.position.x;
			float y = handle.transform.position.y;

			float remainderX = Mathf.Abs (x) % SnapValue;
			float remainderY = Mathf.Abs (y) % SnapValue;

	 

			if (remainderX < SnapValue / 2) {
				handle.position = new Vector2 ((Mathf.Abs (handle.position.x) - remainderX) * Mathf.Sign (handle.position.x), handle.position.y);
			} else {
				handle.position = new Vector2 ((Mathf.Abs (handle.position.x) + (SnapValue - remainderX)) * Mathf.Sign (handle.position.x), handle.position.y);
			}

			float newY;
			if (remainderY < SnapValue / 2) {

				newY = (Mathf.Abs (handle.transform.position.y) - remainderY) * Mathf.Sign (handle.position.y);
				handle.position = new Vector2 (handle.position.x, newY);
			} else {

				newY = (Mathf.Abs (handle.position.y) + (SnapValue - remainderY)) * Mathf.Sign (handle.position.y);
				handle.position = new Vector2 (handle.position.x, newY);
			}
		
		}
	}
}
