using UnityEngine;
using System.Collections;
namespace com.dogonahorse
{
    public class AnalyticsTimer : ScriptableObject
    {

        private float startTime = 0;
     


        public void Init()
        {  
            startTime = Time.time;
        }
       
        public  float GetElapsedTime()
        {
          return Time.time - startTime;
        }

    }
}