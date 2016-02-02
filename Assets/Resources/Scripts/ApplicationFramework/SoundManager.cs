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
        public string musicMixerVolumeID;
        public string soundEffectsMixerVolumeID;

        private static SoundManager instance = null;


        private bool musicEnabled = true;
//        private bool soundFXEnabled = true;

        public float musicVolumeAdjustment = 1;
        public float soundFXVolumeAdjustment = 1;
        private float musicVolume = 1;
        private float soundFXVolume = 1;

        private float formerMusicVolume = 1;
        private float formerSoundFXVolume = 1;

        private bool editingSoundLevels = false;

        private bool actualSoundLevelsLoaded = false;
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


        void cacheOriginalSettings()
        {
            if (!editingSoundLevels)
            {
                editingSoundLevels = true;
                formerMusicVolume = musicVolume;
                formerSoundFXVolume = soundFXVolume;
            }
        }

        void UpdateMusicVolume(float value)
        {
            if (actualSoundLevelsLoaded)
            {
                cacheOriginalSettings();
                musicVolume = value;
                mixer.SetFloat(musicMixerVolumeID, deNormalizeVolume(musicVolume * musicVolumeAdjustment));
            }
        }

        float deNormalizeVolume(float value)
        {
            float t = Mathf.Log10(value);
            return Mathf.Lerp(-80f, 20f, t + 1);
        }
        void UpdateSoundEffectsVolume(float value)
        {
            if (actualSoundLevelsLoaded)
            {

                cacheOriginalSettings();
                soundFXVolume = value;
                mixer.SetFloat(soundEffectsMixerVolumeID, deNormalizeVolume(soundFXVolume * soundFXVolumeAdjustment));
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


            if (!PlayerPrefs.HasKey("musicVolume"))
            {
                PlayerPrefs.SetFloat("musicVolume", musicVolume);

            }
            else
            {
                musicVolume = PlayerPrefs.GetFloat("musicVolume");

            }

            if (!PlayerPrefs.HasKey("soundFXVolume"))
            {
                PlayerPrefs.SetFloat("soundFXVolume", soundFXVolume);
            }
            else
            {
                soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
            }
            formerMusicVolume = musicVolume;
            formerSoundFXVolume = soundFXVolume;
            mixer.SetFloat(musicMixerVolumeID, deNormalizeVolume(musicVolume));
            mixer.SetFloat(soundEffectsMixerVolumeID, deNormalizeVolume(soundFXVolume));
            EventManager.ListenForEvent(AzumiEventType.EnterTitle, OnEnterTitle);
            EventManager.ListenForEvent(AzumiEventType.EnterProgress, OnEnterProgress);
            EventManager.ListenForEvent(AzumiEventType.EnterLevel, OnEnterLevel);
            EventManager.ListenForEvent(AzumiEventType.CancelSettings, OnCancelSettings);
            EventManager.ListenForEvent(AzumiEventType.SaveSettings, OnSaveSettings);
            EventManager.ListenForEvent(AzumiEventType.LevelLost, OnLevelLost);

            actualSoundLevelsLoaded = true;
            mixer.SetFloat(musicMixerVolumeID, deNormalizeVolume(musicVolume * musicVolumeAdjustment));
            mixer.SetFloat(soundEffectsMixerVolumeID, deNormalizeVolume(soundFXVolume * soundFXVolumeAdjustment));
        }
        void SaveSettings()
        {


            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetFloat("soundFXVolume", soundFXVolume);
        }



        void OnEnterTitle(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            if (musicEnabled)
            {
                AudioEventManager.PostEvent(AudioEventType.MainThemeHardStart, this);
            }

        }

        void OnEnterProgress(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
          
            if (musicEnabled)
            {
                AudioEventManager.PostEvent(AudioEventType.LevelThemeFadeOut, this);
                AudioEventManager.PostEvent(AudioEventType.MainThemeHardStart, this);
                AudioEventManager.PostEvent(AudioEventType.LossThemeFadeOut, this);
            }

        }

        void OnEnterLevel(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {     

            if (musicEnabled)
            {
                AudioEventManager.PostEvent(AudioEventType.MainThemeFadeOut, this);
                AudioEventManager.PostEvent(AudioEventType.LevelThemeHardStart, this);
                AudioEventManager.PostEvent(AudioEventType.LossThemeFadeOut, this);
            }
        }
        void OnLevelLost(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
     
            if (musicEnabled)
            {
                AudioEventManager.PostEvent(AudioEventType.LevelThemeFadeOut, this);
                AudioEventManager.PostEvent(AudioEventType.LossThemeHardStart, this);
            }
        }

        void OnCancelSettings(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            UpdateMusicVolume(formerMusicVolume);
            UpdateSoundEffectsVolume(formerSoundFXVolume);
            editingSoundLevels = false;
        }

        void OnSaveSettings(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            SaveSettings();
            formerMusicVolume = musicVolume;
            formerSoundFXVolume = soundFXVolume;
            editingSoundLevels = false;
        }

    }
}