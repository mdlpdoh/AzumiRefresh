using UnityEngine;

using System.Collections.Generic;

namespace com.dogonahorse
{
    /// <summary>
    /// This script is on the AddPointsWall_Quad and SubtractPointsWall_Quad prefab game objects.
    /// It makes it possible to stretch, to position the walls and put an identifying sprite on it.
    /// </summary>
    public class LayeredSpriteWall : MonoBehaviour
    {

        public Transform handle01;
        public Transform handle02;
        public Transform PrimaryWallSegment;
        public Transform SecondaryWallSegment;
        public float fudge = 2;
        public bool Snap = true;
        public float SnapValue = 0.25f;

        public bool DrawLines = false;
        public float LineLength = 1f;
        public Color LineColor = Color.red;

        public int shapesPerWU = 2;
		
        public GameObject HeartsContainer;
        public GameObject SpritePrefab;
        //private Material materialInstance;
        public bool SpritesNeedAdjustment = false;

        public List<Transform> spritesInWall = new List<Transform>();


        void OnDrawGizmos()
        {
            if (Snap)
            {
                snapHandle(handle01);
                snapHandle(handle02);
            }
            Vector2 handle01pos = handle01.transform.localPosition;
            Vector2 handle02pos = handle02.transform.localPosition;
      

            drawMarker(handle01.transform.position);
            drawMarker(handle02.transform.position);

            Vector2 vectorDirection = Vector3.Normalize(handle01pos - handle02pos);
            float angle = Vector3.Angle(vectorDirection, Vector2.up);
            PrimaryWallSegment.localPosition = new Vector2(handle01pos.x + (handle02pos.x - handle01pos.x) / 2, handle01pos.y + (handle02pos.y - handle01pos.y) / 2);
            PrimaryWallSegment.localScale = new Vector2((handle01pos - handle02pos).magnitude * fudge, PrimaryWallSegment.localScale.y);

            float wallOrientation;

            if (handle01pos.x < handle02pos.x)
            {
                wallOrientation = angle + 90;
            }
            else
            {
                wallOrientation = -angle + 90;
            }
            PrimaryWallSegment.rotation = Quaternion.Euler(0, 0, wallOrientation);
            Vector3 difference = PrimaryWallSegment.transform.position - transform.position;
            transform.position = PrimaryWallSegment.transform.position;

            handle01.transform.localPosition -= difference;
            handle02.transform.localPosition -= difference;
            PrimaryWallSegment.transform.localPosition -= difference;
  

            SecondaryWallSegment.localPosition = PrimaryWallSegment.localPosition;
            SecondaryWallSegment.localScale = PrimaryWallSegment.localScale;
            SecondaryWallSegment.rotation = PrimaryWallSegment.rotation;
            int correctSpriteNumber = HowManySprites(PrimaryWallSegment.localScale.x);
;
            adjustNumberOfSprites(correctSpriteNumber);
           
            if (SpritesNeedAdjustment)
            {
                AdjustSpritePlacement();
            }
            checkForExcessSprites(correctSpriteNumber);
        }
        void AdjustSpritePlacement()
        {
            int numberOfSprites = spritesInWall.Count;

			float gapLength = 1.0f/shapesPerWU;
			
			Vector3 vectorDirection = Vector3.Normalize(handle01.transform.position - handle02.transform.position);
			float startOffset =  (PrimaryWallSegment.localScale.x - (gapLength * (numberOfSprites-1)))/2;
		
			Vector3 startPosition = handle01.transform.position - startOffset * vectorDirection;
			
            for (int i = 0; i < numberOfSprites; i++) 
            {
                spritesInWall[i].position = startPosition - vectorDirection * gapLength * i;			
			}

        }
        
        void checkForExcessSprites(int numberOfSprites)
        {
               SpriteRenderer[] AllSpriteChildren = HeartsContainer.GetComponentsInChildren<SpriteRenderer>();
               if (AllSpriteChildren.Length != numberOfSprites){
                   //this is bad so the sprites need to be cleaned up
                   
                   spritesInWall.Clear();
                   for (int i=0; i< AllSpriteChildren.Length; i++) {
                       GameObject.DestroyImmediate (AllSpriteChildren[i].gameObject);
                   }
               }
        }
        
        void adjustNumberOfSprites(int numberOfSprites)
        {
            int i;
            int difference = numberOfSprites - spritesInWall.Count;
            if (difference > 0)
            {
                //instantiate new Sprites and add to spritesInWall list
                for (i = 0; i < difference; i++)
                {
                    GameObject newSprite = Instantiate(SpritePrefab);
                    spritesInWall.Add(newSprite.transform);
					newSprite.transform.parent = HeartsContainer.transform;
                }
            }
            else if (difference < 0)
            {
                for (i = 0; i > difference; i--)
                {
                    GameObject extraSprite = spritesInWall[spritesInWall.Count - 1].gameObject;
                    spritesInWall.RemoveAt(spritesInWall.Count - 1);
                    GameObject.DestroyImmediate(extraSprite);
                }
            }


        }

        int HowManySprites(float wallLength)
        {
            return Mathf.FloorToInt(wallLength * shapesPerWU);
        }

        void drawMarker(Vector2 markerLocation)
        {
            if (DrawLines)
            {
                DebugDraw.DrawMarker(markerLocation, LineLength, LineColor, 1);

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
    }// end class
}// end namespace
