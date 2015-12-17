﻿using UnityEngine;
using System.Collections;


namespace com.dogonahorse
{
    public class WallBehavior : MonoBehaviour
    {
        // Use this for initialization

        [SerializeField]
        private int WallScoreValue = 0;

        [SerializeField]
        private int MaxNumberOfActivations = 5;

        private int remainingActivations;
        private bool wallIsActive = true;
        // Update is called once per frame

        void Start()
        {
            remainingActivations = MaxNumberOfActivations;
        }

        public int GetWallScoreValue()
        {
            if (wallIsActive && remainingActivations > 1)
            {
                remainingActivations--;
                return WallScoreValue;
            }
            else if (wallIsActive && remainingActivations == 1)
            {
                KillWallBehavior();
                wallIsActive = false;
                remainingActivations--;
                return WallScoreValue;
            }
            else
            {
                return 0;
             }
        }
        
         void KillWallBehavior(){
             
             
             
             
         }
         
         
         
    }
}