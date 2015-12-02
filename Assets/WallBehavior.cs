using UnityEngine;
using System.Collections;


namespace com.dogonahorse
{
    public class WallBehavior : MonoBehaviour
    {
        // Use this for initialization

        [SerializeField]
        private int WallScoreValue = 0;

        // Update is called once per frame
        public int GetWallScoreValue()
        {
            return WallScoreValue;
        }
    }
}