using UnityEngine;
using System.Collections;

namespace com.dogonahorse
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
		
	}
}
