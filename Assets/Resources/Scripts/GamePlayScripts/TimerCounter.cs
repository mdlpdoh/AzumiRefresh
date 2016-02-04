using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace com.dogonahorse
{
    public class TimerCounter : MonoBehaviour
    {
        private Text myText;
        public float timer = 7.0f;


        void Awake()
        {
            myText = GetComponent<Text>();
        }

        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.StartTimer, EndGameStartTimer);
            EventManager.ListenForEvent(AzumiEventType.LevelLost, StopTimer);
            EventManager.ListenForEvent(AzumiEventType.LevelWon, StopTimer);
        }
             void OnDestroy()
        {
            EventManager.Instance.RemoveListener(AzumiEventType.StartTimer, EndGameStartTimer);
            EventManager.Instance.RemoveListener(AzumiEventType.LevelLost, StopTimer);
            EventManager.Instance.RemoveListener(AzumiEventType.LevelWon, StopTimer);

        }
        void StopTimer(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            StopAllCoroutines();
        }
        void EndGameStartTimer(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            StartCoroutine("EndTimer");
        }
        private IEnumerator EndTimer()
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                myText.text = timer.ToString("F1");
                yield return null;
            }
            myText.text = "0";
            EventManager.PostEvent(AzumiEventType.OutOfTime, this);

        }// end ienumerator 

    }//end class
}// end namespace

