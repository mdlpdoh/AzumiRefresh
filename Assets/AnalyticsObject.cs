using UnityEngine;
using System;
using System.Collections.Generic;
namespace com.dogonahorse
{
    //-----------------
    public class AnalyticsObject : MonoBehaviour
    {

        // Use this for initialization

        private float startTime = 0;
        private float endTime = 0;
        private float totalTime = 0;
        public AnalyticsEventType analyticsEvent;
        public AzumiEventType startEvent;
        public AzumiEventType closeEvent;



        void Start()
        {
            EventManager.ListenForEvent(startEvent, onStartEvent);

            if (closeEvent != AzumiEventType.unassigned)
            {
                EventManager.ListenForEvent(closeEvent, onCloseEvent);
            }
        }

        // Update is called once per frame
        void onStartEvent(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {

            if (closeEvent == AzumiEventType.unassigned)
            {
                AnalyticsManager.PostEvent(analyticsEvent, this,  new Dictionary<string, object>{ {"dateTime", DateTime.Now.ToString()}});
            }
            else
            {

            }
        }
        void onCloseEvent(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {

        }
    }
}
