using UnityEngine;
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
        public AudioMixer mixer;

        private AudioMixerGroup mixerGroup;

        public string mixerGroupVolumeParameter;
        private AudioSource[] audioSources;

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
            refreshRandomList();
            EventManager.ListenForEvent(myEvent, DoAudioEvent);
        }
        void refreshRandomList()
        {
            randomList = new List<int>();
            randomList.Add(0);
            randomList.Add(1);
            randomList.Add(2);
            randomList.Add(3);
        }


        public void DoAudioEvent(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {

            switch (myAction)
            {
                case AudioActionType.HardStart:

                    Invoke("Play", 0);

                    break;
                case AudioActionType.HardStop:
                    Invoke("Stop", 0);
                    break;



                default:
                    print("Audio Trigger not recognized");
                    break;
            }

        }


        public void Play()
        {
            audioSource.Stop();

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
                audioSource.clip = RandomClips[randomIndex];
                audioSource.Play();
            }
            else
            {
                audioSource.Play();
            }
        }



        void OnDestroy()
        {
            EventManager.Instance.RemoveListener(myEvent, DoAudioEvent);
        }
        public void Stop()
        {
            audioSource.Stop();
        }

    }
}