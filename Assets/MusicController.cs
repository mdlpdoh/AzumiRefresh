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
        FadeOut
    }


    public class MusicController : MonoBehaviour
    {
        public AudioEventType[] audioEvents;
        public AudioActionType[] audioActions;
        public float IntroDelay = 0f;
        public float bpm = 210.0f;
        public float numBeatsPerSegment = 32;
        public AudioClip[] loopClips;
        public AudioMixer mixer;

        public AudioMixerGroup mixerGroup;

        public string mixerGroupVolumeParameter;
        private AudioSource[] audioSources;

        private int clipIndex = 0;
        private bool loopIsRunning = false;
        private double nextEventTime;
        private Dictionary<AudioEventType, AudioActionType> audioTriggers;

        private AudioSource audioSource;
        public float fadeInTime;

        public AnimationCurve fadeInCurve;
        public float fadeOutTime;
        public AnimationCurve fadeOutCurve;
        void Awake()
        {

            audioSource = GetComponent<AudioSource>();
            audioTriggers = new Dictionary<AudioEventType, AudioActionType>();
            if (audioEvents.Length == audioActions.Length)
            {
                for (int i = 0; i < audioEvents.Length; i++)
                {
                    audioTriggers.Add(audioEvents[i], audioActions[i]);
                }
            }
            else
            {
                print("audioEvents and audioActions do not match");
            }
        }

        void Start()
        {
            List<AudioEventType> myKeys = new List<AudioEventType>(audioTriggers.Keys);

            for (int i = 0; i < myKeys.Count; i++)
            {
                AudioEventManager.ListenForEvent(myKeys[i], DoAudioEvent);
            }
            if (loopClips.Length > 0)
            {
                audioSources = new AudioSource[loopClips.Length];
                int i = 0;
                while (i < loopClips.Length)
                {
                    GameObject child = new GameObject("LoopPlayer");
                    child.transform.parent = gameObject.transform;
                    audioSources[i] = child.AddComponent<AudioSource>();
                    audioSources[i].outputAudioMixerGroup = mixerGroup;
                    i++;
                }
            }

        }

        public void DoAudioEvent(AudioEventType audioEventType, Component Sender, object Param = null)
        {

            switch (audioTriggers[audioEventType])
            {
                case AudioActionType.HardStart:

                    Invoke("Play", IntroDelay);

                    break;
                case AudioActionType.HardStop:
                    Invoke("Stop", 0);
                    break;
                case AudioActionType.FadeIn:

                    Invoke("StartFadeIn", IntroDelay);
                    break;
                case AudioActionType.FadeOut:

                    Invoke("StartFadeOut", 0);

                    break;
                default:
                    print("Audio Trigger ot recognized");
                    break;
            }

        }
        void Update()
        {
            if (!loopIsRunning)
                return;

            double time = AudioSettings.dspTime;
            if (time + 1.0F > nextEventTime)
            {
                audioSources[clipIndex].clip = loopClips[clipIndex];
                audioSources[clipIndex].PlayScheduled(nextEventTime);
                Debug.Log("Scheduled source " + clipIndex + " to start at time " + nextEventTime);
                nextEventTime += 60.0F / bpm * numBeatsPerSegment;
                if (clipIndex < audioSources.Length - 1)
                {

                    clipIndex++;
                }
                else
                {

                    clipIndex = 0;
                }
               // print(" clipIndex " + clipIndex);
            }
        }

        // Update is called once per frame
        public void Play()
        {
            nextEventTime = AudioSettings.dspTime + 2.0F;
            if (audioSource.clip != null && !audioSource.isPlaying)
            {

                audioSource.Play();
            }
            else if (loopClips.Length > 0 & !loopIsRunning)
            {

                loopIsRunning = true;
            }
            else
            {
          // audioSource.Stop();
               //    audioSource.Play();
                // print("nat doing anything");
            }
        }
        public void Stop()
        {

            if (audioSource.clip != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            else if (loopClips.Length > 0 & loopIsRunning)
            {
                loopIsRunning = false;
                for (int i = 0; i < audioSources.Length; i++)
                {
                    audioSources[i].Stop();
                }
            }
        }




        public void StartFadeOut()
        {

            if (audioSource.isPlaying || loopIsRunning)
            {
                StartCoroutine("FadeOut");
            }
        }

        public void StartFadeIn()
        {
            mixer.SetFloat(mixerGroupVolumeParameter, 0);
            Play();
            StartCoroutine("FadeIn");
        }

        private IEnumerator FadeIn()
        {
            float currentTime = 0f;
            float volume;
            while (currentTime < fadeInTime)
            {

                float normalizedTime = currentTime / fadeInTime;


                float curveProgress = fadeInCurve.Evaluate(normalizedTime);

                float musicVol = Mathf.Lerp(-80f, 0f, curveProgress);
                mixer.SetFloat(mixerGroupVolumeParameter, musicVol);

                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
            mixer.SetFloat(mixerGroupVolumeParameter, 1f);
        }
        private IEnumerator FadeOut()
        {
            float currentTime = 0f;
            float volume;
            while (currentTime < fadeOutTime)
            {
                float normalizedTime = currentTime / fadeOutTime;
                float curveProgress = fadeOutCurve.Evaluate(normalizedTime);
                float musicVol = Mathf.Lerp(-80f, 0f, 1 - curveProgress);
                mixer.SetFloat(mixerGroupVolumeParameter, musicVol);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
            mixer.SetFloat(mixerGroupVolumeParameter, 0f);

            Stop();
        }
    }
}