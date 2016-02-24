using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.dogonahorse
{
    //-----------------------------------------------------------
    //Enum defining all possible game events
    //List is excessively long and risky to edit
    public enum AzumiEventType
    {


        //Application Events
        unassigned,
        OpenModal,
        CloseModal,
        ScreenShotReady,
        BlurFadeOutComplete,
        EnterTitle,
        EnterProgress,
        EnterLevel,
        CancelSettings,
        SaveSettings,
        UpdatePlayerProgress,

        UnlockAllLevels,
        RelockLevels,
        ResetProgress,

        //Level events
        HitWall,
        HitCollectible,
        HitPowerUp,
        HitTrigger,
        inRepeller,
        HitDoor,
        GamePress,
        GameShift,
        GameRelease,
        GameSwipe,
        SetCoins,
        SetBounces,
        OutOfBounces,
        OutOfTime,
        HealWallExpired,
        SwipesLow,
        SwipesLowFadeIn,
        SwipesLowFadeOut,
        StartTimer,
        LevelWon,
        LevelLost,
        inAttractor,
        inDirectionalMover,
        UITap,
        StartEndGameSequence,
        FinishEndGameSequence,
        WinStar,
        FailStar,
        DoorOpen,
        UnLockLevel,
        SwipesAboveMinimum,
        PauseLevel,
        ResumeLevel,
        RestartLevel,
        ExitLevelEarly,
        CancelTimer

    }
    //-----------------------------------------------------------
    /// <summary>
    /// Singleton EventManager to send events to listeners
    /// Works with IListener implementations
    /// </summary>
    /// <remarks>
    /// Event Handler is overloaded with custom events--adding new enumerated eventTypes, except to the very end, displaces all the subsequent events in the enum.
    /// A better implementation would add event handlers as a property of particular packages, or maybe classes, or more likely an interface.
    /// 
    /// The distinction between the class and instance methods should be cleaned up--both are not necessary 
    /// Attached to GamesScripts gameObject
    /// </remarks>
    public class EventManager : MonoBehaviour
    {
        #region C# properties
        //-----------------------------------------------------------
        /// <summary>
        /// Public access to instance
        /// </summary>
        /// <returns>unique instance of class</returns>
        public static EventManager Instance
        {
            get
            {
                return instance;
            }
            set
            {

            }
        }
        #endregion

        #region variables
        //Internal reference to Notifications Manager instance (singleton design pattern)
        private static EventManager instance = null;

        // Declare a delegate type for events
        public delegate void OnEvent(AzumiEventType azumiEventType, Component Sender, object Param = null);

        //Array of listener objects (all objects registered to listen for events)
        private Dictionary<AzumiEventType, List<OnEvent>> Listeners = new Dictionary<AzumiEventType, List<OnEvent>>();
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
        }


        //-----------------------------------------------------------
        /// <summary>
        /// Function to add specified listener-object to array of listeners
        /// </summary>
        /// <param name="AzumiEventType">Event to Listen for</param>
        /// <param name="Listener">Object to listen for event</param>
        /// <remarks>
        /// This is an instance method. that is somewhat redundant with the static method ListenForEvent 
        /// </remarks>
        public void AddListener(AzumiEventType azumiEventType, OnEvent Listener)
        {
            //List of listeners for this event
            List<OnEvent> ListenList = null;

            //New item to be added. Check for existing event type key. If one exists, add to list
            if (Listeners.TryGetValue(azumiEventType, out ListenList))
            {
                //List exists, so add new item
                ListenList.Add(Listener);
                return;
            }

            //Otherwise create new list as dictionary key
            ListenList = new List<OnEvent>();
            ListenList.Add(Listener);
            Listeners.Add(azumiEventType, ListenList); //Add to internal listeners list
        }


        //-----------------------------------------------------------

        /// <summary>
        ///  Posts  event to listeners
        /// </summary>
        /// <param name="azumiEventType">Event to invoke of type AzumiEventType</param>
        /// <param name="Sender">Object invoking event of type Component</param>
        /// <param name="Param">Optional argument of type Object</param>
        public void PostNotification(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            //Notify all listeners of an event

            //List of listeners for this event only
            List<OnEvent> ListenList = null;

            //If no event entry exists, then exit because there are no listeners to notify
            if (!Listeners.TryGetValue(azumiEventType, out ListenList))
                return;

            //Entry exists. Now notify appropriate listeners
            for (int i = 0; i < ListenList.Count; i++)
            {
                if (!ListenList[i].Equals(null)) //If object is not null, then send message via interfaces
                    ListenList[i](azumiEventType, Sender, Param);
            }
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Adds a listener for a particular event
        /// </summary>
        /// <param name="AzumiEventType">Event to Listen for</param>
        /// <param name="Listener">Object to listen for event</param>
        /// <remarks>
        /// This is an static method that is somewhat redundant with the instance method AddListener ( it just calls AddListener). 
        /// </remarks>

        public static void ListenForEvent(AzumiEventType azumiEventType, OnEvent Listener)
        {
            Instance.AddListener(azumiEventType, Listener);
        }


        //-----------------------------------------------------------
        /// <summary>
        ///  Posts  event to listeners
        /// </summary>
        /// <param name="azumiEventType">Event to invoke of type AzumiEventType</param>
        /// <param name="Sender">Object invoking event of type Component</param>
        /// <param name="Param">Optional argument of type Object</param>
        /// <remarks>
        /// This is a Static method that is somewhat redundant with the instance method PostNotification ( it just calls postEvent). 
        /// </remarks>
        public static void PostEvent(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            Instance.PostNotification(azumiEventType, Sender, Param);
        }


        /// <summary>
        /// Clears all listeners for certain events. 
        /// </summary>
        /// <remarks>
        /// A poor implementation--overly specific and potentially confusing as it may cancel leisters that should persist 
        /// </remarks>

        public static void ClearGameLevelListeners()
        {
            // print("Clearing Game Listeners");

            Instance.RemoveEvent(AzumiEventType.GameSwipe);
            Instance.RemoveEvent(AzumiEventType.SetCoins);
            Instance.RemoveEvent(AzumiEventType.SetBounces);
            Instance.RemoveEvent(AzumiEventType.OutOfBounces);
            Instance.RemoveEvent(AzumiEventType.HitWall);
            Instance.RemoveEvent(AzumiEventType.HitCollectible);
            Instance.RemoveEvent(AzumiEventType.HitPowerUp);
            Instance.RemoveEvent(AzumiEventType.HitTrigger);
            Instance.RemoveEvent(AzumiEventType.HitDoor);
            Instance.RemoveEvent(AzumiEventType.GamePress);
            Instance.RemoveEvent(AzumiEventType.GameShift);
            Instance.RemoveEvent(AzumiEventType.GameRelease);
            Instance.RemoveEvent(AzumiEventType.SwipesLow);
            Instance.RemoveEvent(AzumiEventType.SwipesLowFadeIn);
            Instance.RemoveEvent(AzumiEventType.SwipesLowFadeOut);
            Instance.RemoveEvent(AzumiEventType.StartTimer);
            Instance.RemoveEvent(AzumiEventType.OutOfBounces);
            Instance.RemoveEvent(AzumiEventType.OutOfTime);

        }

        //---------------------------------------------------------
        /// <summary>
        /// Remove event type entry from dictionary, including all listeners
        /// </summary>
        /// <param name="azumiEventType">Event Type to remove</param>
        /// <remarks>
        /// kills all listeners for the designated type 
        /// </remarks>
        public void RemoveEvent(AzumiEventType azumiEventType)
        {
            //Remove entry from dictionary
            Listeners.Remove(azumiEventType);
        }

        //---------------------------------------------------------
        /// <summary>
        /// Remove a listener for a single type and target
        /// </summary>
        /// <param name="AzumiEventType">Event Type to remove</param>
        /// <param name="Listener">Listening Object to remove</param>
        /// <remarks>
        /// kills only the event for designated listener, leaving others untouched
        /// </remarks>
        public void RemoveListener(AzumiEventType azumiEventType, OnEvent Listener)
        {
            if (Listeners.ContainsKey(azumiEventType))
            {
                List<OnEvent> tempList = new List<OnEvent>();
                for (int i = 0; i < Listeners[azumiEventType].Count; i++)
                {
                    if (Listeners[azumiEventType][i] == Listener)
                    {
                        //do nothing--this is the listener we want to eliminate
                    }
                    else
                    {
                        //add to new list which will short;y replace the old one
                        tempList.Add(Listeners[azumiEventType][i]);
                    }
                }
                Listeners[azumiEventType] = tempList;

            }
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Remove all redundant entries from the Dictionary
        /// </summary>
        /// <remarks>
        /// Not sure if this even works
        /// </remarks>
        public void RemoveRedundancies()
        {
            //Create new dictionary
            Dictionary<AzumiEventType, List<OnEvent>> TmpListeners = new Dictionary<AzumiEventType, List<OnEvent>>();

            //Cycle through all dictionary entries
            foreach (KeyValuePair<AzumiEventType, List<OnEvent>> Item in Listeners)
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
