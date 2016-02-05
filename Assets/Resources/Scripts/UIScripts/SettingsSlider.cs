using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace com.dogonahorse
{
    public class SettingsSlider : MonoBehaviour
    {



        public VolumeSetting mySetting;

        private Slider slider;

        private float minLevelFloat;
        private float maxLevelFloat;
        private float currentLevel = 1;


        // Use this for initialization
        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void Start()
        {
       
            if (mySetting == VolumeSetting.Music){
  
                 currentLevel = SoundManager.MusicVolume;
            }else {
                 currentLevel = SoundManager.SoundFXVolume;
            }
            slider.value = currentLevel;
       
        }


        public void SliderUpdate(float newValue)
        {
            if (mySetting == VolumeSetting.Music){
                  SoundManager.MusicVolume = newValue;
            }else {
                 SoundManager.SoundFXVolume = newValue; 
            }
        }
    }
}
