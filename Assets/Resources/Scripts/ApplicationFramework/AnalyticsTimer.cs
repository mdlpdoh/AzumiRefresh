using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{
    public class AnalyticsTimer : ScriptableObject
    {
        private AnalyticsEventType timerType;
        private float startTime = 0;
        private float sndTime = 0;


        public void Init(AnalyticsEventType newType)
        {
            timerType = newType;
            startTime = Time.time;
        }
       
        public  float GetElapsedTime()
        {
          return Time.time - startTime;
        }

    }
}