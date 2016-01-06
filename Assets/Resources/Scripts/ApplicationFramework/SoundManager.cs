using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.dogonahorse
{

    public enum AudioActionType

    {
        unassigned,
        HardStart,
        HardStop,
        FadeIn,
        FadeOut,
        FadingDrone
    }
    public class SoundManager : MonoBehaviour
    {

        private static SoundManager instance = null;
        //private List<LevelPlayerData> LevelPlayerList = new List<LevelPlayerData> ();

        private bool musicEnabled = true;
        private bool moundFXEnabled = true;

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

        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.EnterTitle, OnEnterTitle);
            EventManager.ListenForEvent(AzumiEventType.EnterProgress, OnEnterProgress);
            EventManager.ListenForEvent(AzumiEventType.EnterLevel, OnEnterLevel);
        }




        void OnEnterTitle(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
                       print ("***********************************OnEnterTitle");
            if (musicEnabled){
               AudioEventManager.PostEvent(AudioEventType.MainThemeHardStart, this);
            }
      
        }
        
        void OnEnterProgress(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            print ("***********************************OnEnterProgress");
            if (musicEnabled){
              AudioEventManager.PostEvent(AudioEventType.LevelThemeFadeOut, this);
               AudioEventManager.PostEvent(AudioEventType.MainThemeHardStart, this);
            }
      
        }
        
        void OnEnterLevel(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
                       print ("***********************************OnEnterLevel");
            if (musicEnabled){
               AudioEventManager.PostEvent(AudioEventType.MainThemeFadeOut, this);
               AudioEventManager.PostEvent(AudioEventType.LevelThemeHardStart, this);
            }
      
        }
    }
}