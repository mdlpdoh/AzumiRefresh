using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    public class followPath : MonoBehaviour
    {
        /// <summary>
        /// This script is used for the moving walls and is used with the wallMovePingPong script.
        /// We put this script on the Prefab called AutoMoveWallAlongAxis. Now drop the Platform Path Object (or any game object that has
        /// the wallMovePingPong script on it) onto the public 'Path' in the script component in the inspector.
        /// </summary>

        public enum FollowType
        {
            MoveTowards,
            //we can add different movement types.
            //Lerp 
        }
        public FollowType Type = FollowType.MoveTowards;
        public wallMovePingPong Path;
        public float Speed = 1;
        public float MaxDistanceToGoal = .1f;
        private IEnumerator<Transform> _currentPoint;

        public void Start()
        {
            if (Path == null)
            {
                // Debug.LogError("Path cannot be null", gameObject);
                return;
            }

            _currentPoint = Path.GetPathEnumerator();
            _currentPoint.MoveNext(); //every time we invoke MoveNext we go through the loop in wallMovement script.

            if (_currentPoint.Current == null)
            {
                //if there are no points in the path just break out of this...
                return;
            }
            //setting the position to the first point in the path.
            transform.position = _currentPoint.Current.position;
        }

        public void Update()
        {
            if (_currentPoint == null || _currentPoint.Current == null)
            {
                //again checking to see if object was given a path, if not, get out.
                return;
            }
            if (Type == FollowType.MoveTowards)
            {
                //current potition and target position (transform.position, _currentPoint.Current.position)
                transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
            }
            //else if (Type = FollowType.Lerp) 
            //{
            //transform.position = Vector3.Lerp (transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
            //}
            //are we close enough to target point 
            var distanceSquared = (transform.position - _currentPoint.Current.position).sqrMagnitude;

            if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
            {
                _currentPoint.MoveNext();
            }
        }

    }//end class
}//end namespace
