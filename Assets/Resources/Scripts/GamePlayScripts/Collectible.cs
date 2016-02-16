﻿using UnityEngine;
using System.Collections;

namespace com.dogonahorse
{

    public enum CollectibleType
    {
        //Level events
        Unassigned,
        Coin,
        BagOCoins,
        BoxOCoins,
        MysteryBox,
        BoostBox
    }


    public class Collectible : MonoBehaviour
    {
        public CollectibleType collectibleType;
        
        public bool Snap = true;
        public float SnapValue = 0.25f;


        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name == "Ball")
            {
                EventManager.PostEvent(AzumiEventType.HitCollectible, this, col);
            }
        }


        void OnDrawGizmos()
        {
            if (Snap)
            {
                snapCollectible();
            }
        }
        void snapCollectible()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float remainderX = Mathf.Abs(x) % SnapValue;
            float remainderY = Mathf.Abs(y) % SnapValue;

            float newX;
            if (remainderX < SnapValue / 2)
            {
                newX = (Mathf.Abs(transform.position.x) - remainderX) * Mathf.Sign(transform.position.x);
            }
            else
            {
                newX = (Mathf.Abs(transform.position.x) + (SnapValue - remainderX)) * Mathf.Sign(transform.position.x);
            }

            float newY;
            if (remainderY < SnapValue / 2)
            {
                newY = (Mathf.Abs(transform.position.y) - remainderY) * Mathf.Sign(transform.position.y);
            }
            else
            {
                newY = (Mathf.Abs(transform.position.y) + (SnapValue - remainderY)) * Mathf.Sign(transform.position.y);
            }
            transform.position = new Vector2(newX, newY);
        }



    }//end class
}