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
        Quit
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
        // public delegate void OnEvent(AnalyticsEventType analyticsEventType, Component Sender, Dictionary<string, object> Param = null);

        //Array of listener objects (all objects registered to listen for events)
        private Dictionary<AnalyticsEventType, AnalyticsTimer> timerDict = new Dictionary<AnalyticsEventType, AnalyticsTimer>();

        private static string dev;
        private static string OS;
        private static string ID;
        private static int numberOfTimesPaused = 0;
        private static int numberOfTimesRestarted = 0;
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


            // AnalyticsManager.ListenForEvent(AnalyticsEventType.StartApp, OnStartApp);

        }
        void Start()
        {
            //don't gather analytics from editor
          if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                dev = SystemInfo.deviceModel;
                OS = SystemInfo.operatingSystem;
                ID = SystemInfo.deviceUniqueIdentifier;

                AddNewTimerObject(AnalyticsEventType.StartApp);
                //listeners
                EventManager.ListenForEvent(AzumiEventType.EnterLevel, onEnterLevel);
                EventManager.ListenForEvent(AzumiEventType.LevelWon, onLevelWon);
                EventManager.ListenForEvent(AzumiEventType.LevelLost, onLevelLost);
                EventManager.ListenForEvent(AzumiEventType.PauseLevel, onLevelPaused);
                EventManager.ListenForEvent(AzumiEventType.RestartLevel, OnLevelRestart);
                EventManager.ListenForEvent(AzumiEventType.EnterProgress, OnClearLevelRestarts);
                EventManager.ListenForEvent(AzumiEventType.ExitLevelEarly, OnExitLevelEarly);
                EventManager.ListenForEvent(AzumiEventType.SaveSettings, OnSaveSettings);
                StartApp();
            }
       }

        AnalyticsTimer GetNewTimer()
        {
            AnalyticsTimer newTimer = ScriptableObject.CreateInstance("AnalyticsTimer") as AnalyticsTimer;

            newTimer.Init();
            return newTimer;
        }



        Dictionary<string, object> AddStandardParameters(Dictionary<string, object> dict1)
        {
            dict1.Add("ID", ID);
            dict1.Add("dev", dev);
            dict1.Add("OS", OS);

            return dict1;
        }
        void AddNewTimerObject(AnalyticsEventType newType)
        {
            if (timerDict.ContainsKey(newType))
            {
                timerDict[newType] = GetNewTimer();
            }
            else
            {
                timerDict.Add(newType, GetNewTimer());
            }
        }



        public void onEnterLevel(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            //reset pause count for new level
            numberOfTimesPaused = 0;
            AddNewTimerObject(AnalyticsEventType.StartLevel);
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            paramList.Add("Level", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            paramList.Add("NumPlaysThisSession", LevelManager.Instance.GetTimesLevelPlayed());
            paramList.Add("Wins", LevelManager.Instance.GetWins());
            paramList.Add("Losses", LevelManager.Instance.GetLosses());

            Analytics.CustomEvent("StartLevel", paramList);
            // print("Analytics onEnterLevel:" + Analytics.CustomEvent("StartLevel", paramList));
            // OutputParams(azumiEventType.ToString(), paramList);
        }

        public void onLevelWon(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            FPSDisplay fpsDisplay = GameObject.Find("SceneScripts").GetComponent<FPSDisplay>();
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            paramList.Add("Level", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            paramList.Add("NumPlays", LevelManager.Instance.GetTimesLevelPlayed());
            paramList.Add("Time", timerDict[AnalyticsEventType.StartLevel].GetElapsedTime());
            paramList.Add("Stars", scoreManager.NumberOfStars);
            paramList.Add("Swipes", scoreManager.SwipesRemaining);
            paramList.Add("Coins", scoreManager.CoinsEarned);
            paramList.Add("fps", fpsDisplay.GetAverageFPS());
            Analytics.CustomEvent("Win", paramList);
            // print("Analytics onLevelWon:" + Analytics.CustomEvent("Win", paramList));
            //OutputParams(azumiEventType.ToString(), paramList);
        }

        public void onLevelLost(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {

            ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            FPSDisplay fpsDisplay = GameObject.Find("SceneScripts").GetComponent<FPSDisplay>();
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            paramList.Add("Level", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            paramList.Add("NumPlays", LevelManager.Instance.GetTimesLevelPlayed());
            paramList.Add("Time", timerDict[AnalyticsEventType.StartLevel].GetElapsedTime());
            paramList.Add("result", scoreManager.GetReasonForLoss());
            paramList.Add("Coins", scoreManager.CoinsEarned);
            paramList.Add("fps", fpsDisplay.GetAverageFPS());
            Analytics.CustomEvent("Loss", paramList);
            // print("Analytics onLevelLost:" + Analytics.CustomEvent("Loss", paramList));
            //OutputParams(azumiEventType.ToString(), paramList);
        }

        public void onLevelPaused(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            numberOfTimesPaused++;
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            paramList.Add("Level", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            paramList.Add("Time", timerDict[AnalyticsEventType.StartLevel].GetElapsedTime());
            paramList.Add("timesPaused", numberOfTimesPaused);
            Analytics.CustomEvent("Pause", paramList);
            //print("Analytics onLevelPaused:" + Analytics.CustomEvent("Pause", paramList));
            // OutputParams(azumiEventType.ToString(), paramList);
        }
        public void OnLevelRestart(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            numberOfTimesRestarted++;
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            paramList.Add("Level", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            paramList.Add("Time", timerDict[AnalyticsEventType.StartLevel].GetElapsedTime());
            paramList.Add("timesRestarted", numberOfTimesRestarted);
            Analytics.CustomEvent("Restart", paramList);
            //OutputParams(azumiEventType.ToString(), paramList);
            //print("Analytics OnLevelRestart:" + Analytics.CustomEvent("Restart", paramList));
        }
        public void OnClearLevelRestarts(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            numberOfTimesRestarted = 0;

        }

        public void OnExitLevelEarly(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            paramList.Add("Level", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            paramList.Add("Time", timerDict[AnalyticsEventType.StartLevel].GetElapsedTime());
            Analytics.CustomEvent("Exit", paramList);
            //print("Analytics OnExitLevelEarly:" + Analytics.CustomEvent("Exit", paramList));
            //OutputParams(azumiEventType.ToString(), paramList);
        }

        public void OnSaveSettings(AzumiEventType azumiEventType, Component Sender, object Param = null)
        {
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            paramList.Add("NewMusic", SoundManager.MusicVolume);
            paramList.Add("NewFx", SoundManager.SoundFXVolume);
            paramList.Add("OldMusic", SoundManager.FormerMusicVolume);
            paramList.Add("OldFx", SoundManager.FormerSoundFXVolume);
            Analytics.CustomEvent("Settings", paramList);
            //print("Analytics OnSavedSettings:" + Analytics.CustomEvent("Settings", paramList));
            //OutputParams(azumiEventType.ToString(), paramList);
        }
        /*
        void OutputParams(string listID, Dictionary<string, object> paramList)
        {
            print("--- Start " + listID + " ---");

            foreach (KeyValuePair<string, object> entry in paramList)
            {
                print("    " + entry.Key + ": " + entry.Value.ToString());
            }
            print("--- end ---");


        }
*/
        void StartApp()
        {
            Dictionary<string, object> paramList = AddStandardParameters(new Dictionary<string, object>());
            Analytics.CustomEvent("StartApp", paramList);
            //print("Analytics StartApp:" + Analytics.CustomEvent("StartApp", paramList));
            //OutputParams("StartApp", paramList);
        }


        #endregion

    }
}
