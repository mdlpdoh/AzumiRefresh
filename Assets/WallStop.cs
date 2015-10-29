using UnityEngine;
using System.Collections;

public class WallStop : MonoBehaviour {

//	public GameObject theBlock;

	public Transform block;
	Rigidbody2D r;

	// Use this for initialization
	void Start () {
//		theBlock = GameObject.Find ("practice");
		r = block.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	void OnCollisionEnter2D (Collision2D coll)
//	{
//		if (coll.transform.tag == "gold coin") {
//			Vector2 thePosOfBlock  = theBlock.transform.position;
//			theBlock.transform.position = new Vector2(thePosOfBlock.x, thePosOfBlock.y);
//
//			print ("******Hey the block hit me!!!*******");
//
//		}
//	}


	void OnMouseDrag() {
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
//		transform.position = (objPos - transform.position) * 20;
		r.velocity = (objPos - (Vector2)transform.position) *10;
		print ("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
		print (r.velocity);
		print ("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
	}

//	void OnMouseDrag() {
//		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
//		Vector2 objPos = Camera.main.ScreenToWorldPoint (mousePos);
//		r.velocity = (objPos - transform.position) *20;
//	}

}//end class
