using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{
    public class DragQuad : MonoBehaviour
    {

        //Handle references
        public Transform handle00;
        public Transform handle01;
        public Transform handle10;
        public Transform handle11;
        public Transform wallSegment;


        public Transform wallStripes;

        public Transform TabTop;

        public Transform TabBottom;
        public Transform TabQuad;

        public Transform TabDots;

   //     private float handlePositionDifference;


        public float fudge = 2;
        public bool Snap = true;
        public float SnapValue = 0.25f;

        public bool DrawLines = false;
        public float LineLength = 1f;
        public Color LineColor = Color.red;


        public float TextureUnitScale = 0.7f;
        public float StripeTextureScaleFudge = 3f;
        private Material stripeMaterialInstance = null;
        public MeshRenderer StripeSegmentRenderer;
        public Vector3 stripeYOffset;
        
        public float tabDotsTextureScaleFudge = 3f;
        private Material tabDotsMaterialInstance;
        public MeshRenderer tabDotsSegmentRenderer;
        public Vector3 tabDotsYOffset;
        
        public Vector4 normalColor;
        public Vector4 highlightColor;
     
        
        
        
        
  		void Start()
		{
              normalColor = wallSegment.gameObject.GetComponent<MeshRenderer>().material.color;
         
		}
  		public void OnMouseDown() 
		{
            wallSegment.gameObject.GetComponent<MeshRenderer>().material.color = highlightColor;
		}
		public void OnMouseUp() 
		{
			    wallSegment.gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
	
		}
        void OnDrawGizmos()
        {

            if (stripeMaterialInstance == null)
            {
                stripeMaterialInstance = new Material(StripeSegmentRenderer.sharedMaterial);
                StripeSegmentRenderer.material = stripeMaterialInstance;
            }
        
            if (tabDotsMaterialInstance == null)
            {
                tabDotsMaterialInstance = new Material(tabDotsSegmentRenderer.sharedMaterial);
                tabDotsSegmentRenderer.material = tabDotsMaterialInstance;
            }
            if (Snap)
            {
                snapHandle(handle00);
                snapHandle(handle10);
                snapHandle(handle01);
                snapHandle(handle11);
            }
            AdjustHandles();
            Vector2 handle00pos = handle00.transform.localPosition;
            Vector2 handle01pos = handle01.transform.localPosition;
            Vector2 handle10pos = handle10.transform.localPosition;
//            Vector2 handle11pos = handle11.transform.localPosition;


            drawMarker(handle00.transform.position);
            drawMarker(handle01.transform.position);
            drawMarker(handle10.transform.position);
            drawMarker(handle11.transform.position);


            wallSegment.localPosition = new Vector2(handle00pos.x + (handle01pos.x - handle00pos.x) / 2, handle00pos.y + (handle10pos.y - handle00pos.y) / 2);
            wallSegment.localScale = new Vector2((handle01pos - handle00pos).magnitude * fudge, (handle10pos - handle00pos).magnitude * fudge);
            wallStripes.localPosition = wallSegment.localPosition;
            wallStripes.localScale = wallSegment.localScale - (stripeYOffset * fudge);
            TabQuad.localScale = new Vector2(TabQuad.localScale.x, wallStripes.localScale.y);
            TabDots.localScale = new Vector2(TabDots.localScale.x, wallStripes.localScale.y);
           // stripeMaterialInstance.mainTextureScale = new Vector2(TextureUnitScale, (TextureUnitScale * StripeTextureScaleFudge) * ((handle10pos - handle00pos).magnitude * fudge));
            
            stripeMaterialInstance.mainTextureScale = new Vector2((TextureUnitScale * StripeTextureScaleFudge) * ((handle01pos - handle00pos).magnitude * fudge), (TextureUnitScale * StripeTextureScaleFudge) * ((handle10pos - handle00pos).magnitude * fudge));          
            
            tabDotsMaterialInstance.mainTextureScale = new Vector2(1, (TextureUnitScale * tabDotsTextureScaleFudge) * ((handle10pos - handle00pos).magnitude * fudge));
            Vector3 difference = wallSegment.transform.position - transform.position;
            transform.position = wallSegment.transform.position;


            handle00.localPosition -= difference;
            handle01.localPosition -= difference;
            handle10.localPosition -= difference;
            handle11.localPosition -= difference;
            wallSegment.localPosition -= difference;
            wallStripes.localPosition = wallSegment.localPosition;
//            handlePositionDifference = (handle00.position - transform.position).magnitude;
            TabTop.position = handle01.position;
            TabBottom.position = handle11.position;
            TabQuad.position = new Vector3(handle01.position.x, TabQuad.position.y, -1);
              TabDots.position = new Vector3(handle01.position.x - 0.2f, TabQuad.position.y, -1);
        }

        void drawMarker(Vector2 markerLocation)
        {
            if (DrawLines)
            {
                DebugDraw.DrawMarker(markerLocation, LineLength, LineColor,0);
            }
        }
        void AdjustHandles()
        {
            Transform activeHandle = handle00;
            int handleNum = 0;
            float averageMagnitude = ((handle00.position - transform.position).magnitude +
            (handle01.position - transform.position).magnitude +
            (handle10.position - transform.position).magnitude +
            (handle11.position - transform.position).magnitude) / 4;

            float maxMagnitudeDifference = Mathf.Abs((handle00.position - transform.position).magnitude - averageMagnitude);



            if (Mathf.Abs((handle01.position - transform.position).magnitude - averageMagnitude) > maxMagnitudeDifference)
            {
                handleNum = 1;
                maxMagnitudeDifference = Mathf.Abs((handle01.position - transform.position).magnitude - averageMagnitude);
                activeHandle = handle01;
            }
            if (Mathf.Abs((handle10.position - transform.position).magnitude - averageMagnitude) > maxMagnitudeDifference)
            {
                handleNum = 2;
                maxMagnitudeDifference = Mathf.Abs((handle10.position - transform.position).magnitude - averageMagnitude);
                activeHandle = handle10;
            }
            if (Mathf.Abs((handle11.position - transform.position).magnitude - averageMagnitude) > maxMagnitudeDifference)
            {
                handleNum = 3;
                maxMagnitudeDifference = (handle11.position - transform.position).magnitude;
                activeHandle = handle11;
            }

            switch (handleNum)
            {
                case 0:
                    //handle 00
                    handle01.position = new Vector2(handle01.position.x, activeHandle.position.y);
                    handle10.position = new Vector2(activeHandle.position.x, handle10.position.y);
                    break;
                case 1:
                    //handle 01
                    handle00.position = new Vector2(handle00.position.x, activeHandle.position.y);
                    handle11.position = new Vector2(activeHandle.position.x, handle11.position.y);
                    break;
                case 2:
                    //handle 10
                    handle00.position = new Vector2(activeHandle.position.x, handle00.position.y);
                    handle11.position = new Vector2(handle11.position.x, activeHandle.position.y);
                    break;
                case 3:
                    //handle 11
                    handle01.position = new Vector2(activeHandle.position.x, handle01.position.y);
                    handle10.position = new Vector2(handle10.position.x, activeHandle.position.y);
                    break;


            }

        }



        void snapHandle(Transform handle)
        {
            float x = handle.transform.position.x;
            float y = handle.transform.position.y;

            float remainderX = Mathf.Abs(x) % SnapValue;
            float remainderY = Mathf.Abs(y) % SnapValue;
            if (remainderX < SnapValue / 2)
            {
                handle.position = new Vector2((Mathf.Abs(handle.position.x) - remainderX) * Mathf.Sign(handle.position.x), handle.position.y);
            }
            else
            {
                handle.position = new Vector2((Mathf.Abs(handle.position.x) + (SnapValue - remainderX)) * Mathf.Sign(handle.position.x), handle.position.y);
            }

            float newY;
            if (remainderY < SnapValue / 2)
            {

                newY = (Mathf.Abs(handle.transform.position.y) - remainderY) * Mathf.Sign(handle.position.y);
                handle.position = new Vector2(handle.position.x, newY);
            }
            else
            {

                newY = (Mathf.Abs(handle.position.y) + (SnapValue - remainderY)) * Mathf.Sign(handle.position.y);
                handle.position = new Vector2(handle.position.x, newY);
            }

        }
    }
}
