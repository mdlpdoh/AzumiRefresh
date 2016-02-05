using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class followPath : MonoBehaviour {

	public enum FollowType{
		MoveTowards,
//		Lerp
	}

	public FollowType Type = FollowType.MoveTowards;
	public wallMovePingPong Path;
	public float Speed = 1;
	public float MaxDistanceToGoal = .1f;

	private IEnumerator<Transform> _currentPoint;

	public void Start() {
		if (Path == null) {
			// Debug.LogError("Path cannot be null", gameObject);
			return;
		}

		_currentPoint = Path.GetPathEnumerator ();
		_currentPoint.MoveNext (); //every time we invoke MoveNext we go through the loop in wallMovement script.

		if (_currentPoint.Current == null) {
			//if there are no points in the path just break out of this...
			return;
		}
		//setting the position wto the first point in the path.
		transform.position = _currentPoint.Current.position;
	}

	public void Update() {
		if (_currentPoint == null || _currentPoint.Current == null) {
			//again checking to see if onject was given a path, if not, get out.
			return;
		}
		if (Type == FollowType.MoveTowards) {
			//current potition and target position (transform.position, _currentPoint.Current.position)
			transform.position = Vector3.MoveTowards (transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
		} 
//		else if (Type = FollowType.Lerp) {
//			transform.position = Vector3.Lerp (transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
//		}
		//are we close enough to target point 
		var distanceSquared = (transform.position - _currentPoint.Current.position).sqrMagnitude;

		if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal) {
			_currentPoint.MoveNext ();
		}
	}


}//end class
