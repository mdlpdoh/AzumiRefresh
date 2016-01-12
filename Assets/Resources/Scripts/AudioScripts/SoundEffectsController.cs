﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
namespace com.dogonahorse
{


    public class SoundEffectsController : MonoBehaviour
    {

        public AudioActionType myAction;
        public AzumiEventType myEvent;
        public bool isRandom = false;
        public bool dontRepeat = false;
        public AudioClip[] RandomClips;
        public float maxVolume = 1.0f;
        public float droneVolumeIncreaseRatio = 100f;

        public float droneVolumeDecreaseRatio = 50f;

        private bool droneIsActive = false;

        public float panLevel = 1.0f;
        public AudioMixer mixer;

        private AudioMixerGroup mixerGroup;

        public string mixerGroupVolumeParameter;
        //private AudioSource[] audioSources;
        private List<AudioSource> audioSources = new List<AudioSource>();
        private AudioSource audioSource;
        private List<int> randomList;
        private int randomIndex = 0;
        void Awake()
        {

            audioSource = GetComponent<AudioSource>();
            mixerGroup = audioSource.outputAudioMixerGroup;
            mixer = (AudioMixer)Resources.Load("AzumiAudio");

        }

        void Start()
        {
            if (isRandom && RandomClips.Length > 0)
            {
                refreshRandomList();
            }
            // setUpAudioSources();
            EventManager.ListenForEvent(myEvent, DoAudioEvent);
        }

        AudioSource getNewAudioSource(AudioClip newClip, Transform objectLocation )
        {

            GameObject child = new GameObject("tempPlayer");
            
            
           // child.transform.parent = gameObject.transform;
          child.transform.position =  objectLocation.position;
            AudioSource newAudioSource = child.AddComponent<AudioSource>();
            newAudioSource.outputAudioMixerGroup = mixerGroup;

            newAudioSource.spatialBlend = panLevel;
            newAudioSource.clip = newClip;

            return newAudioSource;
        }
        void refreshRandomList()
        {
            randomList = new List<int>();

            int i = 0;
            while (i < RandomClips.Length)
            {
                randomList.Add(i);
                i++;
            }
        }


        public void DoAudioEvent(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            switch (myAction)
            {
                case AudioActionType.HardStart:
                    Play(Sender.gameObject.transform);
                    break;
                case AudioActionType.HardStop:
                    Stop();
                    break;

                case AudioActionType.FadingDrone:
                    Drone(Sender.gameObject.transform);
                    break;

                default:
                    print("Audio Trigger not recognized");
                    break;
            }
        }

        public void Drone(Transform objectLocation)
        {
            droneIsActive = true;
            if (audioSources.Count < 1 || !audioSources[0].isPlaying)
            {
                //no sound--start sound up
                AudioSource newAudioSource = getNewAudioSource(RandomClips[0], objectLocation);
                newAudioSource.volume = 0;
                newAudioSource.Play();
                audioSources.Add(newAudioSource); ;
            }
            else if (audioSources.Count == 1 || audioSources[0].isPlaying)
            {
                //Sound already playing--Fade In 
                audioSources[0].volume += (maxVolume - audioSources[0].volume) / droneVolumeIncreaseRatio;
            }
        }
        public void Play(Transform objectLocation)
        {
            // audioSource.Stop();
            //clip needs to be randomized between a number of different clips
            if (isRandom && RandomClips.Length > 0)
            {
                if (dontRepeat)
                {
                    if (randomList.Count == 0)
                    {
                        refreshRandomList();
                    }
                    int newRandomIndex = Mathf.RoundToInt(Random.Range(0, randomList.Count));

                    randomIndex = randomList[newRandomIndex];
                    randomList.RemoveAt(newRandomIndex);
                }
                else
                {
                    randomIndex = Mathf.RoundToInt(Random.Range(0, RandomClips.Length));
                }

                AudioSource newAudioSource = getNewAudioSource(RandomClips[randomIndex], objectLocation);
                newAudioSource.Play();
                audioSources.Add(newAudioSource); ;

                //audioSource.clip = R;
                //    audioSources[randomIndex].Stop();
                //audioSources[randomIndex].Play();
            }
            else
            {
                AudioSource newAudioSource = getNewAudioSource(RandomClips[0],  objectLocation);
                newAudioSource.Play();
                audioSources.Add(newAudioSource); ;
            }
        }


        void Update()
        {

            if (audioSources.Count > 0)
            {
                int i = audioSources.Count - 1;
                while (i >= 0)
                {
                    if (!audioSources[i].isPlaying)
                    {
                        GameObject.Destroy(audioSources[i].gameObject);
                        audioSources.RemoveAt(i);
                    }
                    i--;
                }
                if (audioSources.Count > 0  && myAction == AudioActionType.FadingDrone)
                {
                    if (droneIsActive)
                    {
                        droneIsActive = false;
                    }
                    else
                    {
                        float decreaseAmount = audioSources[0].volume / droneVolumeDecreaseRatio;
                        if (audioSources[0].volume - decreaseAmount > 0)
                        {
                            audioSources[0].volume -= decreaseAmount;
                        }
                        else
                        {
                            audioSources[0].volume = 0;

                        }
                    }
                }
            }
        }
        void OnDestroy()
        {
            EventManager.Instance.RemoveListener(myEvent, DoAudioEvent);
        }
        public void Stop()
        {

            int i = 0;
            while (i < audioSources.Count)
            {

                audioSources[i].Stop();
                i++;
            }


        }

    }
}