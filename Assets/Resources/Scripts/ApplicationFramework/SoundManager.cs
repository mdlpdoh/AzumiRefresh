using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace com.dogonahorse
{

    public enum AudioActionType

    {
        unassigned,
        HardStart,
        HardStop,
        FadeIn,
        FadeOut,
        FadingDrone,
    }

    public enum VolumeSetting

    {
        Music,
        SoundFX

    }
    public class SoundManager : MonoBehaviour
    {
        public AudioMixer mixer;
        public string musicMixerVolume;
        public string soundEffectsMixerVolume;

        private static SoundManager instance = null;
        //private List<LevelPlayerData> LevelPlayerList = new List<LevelPlayerData> ();

        private bool musicEnabled = true;
        private bool soundFXEnabled = true;


        private float musicVolume = 1;
        private float soundFXVolume = 1;


        public static SoundManager Instance
        {
            // return reference to private instance 
            get
            {
                return instance;
            }
        }

        public static float MusicVolume
        {
            get
            {
                return instance.musicVolume;
            }
            set
            { 
                instance.UpdateMusicVolume(value);
            }
        }

        public static float SoundFXVolume
        {
            get
            {
                return instance.soundFXVolume;
            }
            set
            {
                instance.UpdateSoundEffectsVolume(value);
            }
        }

        void UpdateMusicVolume(float value)
        {
            musicVolume = value;
            mixer.SetFloat(musicMixerVolume, deNormalizeVolume(musicVolume));
        }
        
        float  deNormalizeVolume(float value)
        {     
            float t = Mathf.Log10(value);
            return  Mathf.Lerp(-80f, 0f, t + 1);
        }
        void UpdateSoundEffectsVolume(float value)
        {
            soundFXVolume = value;
            mixer.SetFloat(soundEffectsMixerVolume, deNormalizeVolume(soundFXVolume));
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
            // print("***********************************OnEnterTitle");
            if (musicEnabled)
            {
                AudioEventManager.PostEvent(AudioEventType.MainThemeHardStart, this);
            }

        }

        void OnEnterProgress(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            // print("***********************************OnEnterProgress");
            if (musicEnabled)
            {
                AudioEventManager.PostEvent(AudioEventType.LevelThemeFadeOut, this);
                AudioEventManager.PostEvent(AudioEventType.MainThemeHardStart, this);
            }

        }

        void OnEnterLevel(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            // print("***********************************OnEnterLevel");
            if (musicEnabled)
            {
                AudioEventManager.PostEvent(AudioEventType.MainThemeFadeOut, this);
                AudioEventManager.PostEvent(AudioEventType.LevelThemeHardStart, this);
            }
        }
    }
}