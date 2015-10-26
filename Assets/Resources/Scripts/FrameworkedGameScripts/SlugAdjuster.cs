using UnityEngine;
using System.Collections;

public class SlugAdjuster : MonoBehaviour {


	public Transform Slug01;
	public Transform Slug02;
	public Transform Slug03;
	public float fudge = 2;

	// Use this for initialization

	
	// Update is called once per frame
	void OnDrawGizmos () {

		Vector3 slug01pos  = Slug01.transform.localPosition;
		Vector3 slug02pos = Slug02.transform.localPosition;
		Vector3 slug03pos = Slug03.transform.localPosition;
		Slug02.transform.localPosition = new Vector3(slug01pos.x + (slug03pos.x-slug01pos.x)/2, slug02pos.y, slug02pos.z);
		Slug02.transform.localScale = new Vector3 ( (slug03pos.x-slug01pos.x) * fudge, 1, 1);

	}
}
