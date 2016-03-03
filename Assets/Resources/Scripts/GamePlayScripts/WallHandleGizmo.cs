using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    /// <summary>
    /// This script goes on the handle game object that is a child of the SimpleWall_Quad Prefab.
    /// This enables Snap to a grid ability.
    /// </summary>
    public class WallHandleGizmo : MonoBehaviour
    {

        public bool Snap
        {
            get
            {
                return snap;
            }
            set
            {
                snap = value;
            }
        }

        public float SnapValue
        {
            get
            {
                return snapValue;
            }
            set
            {
                snapValue = value;
            }
        }

        private bool snap = true;
        private float snapValue = 0.25f;

        void Start()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

    }//end class
}//end namespace
