using UnityEngine;
using System.Collections;

namespace com.dogOnaHorse
{
	public class WallHandleGizmo : MonoBehaviour
	{

		public bool Snap {
			get { 
				return snap; 
			}
			set {
				snap = value; 
			}
		}
		
		public float SnapValue {
			get { 
				return snapValue; 
			}
			set {
				snapValue = value; 
			}
		}

		private bool snap = true;
		private float snapValue = 0.25f;

		void Start ()
		{
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
			spriteRenderer.enabled = false;
		}
		/*
		void OnDrawGizmos ()
		{

			if (snap) {

				float x = transform.position.x;
				float y = transform.position.y;
				float remainderX = x % snapValue;
				float remainderY = y % snapValue;
				if (remainderX < snapValue / 2) {
					transform.position = new Vector2(transform.position.x - remainderX, transform.position.y);
				} else {
					transform.position = new Vector2(transform.position.x + (snapValue - remainderX), transform.position.y);
				}
				if (remainderY < snapValue / 2) {
					transform.position = new Vector2(transform.position.x, transform.position.y- remainderY);
				} else {
					transform.position = new Vector2(transform.position.x, transform.position.y +  (snapValue - remainderY));
				}
			}
		}*/
	}
}
