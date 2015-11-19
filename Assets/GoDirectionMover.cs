using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
public class GoDirectionMover : MonoBehaviour 
	{
	public Transform DirectionalArrow;
	public Transform MoverIcon;

	
	
	void Start(){
		DirectionalArrow.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	void OnTriggerEnter2D (Collider2D col)
		{
			
			if (col.gameObject.name == "Ball") {
				
				
				col.GetComponent<Rigidbody2D>().velocity = col.gameObject.GetComponent<AzumiBallRoll>().MaximumVelocity * Vector3.Normalize (DirectionalArrow.position - col.transform.position) ;

			}

		}
		
		
		void OnDrawGizmos ()
		{
			
			Debug.DrawLine(transform.position, DirectionalArrow.position, Color.red);
			Vector2 vectorDirection = Vector3.Normalize (DirectionalArrow.position - transform.position);
			float angle = Vector3.Angle (vectorDirection, Vector2.up);
			if (DirectionalArrow.position.x < transform.position.x) {
				DirectionalArrow.rotation = Quaternion.Euler (0, 0, angle + 90);
				MoverIcon.rotation = Quaternion.Euler (0, 0, angle + 90);
			} else {
				DirectionalArrow.rotation = Quaternion.Euler (0, 0, -angle + 90);
				MoverIcon.rotation = Quaternion.Euler (0, 0, -angle + 90);
			}
			MoverIcon.localPosition = new Vector2(0,0);
		}
	}
}
