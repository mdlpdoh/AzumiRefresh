using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;


namespace com.dogonahorse
{

    //-----------------------------------------------------------
    //Enum defining all possible game events
    //More events should be added to the list
    public enum AnalyticsEventType
    {

        //Application Events
        StartApp,
        StartLevel,
        WonLevel,
        LostLevel,

        PausedLevel,

        ResumedLevel,
        RestartedLevel,
        ExitedLevel,
        LevelThemeFadeOut,

        SavedNewSettings,
        ClosedInstructions,
        ClosedAboutPangolins,
        QuitApp
    }

    //Analytics.CustomEvent(string customEventName, IDictionary<string, object> eventData);
    //-----------------------------------------------------------
    //Singleton EventManager to send events to listeners
    //Works with IListener implementations
    public class AnalyticsManager : MonoBehaviour
    {

        #region C# properties
        //-----------------------------------------------------------
        //Public access to instance
        public static AnalyticsManager Instance
        {
            get { return instance; }
            set { }
        }
        #endregion



        #region variables
        //Internal reference to Notifications Manager instance (singleton design pattern)
        private static AnalyticsManager instance = null;

        // Declare a delegate type for events
        public delegate void OnEvent(AnalyticsEventType analyticsEventType, Component Sender, Dictionary<string, object> Param = null);

        //Array of listener objects (all objects registered to listen for events)
        private Dictionary<AnalyticsEventType, List<OnEvent>> Listeners = new Dictionary<AnalyticsEventType, List<OnEvent>>();

        private static string deviceModel;
        private static string operatingSystem;
        #endregion
        //-----------------------------------------------------------
        #region methods
        //Called at start-up to initialize
        void Awake()
        {
            //If no instance exists, then assign this instance
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); //Prevent object from being destroyed on scene exit
            }
            else //Instance already exists, so destroy this one. This should be a singleton object
                DestroyImmediate(this);


            AnalyticsManager.ListenForEvent(AnalyticsEventType.StartApp, OnStartApp);

        }

        void Start()
        {
            deviceModel = SystemInfo.deviceModel;
            operatingSystem = SystemInfo.operatingSystem;
            SendEvent(AnalyticsEventType.StartApp);

        }
        void OnStartApp(AnalyticsEventType analyticsEventType, Component Sender, Dictionary<string, object> Param)
        {
          //  SendEvent(analyticsEventType);

        }
        /*
        Dictionary<string, object> MergeParameters(Dictionary<string, object> dict1, Dictionary<string, object> dict2)
        {

            for (int i = 0; i < dict2.Count; i++)
            {
                dict1.Add(dict2);
            }
            return dict1;
        }
*/

        void SendEvent(AnalyticsEventType analyticsEventType)
        {

            print("Analytics :" + Analytics.CustomEvent(analyticsEventType.ToString(), new Dictionary<string, object>
            {
                { "deviceModel", deviceModel },
                                                                        { "operatingSystem", operatingSystem },
                                                                         }));

        }


        //-----------------------------------------------------------
        /// <summary>
        /// Function to add specified listener-object to array of listeners
        /// </summary>
        /// <param name="AudioEventType">Event to Listen for</param>
        /// <param name="Listener">Object to listen for event</param>
        public void AddListener(AnalyticsEventType analyticsEventType, OnEvent Listener)
        {
            //List of listeners for this event
            List<OnEvent> ListenList = null;

            //New item to be added. Check for existing event type key. If one exists, add to list
            if (Listeners.TryGetValue(analyticsEventType, out ListenList))
            {
                //List exists, so add new item
                ListenList.Add(Listener);
                return;
            }

            ListenList = new List<OnEvent>();
            ListenList.Add(Listener);
            Listeners.Add(analyticsEventType, ListenList); //Add to internal listeners list
        }


        public void OnNewEvent(AnalyticsEventType analyticsEventType, Component Sender, Dictionary<string, object> Param)
        {


        }

        //-----------------------------------------------------------
        /// <summary>
        /// Function to post event to listeners
        /// </summary>
        /// <param name="AudioEventType">Event to invoke</param>
        /// <param name="Sender">Object invoking event</param>
        /// <param name="Param">Optional argument</param>
        /// 
        public static void PostEvent(AnalyticsEventType analyticsEventType, Component Sender, Dictionary<string, object> Param)
        {
            Instance.PostNotification(analyticsEventType, Sender, Param);
        }

        public static void ListenForEvent(AnalyticsEventType analyticsEventType, OnEvent Listener)
        {
            Instance.AddListener(analyticsEventType, Listener);
        }

        public void PostNotification(AnalyticsEventType analyticsEventType, Component Sender, Dictionary<string, object> Param)
        {
            //Notify all listeners of an event

            //List of listeners for this event only
            List<OnEvent> ListenList = null;

            //If no event entry exists, then exit because there are no listeners to notify
            if (!Listeners.TryGetValue(analyticsEventType, out ListenList))
                return;

            //Entry exists. Now notify appropriate listeners
            for (int i = 0; i < ListenList.Count; i++)
            {
                if (!ListenList[i].Equals(null)) //If object is not null, then send message via interfaces
                    ListenList[i](analyticsEventType, Sender, Param);
            }
        }



        //---------------------------------------------------------
        //Remove event type entry from dictionary, including all listeners
        public void RemoveEvent(AnalyticsEventType analyticsEventType)
        {
            //Remove entry from dictionary
            Listeners.Remove(analyticsEventType);
        }
        //-----------------------------------------------------------
        //Remove all redundant entries from the Dictionary
        public void RemoveRedundancies()
        {
            //Create new dictionary
            Dictionary<AnalyticsEventType, List<OnEvent>> TmpListeners = new Dictionary<AnalyticsEventType, List<OnEvent>>();

            //Cycle through all dictionary entries
            foreach (KeyValuePair<AnalyticsEventType, List<OnEvent>> Item in Listeners)
            {
                //Cycle through all listener objects in list, remove null objects
                for (int i = Item.Value.Count - 1; i >= 0; i--)
                {
                    //If null, then remove item

                    if (Item.Value[i].Equals(null))
                        Item.Value.RemoveAt(i);
                }

                //If items remain in list for this notification, then add this to tmp dictionary
                if (Item.Value.Count > 0)
                    TmpListeners.Add(Item.Key, Item.Value);
            }

            //Replace listeners object with new, optimized dictionary
            Listeners = TmpListeners;
        }
        //-----------------------------------------------------------
        //Called on scene change. Clean up dictionary
        void OnLevelWasLoaded()
        {
            RemoveRedundancies();
        }
        //-----------------------------------------------------------
        #endregion
    }
}
