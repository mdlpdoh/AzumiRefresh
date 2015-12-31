using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.dogonahorse
{


    public class SoundManager : MonoBehaviour
    {

        private static SoundManager instance = null;
        //private List<LevelPlayerData> LevelPlayerList = new List<LevelPlayerData> ();
       
        public static SoundManager Instance
        {
            // return reference to private instance 
            get
            {
                return instance;
            }
        }

  void Awake()
        {
            // Check if existing instance of class exists in scene 35 
            // If so, then destroy this instance
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            // Make this active and only instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
     
    }
}